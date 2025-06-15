// Scripts/Systems/GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventorySystem inventory;

    public int currentDay = 1;

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
