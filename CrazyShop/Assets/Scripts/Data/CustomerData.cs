using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerDialogueSet
{
    [TextArea] public string greetLine;
    [TextArea] public string haggleLine;
    [TextArea] public string successLine;
    [TextArea] public string failLine;
    [TextArea] public string skipLine;
}

[CreateAssetMenu(fileName = "CustomerData", menuName = "Game/Customer")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public CustomerType customerType;
    public Sprite portrait;
    public List<ItemData> preferredItems;
    public float feel;
    public int gold;
    public CustomerDialogueSet dialogue;
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
