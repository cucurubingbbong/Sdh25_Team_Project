using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class RanEvent : MonoBehaviour
{
    public static RanEvent instance;
    public int rand = Random.Range(0, 100);
    public int delta;

    public int amount;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void TriggerRanEvent()
    {
        int rand = Random.Range(0, 100);
        if (rand < 25)
        {
            ActMysterious();
        }
        else if (rand < 50)
        {
            Bonus(Random.Range(200, 500));
        }
        else if (rand < 75)
        {
            Loss();
        }
        else
        {
            Reputation(Random.Range(-5, 5));
        }
    }

    void Bonus(int bonusAmount) // ����: �޼��忡 int �Ű������� �߰��Ͽ� ���� �ذ�  
    {
        Debug.Log($"���ʽ� ��带 ȹ�� �Ͽ����ϴ�: {bonusAmount}");
        GoldManager.Instance.AddGold(bonusAmount);
    }

    void ActMysterious()
    {
        Debug.Log("������ �մ��� �湮�Ͽ����ϴ�");
        Debug.Log($"���ʽ� ��� +{amount}");
        GoldManager.Instance.AddGold(amount);
    }

    void Loss()
    {
        Debug.Log("��带 �Ҿ����ϴ�");
        GoldManager.Instance.LossGold(amount);
    }

    void Reputation(int delta)
    {
        if (delta >= 0)
        {
            GManager.instance.reputation += delta;
            Debug.Log("�ùε� ���̿��� ������ �ö����ϴ�");
            Debug.Log($"+{delta}");
        }
        else
        {
            GManager.instance.reputation += delta;
            Debug.Log("�ùε� ���̿��� ������ ���������ϴ�");
            Debug.Log($"{delta} ���� ����: {GManager.instance.reputation}");
        }
    }
}
