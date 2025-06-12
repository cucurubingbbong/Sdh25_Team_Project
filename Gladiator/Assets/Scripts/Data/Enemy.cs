using UnityEngine;

public class Enemy : MonoBehaviour, IDamageDealer, IDamageReceiver
{
    public string unitName;
    public CharacterStats baseStats;
    public float currentHp;

    private void Start()
    {
        currentHp = baseStats.maxHp;
    }

    public int CalculateDamage(IDamageReceiver target)
    {
        float damage = baseStats.attackPower;

        // 크리티컬 계산
        if (Random.value < baseStats.criticalChance)
        {
            damage *= baseStats.criticalDamage;
            Debug.Log($"{unitName}이(가) 크리티컬 공격!");
        }

        damage *= Random.Range(0.9f, 1.1f);

        // 방어력 적용
        int finalDamage = Mathf.RoundToInt(damage) - target.GetDefense();
        return Mathf.Max(finalDamage, 0);
    }

  
    public void TakeDamage(int damage)
    {
        Debug.Log($"{unitName}이(가) {damage} 데미지를 입었습니다!");
        currentHp -= damage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public int GetDefense()
    {
        return baseStats.defensePower;
    }

    private void Die()
    {
        Debug.Log($"{unitName}이(가) 쓰러졌습니다.");
    }
}
