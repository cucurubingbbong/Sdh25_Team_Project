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
        Debug.Log($" Day {currentDay} ����");
        // ���� ��� ���� �ܰ�
    }

    public void EndDay()
    {
        Debug.Log($"Day {currentDay} ����");
        currentDay++;
        StartDay();
    }

}
