using UnityEngine;

public class Player : UnitBase
{
    public override void StartTurn()
    {
        base.StartTurn();
        Debug.Log("플레이어 턴 시작!");
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TurnManager.Instance.NextTurn();
    }
}
