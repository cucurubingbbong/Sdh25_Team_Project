using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public string unitName;
    public CharacterStats statData;

    public float nowHp;

    protected List<StatusEffect> statusList = new List<StatusEffect>();

    protected virtual void Start()
    {
        nowHp = statData.maxHp;
    }

    public virtual void TakeDamage(int damage)
    {
        nowHp -= damage;
        if (nowHp <= 0)
        {
            nowHp = 0;
            Die();
        }
    }

    public int GetDefense()
    {
        return statData.defensePower;
    }

    public virtual int MakeDamage(UnitBase target)
    {
        float dmg = statData.attackPower;

        if (Random.value < statData.criticalChance)
        {
            dmg *= statData.criticalDamage;
            Debug.Log(unitName + " ũ��Ƽ�� ��Ʈ!");
        }

        dmg *= Random.Range(0.9f, 1.1f);

        int finalDmg = Mathf.RoundToInt(dmg) - target.GetDefense();
        return Mathf.Max(finalDmg, 0);
    }

    public virtual void Die()
    {
        Debug.Log(unitName + " ����...");
    }

    public virtual void StartTurn()
    {
        for (int i = 0; i < statusList.Count; i++)
        {
            statusList[i].OnTurnStart(this);
        }
    }

    public virtual void EndTurn()
    {
        for (int i = 0; i < statusList.Count; i++)
        {
            statusList[i].OnTurnEnd(this);
        }

        // �� ���� �� ���� �ð� ���� �� ����
        statusList.RemoveAll(status => status.duration <= 0);
    }

    public void AddStatus(StatusEffect newStatus)
    {
        statusList.Add(newStatus);
    }
}
