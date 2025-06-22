using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerSystem : MonoBehaviour
{
    public static CustomerSystem instance;

    [Header("UI 요소")]
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

    [Header("데이터")]
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
            dialogueText.text = "오늘 하루가 끝났습니다.";
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
            dialogueText.text = "아무 손님도 오지 않았습니다.";
            NextTurn();
        }
    }

    void SpawnCustomer()
    {
        baseCustomerData = allCustomers[Random.Range(0, allCustomers.Count)];
        currentFeel = Mathf.Clamp(Random.Range(baseCustomerData.feel - 20f, baseCustomerData.feel + 20f), 0f, 100f);
        haggleSucceeded = false;

        // 무작위 요구 아이템 설정
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

        // UI 업데이트
        portrait.sprite = baseCustomerData.portrait;
        nameText.text = baseCustomerData.customerName;

        string wantInfo = "";
        foreach (var pair in currentItemRequest)
        {
            wantInfo += $"- {pair.Key.itemName} x{pair.Value} ({pair.Key.price}G)\n";
        }
        wantText.text = "요구 아이템:\n" + wantInfo;

        UpdateFeelUI();
        dialogueText.text = baseCustomerData.dialogue.greetLine;
        offerPriceInput.text = "";
    }

    void UpdateFeelUI()
    {
        feelText.text = $"기분: {Mathf.RoundToInt(currentFeel)}";
    }

    void OnClickHaggle()
    {
        if (!int.TryParse(offerPriceInput.text, out int offerPrice) || offerPrice <= 0)
        {
            dialogueText.text = "가격을 정확히 입력하세요.";
            return;
        }

        int baseTotalPrice = 0;
        foreach (var pair in currentItemRequest)
        {
            baseTotalPrice += pair.Key.price * pair.Value;
        }

        float priceRatio = (float)(offerPrice - baseTotalPrice) / baseTotalPrice;

        // 성격에 따라 흥정 성공률 조정
        float baseChance = 60f;
        float feelBonus = (currentFeel - 50f) * 0.5f;
        float pricePenalty = priceRatio * 100f;

        switch (baseCustomerData.customerType)
        {
            case CustomerType.BargainHunter:
                baseChance += 10f; // 흥정 잘됨
                break;
            case CustomerType.Collector:
                baseChance -= 10f; // 흥정 어려움
                break;
            case CustomerType.Impulsive:
                baseChance += 20f; // 흥정 무조건 잘됨
                break;
            case CustomerType.Thief:
                baseChance -= 15f; // 잘 안됨
                break;
        }

        float successRate = Mathf.Clamp(baseChance + feelBonus - pricePenalty, 5f, 95f);
        float roll = Random.Range(0f, 100f);

        dialogueText.text = baseCustomerData.dialogue.haggleLine;

        if (roll <= successRate)
        {
            haggleSucceeded = true;
            dialogueText.text += "\n흥정 성공!";
        }
        else
        {
            currentFeel -= 10f;
            currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
            UpdateFeelUI();
            dialogueText.text = baseCustomerData.dialogue.failLine + "\n흥정 실패!";
        }
    }

    void OnClickSell()
    {
        if (!haggleSucceeded && baseCustomerData.customerType != CustomerType.Impulsive)
        {
            dialogueText.text = "흥정에 먼저 성공해야 판매할 수 있습니다.";
            return;
        }

        if (!int.TryParse(offerPriceInput.text, out int finalPrice) || finalPrice <= 0)
        {
            dialogueText.text = "가격을 정확히 입력하세요.";
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
            dialogueText.text = "요구한 아이템이 부족합니다.";
            return;
        }

        foreach (var pair in currentItemRequest)
        {
            InventorySystem.instance.TryRemoveItem(pair.Key, pair.Value);
        }

        GoldManager.Instance.AddGold(finalPrice);
        dialogueText.text = baseCustomerData.dialogue.successLine + $"\n{finalPrice}G를 벌었습니다.";

        // 도둑일 경우 확률로 훔치고 사라짐
        if (baseCustomerData.customerType == CustomerType.Thief && Random.value < 0.2f)
        {
            dialogueText.text = "손님이 도망쳤습니다! 돈을 주지 않았습니다.";
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
