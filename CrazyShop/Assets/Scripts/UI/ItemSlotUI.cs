using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [Header("UI구성")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    public ItemData itemData;



    public void Setup(ItemData data)
    {
        itemData = data;
        iconImage.sprite = itemData.icon;
        nameText.text = itemData.itemName;
        priceText.text = itemData.price.ToString() + " G";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        if (GoldManager.Instance.TrySpendGold(itemData.price))
        {
            Destroy(gameObject);
            Debug.Log($"구매: {itemData.itemName}");
            InventorySystem.instance.AddItem(itemData , 1);
        }
        else
        {
            Debug.Log("골드가 충분하지 않네요");
        }
    }
}
