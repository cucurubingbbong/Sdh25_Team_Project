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

    void Bonus(int bonusAmount) // 수정: 메서드에 int 매개변수를 추가하여 오류 해결  
    {
        Debug.Log($"보너스 골드를 획득 하였습니다: {bonusAmount}");
        GoldManager.Instance.AddGold(bonusAmount);
    }

    void ActMysterious()
    {
        Debug.Log("수상한 손님이 방문하였습니다");
        Debug.Log($"보너스 골드 +{amount}");
        GoldManager.Instance.AddGold(amount);
    }

    void Loss()
    {
        Debug.Log("골드를 잃었습니다");
        GoldManager.Instance.LossGold(amount);
    }

    void Reputation(int delta)
    {
        if (delta >= 0)
        {
            GManager.instance.reputation += delta;
            Debug.Log("시민들 사이에서 평판이 올랐습니다");
            Debug.Log($"+{delta}");
        }
        else
        {
            GManager.instance.reputation += delta;
            Debug.Log("시민들 사이에서 평판이 떨어졌습니다");
            Debug.Log($"{delta} 현재 평판: {GManager.instance.reputation}");
        }
    }
}
