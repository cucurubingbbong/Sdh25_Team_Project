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
    void OnApply(IStatusEffectReceiver target);       // ���� ����
    void OnTurnStart(IStatusEffectReceiver target);   // �� ����
    void OnTurnEnd(IStatusEffectReceiver target);     // �� ���� 
    void OnExpire(IStatusEffectReceiver target);      // �����̻� ������
}

public interface IStatusEffectReceiver
{
    void AddStatusEffect(IStatusEffect effect);
    void RemoveStatusEffect(IStatusEffect effect);
    List<IStatusEffect> GetStatusEffects();
}





