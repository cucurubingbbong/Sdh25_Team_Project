using UnityEngine;

public class Enemy : UnitBase
{
    public override void StartTurn()
    {
        base.StartTurn();

        Debug.Log("적 턴 시작!");

        int damage = MakeDamage(TurnManager.Instance.player);
        TurnManager.Instance.player.TakeDamage(damage);

        EndTurn();
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TurnManager.Instance.NextTurn();
    }
}
