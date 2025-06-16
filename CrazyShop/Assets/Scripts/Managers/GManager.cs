// Scripts/Systems/GameManager.cs
using UnityEngine;

public class GManager : MonoBehaviour
{
    public InventorySystem inventory;

    public int currentDay = 1;

    public int reputation = 0;

    public bool Distribute  = false;

    public static GManager instance;
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
        StartDay();
    }

    public void StartDay()
    {
        Debug.Log($" Day {currentDay} 시작");
        DistributeManager.instance.Distribute();
    }

    public void EndDay()
    {
        Debug.Log($"Day {currentDay} 종료");
        currentDay++;
        StartDay();
        DistributeManager.instance.ResetDailyDistribution();
    }

}
