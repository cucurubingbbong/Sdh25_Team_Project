using UnityEngine;

public class Player : UnitBase
{
    public override void StartTurn()
    {
        base.StartTurn();
        Debug.Log("�÷��̾� �� ����!");
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TurnManager.Instance.NextTurn();
    }
}
