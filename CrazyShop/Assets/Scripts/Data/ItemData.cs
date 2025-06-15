using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite icon;
}

