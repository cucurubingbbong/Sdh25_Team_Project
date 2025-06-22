using UnityEngine;
using TMPro;

public class DailySettlement : MonoBehaviour
{
    [Header("결산 UI")]
    public GameObject settlementPanel;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI lossText;
    public TextMeshProUGUI marginText;
    public TextMeshProUGUI UpgradePointText;

    [Header("결산 데이터")]
    public int TodayProfit = 0; // 오늘 번 금액
    public int TodayLoss = 0;   //오늘 손해본 금액
    public int TotalMargin = 0; //전체 마진(누적임)
    public int EarnedUpgradePoints = 0; //오늘 얻은 업그레이드 포인트

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


        //결산 데이터를 업데이트 하고 화면에 표시
        public void ShowSettlement(int currentDay)
        {
        TotalMargin += (TodayProfit - TodayLoss);

        // 업그레이드 포인트 계산
        EarnedUpgradePoints = (TodayProfit - TodayLoss) / 50;
        EarnedUpgradePoints = Mathf.Max(0, EarnedUpgradePoints); //손해 볼시 0pt

        // UI
        settlementPanel.SetActive(true);
        CustomerSystem.instance.GamePanel.SetActive(false);
        dayText.text = $"{currentDay}일차";
        profitText.text = $"이익 : {TodayProfit} G";
        lossText.text = $" 손해 : {TodayLoss} G";
        marginText.text = $" 마진 : {TotalMargin} G";
        UpgradePointText.text = $"업그레이드 포인트 + {EarnedUpgradePoints}";

        // 다음날 초기화
        TodayProfit = 0;
        TodayLoss = 0;
    }
}
