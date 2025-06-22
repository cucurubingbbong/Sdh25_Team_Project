using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerSystem : MonoBehaviour
{
    public static CustomerSystem instance;

    [Header("UI ���")]
    public GameObject GamePanel;
    public GameObject panel;
    public Image portrait;
    public TMP_Text nameText;
    public TMP_Text wantText;
    public TMP_Text feelText;
    public TMP_Text dialogueText;
    public TMP_InputField offerPriceInput;
    public Button haggleButton;
    public Button sellButton;
    public Button skipButton;

    [Header("������")]
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
        GamePanel.SetActive(true);

        haggleButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();

        haggleButton.onClick.AddListener(OnClickHaggle);
        sellButton.onClick.AddListener(OnClickSell);
        skipButton.onClick.AddListener(OnClickSkip);

        NextTurn();
    }

    public void NextTurn()
    {
        if (turn >= maxTurns)
        {
            dialogueText.text = "���� �Ϸ簡 �������ϴ�.";
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
            dialogueText.text = "�ƹ� �մԵ� ���� �ʾҽ��ϴ�.";
            NextTurn();
        }
    }

    void SpawnCustomer()
    {
        baseCustomerData = allCustomers[Random.Range(0, allCustomers.Count)];
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

        // UI ������Ʈ
        portrait.sprite = baseCustomerData.portrait;
        nameText.text = baseCustomerData.customerName;

        string wantInfo = "";
        foreach (var pair in currentItemRequest)
        {
            wantInfo += $"- {pair.Key.itemName} x{pair.Value} ({pair.Key.price}G)\n";
        }
        wantText.text = "�䱸 ������:\n" + wantInfo;

        UpdateFeelUI();
        dialogueText.text = baseCustomerData.dialogue.greetLine;
        offerPriceInput.text = "";
    }

    void UpdateFeelUI()
    {
        feelText.text = $"���: {Mathf.RoundToInt(currentFeel)}";
    }

    void OnClickHaggle()
    {
        if (!int.TryParse(offerPriceInput.text, out int offerPrice) || offerPrice <= 0)
        {
            dialogueText.text = "������ ��Ȯ�� �Է��ϼ���.";
            return;
        }

        int baseTotalPrice = 0;
        foreach (var pair in currentItemRequest)
        {
            baseTotalPrice += pair.Key.price * pair.Value;
        }

        float priceRatio = (float)(offerPrice - baseTotalPrice) / baseTotalPrice;

        // ���ݿ� ���� ���� ������ ����
        float baseChance = 60f;
        float feelBonus = (currentFeel - 50f) * 0.5f;
        float pricePenalty = priceRatio * 100f;

        switch (baseCustomerData.customerType)
        {
            case CustomerType.BargainHunter:
                baseChance += 10f; // ���� �ߵ�
                break;
            case CustomerType.Collector:
                baseChance -= 10f; // ���� �����
                break;
            case CustomerType.Impulsive:
                baseChance += 20f; // ���� ������ �ߵ�
                break;
            case CustomerType.Thief:
                baseChance -= 15f; // �� �ȵ�
                break;
        }

        float successRate = Mathf.Clamp(baseChance + feelBonus - pricePenalty, 5f, 95f);
        float roll = Random.Range(0f, 100f);

        dialogueText.text = baseCustomerData.dialogue.haggleLine;

        if (roll <= successRate)
        {
            haggleSucceeded = true;
            dialogueText.text += "\n���� ����!";
        }
        else
        {
            currentFeel -= 10f;
            currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
            UpdateFeelUI();
            dialogueText.text = baseCustomerData.dialogue.failLine + "\n���� ����!";
        }
    }

    void OnClickSell()
    {
        if (!haggleSucceeded && baseCustomerData.customerType != CustomerType.Impulsive)
        {
            dialogueText.text = "������ ���� �����ؾ� �Ǹ��� �� �ֽ��ϴ�.";
            return;
        }

        if (!int.TryParse(offerPriceInput.text, out int finalPrice) || finalPrice <= 0)
        {
            dialogueText.text = "������ ��Ȯ�� �Է��ϼ���.";
            return;
        }

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
            dialogueText.text = "�䱸�� �������� �����մϴ�.";
            return;
        }

        foreach (var pair in currentItemRequest)
        {
            InventorySystem.instance.TryRemoveItem(pair.Key, pair.Value);
        }

        GoldManager.Instance.AddGold(finalPrice);
        dialogueText.text = baseCustomerData.dialogue.successLine + $"\n{finalPrice}G�� �������ϴ�.";

        // ������ ��� Ȯ���� ��ġ�� �����
        if (baseCustomerData.customerType == CustomerType.Thief && Random.value < 0.2f)
        {
            dialogueText.text = "�մ��� �����ƽ��ϴ�! ���� ���� �ʾҽ��ϴ�.";
            GoldManager.Instance.TrySpendGold(finalPrice);
        }

        NextTurn();
    }

    void OnClickSkip()
    {
        currentFeel -= 5f;
        currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
        UpdateFeelUI();
        dialogueText.text = baseCustomerData.dialogue.skipLine;
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
