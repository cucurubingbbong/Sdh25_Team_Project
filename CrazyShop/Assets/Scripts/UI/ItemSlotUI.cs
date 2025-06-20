using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [Header("UI ����")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI countText;
    public Button buyButton;

    public ItemData itemData;
    public int itemCount; // ���� ���� ����

    public void Setup(ItemData data, int count)
    {
        itemData = data;
        itemCount = count;

        iconImage.sprite = itemData.icon;
        nameText.text = itemData.itemName;
        priceText.text = itemData.price + " G";
        countText.text = $"x{itemCount}";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        if (itemCount <= 0)
        {
            Debug.Log("��� ����");
            return;
        }

        if (GoldManager.Instance.TrySpendGold(itemData.price))
        {
            itemCount--;
            InventorySystem.instance.AddItem(itemData, 1);
            Debug.Log($"����: {itemData.itemName}");

            if (itemCount <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                countText.text = $"x{itemCount}";
            }
        }
        else
        {
            Debug.Log("��尡 �����մϴ�");
        }
    }
}
