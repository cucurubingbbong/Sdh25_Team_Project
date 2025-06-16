using System.Collections.Generic;
using UnityEngine;

public class DistributeManager : MonoBehaviour
{
    public GameObject stockPanel;
    public GameObject ItemParent;
    public GameObject itemSlotPrefab;
    public List<ItemData> itemPool; 

    public int cost = 30;
    public bool alreadyDistributed = false;

    public static DistributeManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void Distribute()
    {
        if (alreadyDistributed) return;

        if (!GoldManager.Instance.TrySpendGold(cost))
        {
            Debug.Log("∞ÒµÂ ∫Œ¡∑");
            return;
        }

        alreadyDistributed = true;
        GenerateStock();
    }

    private void GenerateStock()
    {
        stockPanel.SetActive(true);
        foreach (Transform child in ItemParent.transform)
        {
            Destroy(child.gameObject); 
        }

        int count = Random.Range(3, 6); 
        for (int i = 0; i < count; i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, ItemParent.transform);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            ItemData randomItem = itemPool[Random.Range(0, itemPool.Count)];
            slotUI.Setup(randomItem);
        }
    }

    public void EndDistribute()
    {
        stockPanel.SetActive(false);
    }

    public void ResetDailyDistribution()
    {
        alreadyDistributed = false;
    }
}
