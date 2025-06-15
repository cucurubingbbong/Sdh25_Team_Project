using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public Dictionary<ItemData, int> inventory = new();

    public void AddItem(ItemData item, int count)
    {
        if (inventory.ContainsKey(item))
            inventory[item] += count;
        else
            inventory[item] = count;
    }

    public bool TryRemoveItem(ItemData item, int count)
    {
        if (!inventory.ContainsKey(item) || inventory[item] < count)
            return false;

        inventory[item] -= count;
        if (inventory[item] <= 0)
            inventory.Remove(item);
        return true;
    }

    public int GetItemCount(ItemData item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

    public Dictionary<ItemData, int> GetAllItems()
    {
        return new Dictionary<ItemData, int>(inventory);
    }
}