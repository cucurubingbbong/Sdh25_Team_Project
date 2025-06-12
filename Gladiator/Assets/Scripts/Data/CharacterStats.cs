using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    // 최대 체력
    [SerializeField] float m_maxHp;
    // 공격력
    [SerializeField] int m_attackPower;
    // 방어력
    [SerializeField] int m_DefensePower;
    // 크리티컬 확률
    [SerializeField] float m_criticalChance;
    // 크리티컬 데미지
    [SerializeField] float m_ciriticalDamage;
    // 회피 확률
    [SerializeField] float m_dodgeChance;
    // 스피드
    [SerializeField] int m_speed;

    public float maxHp { get { return m_maxHp; } }
    public int attackPower { get { return m_attackPower; } }
    public int defensePower { get { return m_DefensePower;} }
    public float criticalChance { get { return m_criticalChance; } }
    public float criticalDamage { get { return m_ciriticalDamage; } }
    public float dodgeChance { get { return m_dodgeChance;} }
    public int speed { get { return m_speed; } }




}
