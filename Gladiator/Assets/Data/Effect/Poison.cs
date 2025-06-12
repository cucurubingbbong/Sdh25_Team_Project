using UnityEngine;

public class Poison : StatusEffect
{
    private int poisonDamage;

    public Poison(int turns, int dmg) : base("중독", turns)
    {
        poisonDamage = dmg;
    }

    public override void OnTurnStart(UnitBase unit)
    {
        unit.TakeDamage(poisonDamage);
        Debug.Log(unit.unitName + " 중독으로 " + poisonDamage + " 데미지!");
    }
}
