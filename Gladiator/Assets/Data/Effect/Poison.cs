using UnityEngine;

public class Poison : StatusEffect
{
    private int poisonDamage;

    public Poison(int turns, int dmg) : base("�ߵ�", turns)
    {
        poisonDamage = dmg;
    }

    public override void OnTurnStart(UnitBase unit)
    {
        unit.TakeDamage(poisonDamage);
        Debug.Log(unit.unitName + " �ߵ����� " + poisonDamage + " ������!");
    }
}
