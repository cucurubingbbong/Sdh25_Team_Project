using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite icon;
}


[CreateAssetMenu(menuName = "Game/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;
}