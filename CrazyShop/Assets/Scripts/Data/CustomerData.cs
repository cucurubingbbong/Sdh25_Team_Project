using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Game/Customer")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public Sprite portrait;
    public List<ItemData> preferredItems;
}
