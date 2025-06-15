// Scripts/Systems/GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventorySystem inventory;
    public ItemDatabase itemDatabase;

    public int currentDay = 1;

    private void Start()
    {
        StartDay();
    }

    public void StartDay()
    {
        Debug.Log($" Day {currentDay} 시작");
        // 유통 재고 구매 단계
    }

    public void EndDay()
    {
        Debug.Log($"Day {currentDay} 종료");
        currentDay++;
        StartDay();
    }

}
