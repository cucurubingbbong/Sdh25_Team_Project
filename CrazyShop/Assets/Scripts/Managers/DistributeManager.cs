using System.Collections.Generic;
using UnityEngine;

public class DistributeManager : MonoBehaviour
{
    public GameObject stockPanel;  // StockPanel 참조
    public GameObject itemSlotPrefab;
    public List<ItemData> itemPool; // 가능한 아이템 리스트

    private const int cost = 30;
    private bool alreadyDistributed = false;

    public void OnClickDistribute()
    {
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
        foreach (Transform child in stockPanel.transform)
        {
            Destroy(child.gameObject); // 기존 슬롯 제거
        }

        int count = Random.Range(3, 6); // 3~5개 랜덤
        for (int i = 0; i < count; i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, stockPanel.transform);
            ItemSlotUI slotUI = slot.GetComponent<ItemSlotUI>();
            ItemData randomItem = itemPool[Random.Range(0, itemPool.Count)];
            slotUI.Setup(randomItem);
        }
    }

    public void ResetDailyDistribution()
    {
        alreadyDistributed = false;
    }
}
