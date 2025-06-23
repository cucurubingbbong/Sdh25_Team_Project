using TMPro;
using UnityEditor;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public InventorySystem inventory;

    public int currentDay = 1;

    public int reputation = 0;

    public bool Distribute  = false;

    public static GManager instance;

    public TextMeshProUGUI repText;

    public TextMeshProUGUI dayText;

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

    private void Start()
    {
        EventManager.instance.TryRunEvent(0);
    }

    public void Update()
    {
        repText.text = reputation.ToString();
        dayText.text = $"Day : {currentDay}";

    }

    public void StartDay()
    {
        currentDay++;
        Debug.Log($" Day {currentDay} 시작");
        DistributeManager.instance.Distribute();
    }

    public void EndDay()
    {
        Debug.Log($"Day {currentDay} 종료");
        StartDay();
        DistributeManager.instance.ResetDailyDistribution();
    }

}
