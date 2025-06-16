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
    public TextMeshProUGUI reputationText;

    [Header("결산 데이터")]
    public int TodayProfit = 0; // 오늘 번 금액
    public int TodayLoss = 0;   //오늘 손해본 금액
    public int TotalMargin = 0; //전체 마진(누적임)
    public int EarnedUpgradePoints = 0; //오늘 얻은 업그레이드 포인트
    public int Reputation = 0;  //현재 평판 수치, 처음은 0으로 시작

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

    //결산 데이터를 업데이트 하고 화면에 표시
    public void ShowSettlement(int currentDay)
    {
        TotalMargin += (TodayProfit - TodayLoss);

        //포인트 및 편팡 계산
        EarnedUpgradePoints = (TodayProfit - TodayLoss) / 50;
        EarnedUpgradePoints = Mathf.Max(0, EarnedUpgradePoints); //손해 볼시 0pt
        Reputation += EarnedUpgradePoints;

        // UI
        settlementPanel.SetActive(true);
        dayText.text = $"Day {currentDay}";
        profitText.text = $" + {TodayProfit} G";
        lossText.text = $" - {TodayLoss} G";
        marginText.text = $"{TotalMargin} G";
        UpgradePointText.text = $"+ {EarnedUpgradePoints}";
        reputationText.text = $"Reputaion: {Reputation}";

        // 다음날 초기화
        TodayProfit = 0;
        TodayLoss = 0;
    }
    // 아이템 판매되었을때 수익 기록
    public void AddProfit(int amount)
    {
        TodayProfit += amount;
    }
    // 손해 발생시 기록됨 ( ex : 아이템 배분 비용)
    public void AddLoss(int amount)
    {
        TodayLoss += amount;
    }
}
