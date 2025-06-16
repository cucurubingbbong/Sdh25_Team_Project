using UnityEngine;
using TMPro;

public class DailySettlement : MonoBehaviour
{
    [Header("��� UI")]
    public GameObject settlementPanel;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI lossText;
    public TextMeshProUGUI marginText;
    public TextMeshProUGUI UpgradePointText;
    public TextMeshProUGUI reputationText;

    [Header("��� ������")]
    public int TodayProfit = 0; // ���� �� �ݾ�
    public int TodayLoss = 0;   //���� ���غ� �ݾ�
    public int TotalMargin = 0; //��ü ����(������)
    public int EarnedUpgradePoints = 0; //���� ���� ���׷��̵� ����Ʈ
    public int Reputation = 0;  //���� ���� ��ġ, ó���� 0���� ����

    public static DailySettlement Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //��� �����͸� ������Ʈ �ϰ� ȭ�鿡 ǥ��
    public void ShowSettlement(int currentDay)
    {
        TotalMargin += (TodayProfit - TodayLoss);

        //����Ʈ �� ���� ���
        EarnedUpgradePoints = (TodayProfit - TodayLoss) / 50;
        EarnedUpgradePoints = Mathf.Max(0, EarnedUpgradePoints); //���� ���� 0pt
        Reputation += EarnedUpgradePoints;

        // UI
        settlementPanel.SetActive(true);
        dayText.text = $"Day {currentDay}";
        profitText.text = $" + {TodayProfit} G";
        lossText.text = $" - {TodayLoss} G";
        marginText.text = $"{TotalMargin} G";
        UpgradePointText.text = $"+ {EarnedUpgradePoints}";
        reputationText.text = $"Reputaion: {Reputation}";

        // ������ �ʱ�ȭ
        TodayProfit = 0;
        TodayLoss = 0;
    }
    // ������ �ǸŵǾ����� ���� ���
    public void AddProfit(int amount)
    {
        TodayProfit += amount;
    }
    // ���� �߻��� ��ϵ� ( ex : ������ ��� ���)
    public void AddLoss(int amount)
    {
        TodayLoss += amount;
    }
}
