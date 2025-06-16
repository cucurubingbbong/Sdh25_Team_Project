// CustomerData.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Game/Customer")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public CustomerType customerType;
    public Sprite portrait;

    public List<ItemData> preferredItems;

    [System.Serializable]
    public class ItemAmount
    {
        public ItemData item;
        public int amount;
    }

    // 인스펙터에서 편집하기 위한 리스트
    public List<ItemAmount> requiredItemsList = new();

    // 딕셔너리로 변환
    public Dictionary<ItemData, int> requiredItemsDict;

    public float feel;
    public int gold;
}

public enum CustomerType
{
    Regular,
    BargainHunter,
    Collector,
    Impulsive,
    Thief,
    Mysterious,
}
