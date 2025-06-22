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

    [Header("��� ������")]
    public int TodayProfit = 0; // ���� �� �ݾ�
    public int TodayLoss = 0;   //���� ���غ� �ݾ�
    public int TotalMargin = 0; //��ü ����(������)
    public int EarnedUpgradePoints = 0; //���� ���� ���׷��̵� ����Ʈ

    public static DailySettlement instance;


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


        //��� �����͸� ������Ʈ �ϰ� ȭ�鿡 ǥ��
        public void ShowSettlement(int currentDay)
        {
        TotalMargin += (TodayProfit - TodayLoss);

        // ���׷��̵� ����Ʈ ���
        EarnedUpgradePoints = (TodayProfit - TodayLoss) / 50;
        EarnedUpgradePoints = Mathf.Max(0, EarnedUpgradePoints); //���� ���� 0pt

        // UI
        settlementPanel.SetActive(true);
        CustomerSystem.instance.GamePanel.SetActive(false);
        dayText.text = $"{currentDay}����";
        profitText.text = $"���� : {TodayProfit} G";
        lossText.text = $" ���� : {TodayLoss} G";
        marginText.text = $" ���� : {TotalMargin} G";
        UpgradePointText.text = $"���׷��̵� ����Ʈ + {EarnedUpgradePoints}";

        // ������ �ʱ�ȭ
        TodayProfit = 0;
        TodayLoss = 0;
    }
}
