using UnityEngine;

public class Enemy : MonoBehaviour, IDamageDealer, IDamageReceiver
{
    public string unitName;
    public CharacterStats baseStats;
    public float currentHp;

    private void Start()
    {
        currentHp = baseStats.maxHp;
    }

    public int CalculateDamage(IDamageReceiver target)
    {
        float damage = baseStats.attackPower;

        // ũ��Ƽ�� ���
        if (Random.value < baseStats.criticalChance)
        {
            damage *= baseStats.criticalDamage;
            Debug.Log($"{unitName}��(��) ũ��Ƽ�� ����!");
        }

        damage *= Random.Range(0.9f, 1.1f);

        // ���� ����
        int finalDamage = Mathf.RoundToInt(damage) - target.GetDefense();
        return Mathf.Max(finalDamage, 0);
    }

  
    public void TakeDamage(int damage)
    {
        Debug.Log($"{unitName}��(��) {damage} �������� �Ծ����ϴ�!");
        currentHp -= damage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public int GetDefense()
    {
        return baseStats.defensePower;
    }

    private void Die()
    {
        Debug.Log($"{unitName}��(��) ���������ϴ�.");
    }
}
