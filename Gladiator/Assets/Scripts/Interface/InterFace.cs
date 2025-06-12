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

