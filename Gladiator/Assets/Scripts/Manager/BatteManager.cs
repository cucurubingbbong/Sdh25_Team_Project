using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public Player player;
    public Enemy enemy;

    public void Start()
    {
        StartBattle();
    }
    public void StartBattle()
    {
        int damage = player.CalculateDamage(enemy);
        enemy.TakeDamage(damage);
    }
}
