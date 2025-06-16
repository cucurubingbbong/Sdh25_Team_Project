// CustomerSystem.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerSystem : MonoBehaviour
{
    public static CustomerSystem instance;
    [Header("UI���")]
    public GameObject GamePanel;
    public GameObject panel;
    public Image portrait;
    public TMP_Text nameText;
    public TMP_Text wantText;
    public TMP_Text feelText;
    public TMP_InputField offerPriceInput;
    public Button haggleButton;
    public Button sellButton;
    public Button skipButton;

    [Header("Data")]
    public List<CustomerData> allCustomers;
    public CustomerData baseCustomerData;
    public Dictionary<ItemData, int> currentItemRequest;
    public float currentFeel;
    public bool haggleSucceeded = false;

    public int turn = 0;
    public int maxTurns = 10;

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

    public void TurnStart()
    {
        panel.SetActive(true);
        GamePanel.SetActive(true );
        haggleButton.onClick.AddListener(OnClickHaggle);
        sellButton.onClick.AddListener(OnClickSell);
        skipButton.onClick.AddListener(OnClickSkip);

        NextTurn();
    }

    public void NextTurn()
    {
        if (turn >= maxTurns)
        {
            Debug.Log("�Ϸ簡 �������ϴ�.");
            panel.SetActive(false);
            return;
        }

        turn++;
        float chance = Random.Range(0f, 1f);
        if (chance < 0.7f)
        {
            SpawnCustomer();
        }
        else
        {
            Debug.Log("�մ� ����");
            panel.SetActive(false);
        }
    }

    void SpawnCustomer()
    {
        baseCustomerData = allCustomers[Random.Range(0, allCustomers.Count)];
        // feel ���� 0 �̻����� ����
        currentFeel = Mathf.Clamp(Random.Range(baseCustomerData.feel - 20f, baseCustomerData.feel + 20f), 0f, 100f);
        haggleSucceeded = false;

        // ������ �䱸 ������ ����
        currentItemRequest = new Dictionary<ItemData, int>();
        int requestCount = Random.Range(1, 4); 

        List<ItemData> shuffled = new List<ItemData>(baseCustomerData.preferredItems);
        Shuffle(shuffled);

        for (int i = 0; i < Mathf.Min(requestCount, shuffled.Count); i++)
        {
            ItemData item = shuffled[i];
            int quantity = Random.Range(1, 4); 
            currentItemRequest[item] = quantity;
        }

        // UI ����
        portrait.sprite = baseCustomerData.portrait;
        nameText.text = baseCustomerData.customerName;

        string wantInfo = "";
        foreach (var pair in currentItemRequest)
        {
            wantInfo += $"- {pair.Key.itemName} x{pair.Value} ({pair.Key.price}G)\n";
        }
        wantText.text = "�䱸 ������:\n" + wantInfo;

        UpdateFeelUI();
        offerPriceInput.text = "";
        panel.SetActive(true);
    }

    void UpdateFeelUI()
    {
        feelText.text = $"���: {Mathf.RoundToInt(currentFeel)}";
    }

    void OnClickHaggle()
    {
        if (!int.TryParse(offerPriceInput.text, out int offerPrice) || offerPrice <= 0)
        {
            Debug.Log("������ �Է��ϼ���.");
            return;
        }

        // ��ü �䱸 �ݾ� ���
        int baseTotalPrice = 0;
        foreach (var pair in currentItemRequest)
        {
            baseTotalPrice += pair.Key.price * pair.Value;
        }

        float priceRatio = (float)(offerPrice - baseTotalPrice) / baseTotalPrice;

        // ���� Ȯ�� ���
        float baseChance = 60f;
        float feelBonus = (currentFeel - 50f) * 0.5f;
        float pricePenalty = priceRatio * 100f;

        float successRate = Mathf.Clamp(baseChance + feelBonus - pricePenalty, 5f, 95f);
        float roll = Random.Range(0f, 100f);

        if (roll <= successRate)
        {
            haggleSucceeded = true;
            Debug.Log($"���� ����! {offerPrice}G�� �Ǹ� ����.");
        }
        else
        {
            currentFeel -= 10f;
            currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
            UpdateFeelUI();
            Debug.Log("���� ����! ��� �϶�.");
        }
    }

    void OnClickSell()
    {
        if (!haggleSucceeded)
        {
            Debug.Log("���� ���� �����ؾ� �մϴ�.");
            return;
        }

        if (!int.TryParse(offerPriceInput.text, out int finalPrice) || finalPrice <= 0)
        {
            Debug.Log("������ �Է��ϼ���.");
            return;
        }

        // �䱸�� �������� ��� �Ǹ� �����ϵ��� ó��
        bool hasAllItems = true;
        foreach (var pair in currentItemRequest)
        {
            if (InventorySystem.instance.GetItemCount(pair.Key) < pair.Value)
            {
                hasAllItems = false;
                break;
            }
        }

        if (!hasAllItems)
        {
            Debug.Log("�䱸�� �������� ��� ������ ���� �ʽ��ϴ�");
            return;
        }

        // ��� �κ��丮���� �����ϰ� ��� �߰�
        foreach (var pair in currentItemRequest)
        {
            InventorySystem.instance.TryRemoveItem(pair.Key, pair.Value);
        }

        GoldManager.Instance.AddGold(finalPrice);
        Debug.Log($"{baseCustomerData.customerName}���� �������� �Ǹ��߽��ϴ�. {finalPrice}G ȹ��!");
        panel.SetActive(false);
        NextTurn();
    }

    void OnClickSkip()
    {
        currentFeel -= 5f;
        currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
        Debug.Log($"{baseCustomerData.customerName}��(��) �׳� ���½��ϴ�.");
        panel.SetActive(false);
        NextTurn();
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
