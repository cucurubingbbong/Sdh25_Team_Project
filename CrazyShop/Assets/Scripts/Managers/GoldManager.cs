using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int currentGold = 100;
    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); ;
    }

    void Start()
    {
        UpdateGoldUI();
    }

    public bool TrySpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            DailySettlement.instance.TodayLoss += amount;
            UpdateGoldUI();
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        DailySettlement.instance.TodayProfit += amount; ;
        UpdateGoldUI();
    }
    public void LossGold(int amount)
    {
        currentGold -= amount;
                    DailySettlement.instance.TodayLoss += amount;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        goldText.text = $"Gold: {currentGold}";
    }
}
