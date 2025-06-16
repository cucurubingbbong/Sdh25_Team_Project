// CustomerSystem.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerSystem : MonoBehaviour
{
    public static CustomerSystem instance;
    [Header("UI요소")]
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
            Debug.Log("하루가 끝났습니다.");
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
            Debug.Log("손님 없음");
            panel.SetActive(false);
        }
    }

    void SpawnCustomer()
    {
        baseCustomerData = allCustomers[Random.Range(0, allCustomers.Count)];
        // feel 범위 0 이상으로 제한
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

        // UI 설정
        portrait.sprite = baseCustomerData.portrait;
        nameText.text = baseCustomerData.customerName;

        string wantInfo = "";
        foreach (var pair in currentItemRequest)
        {
            wantInfo += $"- {pair.Key.itemName} x{pair.Value} ({pair.Key.price}G)\n";
        }
        wantText.text = "요구 아이템:\n" + wantInfo;

        UpdateFeelUI();
        offerPriceInput.text = "";
        panel.SetActive(true);
    }

    void UpdateFeelUI()
    {
        feelText.text = $"기분: {Mathf.RoundToInt(currentFeel)}";
    }

    void OnClickHaggle()
    {
        if (!int.TryParse(offerPriceInput.text, out int offerPrice) || offerPrice <= 0)
        {
            Debug.Log("가격을 입력하세요.");
            return;
        }

        // 전체 요구 금액 계산
        int baseTotalPrice = 0;
        foreach (var pair in currentItemRequest)
        {
            baseTotalPrice += pair.Key.price * pair.Value;
        }

        float priceRatio = (float)(offerPrice - baseTotalPrice) / baseTotalPrice;

        // 성공 확률 계산
        float baseChance = 60f;
        float feelBonus = (currentFeel - 50f) * 0.5f;
        float pricePenalty = priceRatio * 100f;

        float successRate = Mathf.Clamp(baseChance + feelBonus - pricePenalty, 5f, 95f);
        float roll = Random.Range(0f, 100f);

        if (roll <= successRate)
        {
            haggleSucceeded = true;
            Debug.Log($"흥정 성공! {offerPrice}G에 판매 가능.");
        }
        else
        {
            currentFeel -= 10f;
            currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
            UpdateFeelUI();
            Debug.Log("흥정 실패! 기분 하락.");
        }
    }

    void OnClickSell()
    {
        if (!haggleSucceeded)
        {
            Debug.Log("흥정 먼저 성공해야 합니다.");
            return;
        }

        if (!int.TryParse(offerPriceInput.text, out int finalPrice) || finalPrice <= 0)
        {
            Debug.Log("가격을 입력하세요.");
            return;
        }

        // 요구한 아이템을 모두 판매 가능하도록 처리
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
            Debug.Log("요구한 아이템을 모두 가지고 있지 않습니다");
            return;
        }

        // 모두 인벤토리에서 제거하고 골드 추가
        foreach (var pair in currentItemRequest)
        {
            InventorySystem.instance.TryRemoveItem(pair.Key, pair.Value);
        }

        GoldManager.Instance.AddGold(finalPrice);
        Debug.Log($"{baseCustomerData.customerName}에게 아이템을 판매했습니다. {finalPrice}G 획득!");
        panel.SetActive(false);
        NextTurn();
    }

    void OnClickSkip()
    {
        currentFeel -= 5f;
        currentFeel = Mathf.Clamp(currentFeel, 0f, 100f);
        Debug.Log($"{baseCustomerData.customerName}을(를) 그냥 보냈습니다.");
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
