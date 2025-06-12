using UnityEngine;

public abstract class StatusEffect
{
    public string effectName;
    public int duration;

    public StatusEffect(string name, int turns)
    {
        effectName = name;
        duration = turns;
    }

    public virtual void OnTurnStart(UnitBase unit) { }

    public virtual void OnTurnEnd(UnitBase unit)
    {
        duration--;
    }
}
