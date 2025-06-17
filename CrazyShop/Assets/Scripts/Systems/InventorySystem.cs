// InventorySystem.cs
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public Dictionary<ItemData, int> inventory = new();

    public GameObject InvenParent;
    public GameObject InvenSlot;
    //public GameObject InvenIMG;

    public bool OnInventroy;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

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

    public void OnInventory()
    {
        if (!OnInventroy)
        {
            InvenParent.SetActive(true);
            OnInventroy = true;
            //InvenIMG.SetActive(true);
        }
        else
        {
            OnInventroy = false;
            InvenParent.SetActive(false);
            //InvenIMG.SetActive(false);
        }
        foreach (Transform child in InvenParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<ItemData, int> pair in inventory)
        {
            GameObject slot = Instantiate(InvenSlot, InvenParent.transform);
            InvenSlotUI slotUI = slot.GetComponent<InvenSlotUI>();
            slotUI.InvenSetup(pair.Key, pair.Value);
        }
    }
}
