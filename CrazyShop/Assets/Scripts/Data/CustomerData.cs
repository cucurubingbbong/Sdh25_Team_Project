using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Game/Customer")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public CustomerType customerType;
    public Sprite portrait;
    public List<ItemData> preferredItems;
}

public enum CustomerType
{
    Regular,      // 일반 손님, 무난하게 아이템 구매
    BargainHunter, // 흥정 잘하는 손님, 가격 깎으려 함
    Collector,    // 특정 희귀 아이템만 찾는 손님
    Impulsive,    // 충동구매형, 이상한 아이템도 막 사감
    Thief,        // 훔치려고 시도하는 손님
    Mysterious,   // 정체 불명, 특수한 이벤트를 일으킬 수 있음
}
