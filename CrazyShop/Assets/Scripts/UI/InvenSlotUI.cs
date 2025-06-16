using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvenSlotUI : MonoBehaviour
{
    [Header("UI±¸¼º")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI itemCount;

    public ItemData itemData;



    public void InvenSetup(ItemData item, int count)
    {
        itemData = item;
        iconImage.sprite = itemData.icon;
        nameText.text = itemData.itemName;
        priceText.text = itemData.price.ToString() + " G";
        itemCount.text = count.ToString();
    }
}
