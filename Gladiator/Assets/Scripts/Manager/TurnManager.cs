using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public Player player;
    public Enemy enemy;

    private Queue<UnitBase> turnQueue = new Queue<UnitBase>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetTurnOrder();
        StartFirstTurn();
    }

    void SetTurnOrder()
    {
        if (player.statData.speed >= enemy.statData.speed)
        {
            turnQueue.Enqueue(player);
            turnQueue.Enqueue(enemy);
        }
        else
        {
            turnQueue.Enqueue(enemy);
            turnQueue.Enqueue(player);
        }
    }

    void StartFirstTurn()
    {
        UnitBase firstUnit = turnQueue.Peek();
        firstUnit.StartTurn();
    }

    public void NextTurn()
    {
        UnitBase finishedUnit = turnQueue.Dequeue();
        turnQueue.Enqueue(finishedUnit);

        UnitBase nextUnit = turnQueue.Peek();
        nextUnit.StartTurn();
    }
}
