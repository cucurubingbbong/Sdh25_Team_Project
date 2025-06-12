using System.Collections.Generic;
using Unity.VisualScripting;

public interface ITurnActor
{
    void OnTurnStart();
    void TakeTurn();
    void OnTurnEnd();
}

public interface IDamageDealer
{
    int CalculateDamage(IDamageReceiver target);
}

public interface IDamageReceiver
{
    void TakeDamage(int damage);
    int GetDefense(); 
}

public interface IStatusEffect
{
    string Name { get; }
    int Duration { get; }
    void OnApply(IStatusEffectReceiver target);       // 상태 적용
    void OnTurnStart(IStatusEffectReceiver target);   // 턴 시작
    void OnTurnEnd(IStatusEffectReceiver target);     // 턴 종료 
    void OnExpire(IStatusEffectReceiver target);      // 상태이상 끝날떄
}

public interface IStatusEffectReceiver
{
    void AddStatusEffect(IStatusEffect effect);
    void RemoveStatusEffect(IStatusEffect effect);
    List<IStatusEffect> GetStatusEffects();
}





