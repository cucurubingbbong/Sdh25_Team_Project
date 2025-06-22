using System.Collections;
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
    public Sprite[] Feel_Icon;
    public Image RealFeel;
    public TMP_Text nameText;
    public TMP_Text wantText;
    public TMP_Text feelText;
    public TMP_Text dialogueText;
    public TMP_Text totalPriceText;         
    public TMP_Text expectedSuccessText;    
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
    public int maxTurns = 12;

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

        StartNextTurn();
    }

    public void StartNextTurn()
    {
        StartCoroutine(NextTurn()); 
    }
    
    IEnumerator NextTurn()
    {
        portrait.sprite = null;
        portrait.gameObject.SetActive(false);
        dialogueText.text = "";
        wantText.text = "";
        feelText.text = "";
        totalPriceText.text = "";
        expectedSuccessText.text = "";

        yield return new WaitForSeconds(2f); 

        portrait.gameObject.SetActive(true);

        if (turn >= maxTurns)
        {
            dialogueText.text = "���� �Ϸ簡 �������ϴ�.";
            panel.SetActive(false);
            DailySettlement.instance.ShowSettlement(GManager.instance.currentDay);
            yield break;
        }

        turn++;
        float chance = Random.Range(0f, 1f);
        if (chance < 0.5 + GManager.instance.reputation * 0.05)
        {
            SpawnCustomer();
        }
        else
        {
            dialogueText.text = "�ƹ� �մԵ� ���� �ʾҽ��ϴ�.";
            yield return new WaitForSeconds(1f); 
            StartNextTurn(); 
        }
    }


    void SpawnCustomer()
    {
        baseCustomerData = allCustomers[Random.Range(0, allCustomers.Count)];
        currentFeel = Mathf.Clamp(Random.Range(baseCustomerData.feel - 20f, baseCustomerData.feel + 20f), 0f, 100f);
        haggleSucceeded = false;

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

        portrait.sprite = baseCustomerData.portrait;
        nameText.text = baseCustomerData.customerName;

        string wantInfo = "";
        int totalPrice = 0;
        foreach (var pair in currentItemRequest)
        {
            wantInfo += $"- {pair.Key.itemName} x{pair.Value} ({pair.Key.price}G)\n";
            totalPrice += pair.Key.price * pair.Value;
        }

        wantText.text = "�䱸 ������:\n" + wantInfo;
        totalPriceText.text = $"�� ����: {totalPrice}G";
        expectedSuccessText.text = "���� Ȯ��: -";
        expectedSuccessText.color = Color.white;

        UpdateFeelUI();
        dialogueText.text = baseCustomerData.dialogue.greetLine;
        offerPriceInput.text = "";
    }

    void UpdateFeelUI()
    {
        feelText.text = $"���: {Mathf.RoundToInt(currentFeel)}";
        if (currentFeel < 75) RealFeel.sprite = Feel_Icon[1];
        else if (currentFeel < 25) RealFeel.sprite = Feel_Icon[2];
        else RealFeel.sprite = Feel_Icon[1];
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
        float baseChance = 60f;
        float feelBonus = (currentFeel - 50f) * 0.5f;
        float pricePenalty = priceRatio * 100f;

        switch (baseCustomerData.customerType)
        {
            case CustomerType.BargainHunter:
                baseChance += 10f; break;
            case CustomerType.Collector:
                baseChance -= 10f; break;
            case CustomerType.Impulsive:
                baseChance += 20f; break;
            case CustomerType.Thief:
                baseChance -= 15f; break;
        }

        float successRate = Mathf.Clamp(baseChance + feelBonus - pricePenalty, 0.1f, 99f);
        expectedSuccessText.text = $"���� Ȯ��: {Mathf.RoundToInt(successRate)}%";

        if (successRate >= 70f)
            expectedSuccessText.color = Color.green;
        else if (successRate >= 40f)
            expectedSuccessText.color = Color.yellow;
        else
            expectedSuccessText.color = Color.red;

        float roll = Random.Range(0f, 100f);
        if (roll <= successRate)
        {
            haggleSucceeded = true;
            UpdateFeelUI();
            dialogueText.text = baseCustomerData.dialogue.haggleLine;
        }
        else
        {
            currentFeel -= 5f;
            GManager.instance.reputation -= 5;
            GManager.instance.reputation = Mathf.Clamp(GManager.instance.reputation, 0, 100);
            currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
            UpdateFeelUI();
            dialogueText.text = baseCustomerData.dialogue.failLine;
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
        dialogueText.text = baseCustomerData.dialogue.successLine;

        if (baseCustomerData.customerType == CustomerType.Thief && Random.value < 0.2f)
        {
            dialogueText.text = "�մ��� �����ƽ��ϴ�! ���� ���� �ʾҽ��ϴ�.";
            GoldManager.Instance.TrySpendGold(finalPrice);
        }

        StartNextTurn();
    }

    void OnClickSkip()
    {
        dialogueText.text = baseCustomerData.dialogue.skipLine;
        currentFeel -= 5f;
        currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
        UpdateFeelUI();
        StartNextTurn();
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
