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
        stockPanel.SetActive(true);
        if (alreadyDistributed) return;

        if (!GoldManager.Instance.TrySpendGold(cost))
        {
            Debug.Log("골드 부족");
            return;
        }

        alreadyDistributed = true;
        GenerateStock();
    }

    private void GenerateStock()
    {
        stockPanel.SetActive(true);

        // 기존 슬롯 제거
        foreach (Transform child in ItemParent.transform)
        {
            Destroy(child.gameObject);
        }

        // 아이템 수량을 Dictionary로 정리
        Dictionary<ItemData, int> stockDict = new Dictionary<ItemData, int>();
        int count = Random.Range(15, 30);

        for (int i = 0; i < count; i++)
        {
            ItemData randomItem = itemPool[Random.Range(0, itemPool.Count)];

            if (stockDict.ContainsKey(randomItem))
            {
                stockDict[randomItem]++;
            }
            else
            {
                stockDict[randomItem] = 1;
            }
        }

        // 슬롯 생성 (아이템 + 수량)
        foreach (KeyValuePair<ItemData, int> pair in stockDict)
        {
            GameObject slot = Instantiate(itemSlotPrefab, ItemParent.transform);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            slotUI.Setup(pair.Key, pair.Value);
        }
    }


    public void EndDistribute()
    {
        stockPanel.SetActive(false);
        CustomerSystem.instance.TurnStart();
    }

    public void ResetDailyDistribution()
    {
        alreadyDistributed = false;
    }
}
