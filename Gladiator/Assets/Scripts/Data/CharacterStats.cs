using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    // �ִ� ü��
    [SerializeField] float m_maxHp;
    // ���ݷ�
    [SerializeField] int m_attackPower;
    // ����
    [SerializeField] int m_DefensePower;
    // ũ��Ƽ�� Ȯ��
    [SerializeField] float m_criticalChance;
    // ũ��Ƽ�� ������
    [SerializeField] float m_ciriticalDamage;
    // ȸ�� Ȯ��
    [SerializeField] float m_dodgeChance;
    // ���ǵ�
    [SerializeField] int m_speed;

    public float maxHp { get { return m_maxHp; } }
    public int attackPower { get { return m_attackPower; } }
    public int defensePower { get { return m_DefensePower;} }
    public float criticalChance { get { return m_criticalChance; } }
    public float criticalDamage { get { return m_ciriticalDamage; } }
    public float dodgeChance { get { return m_dodgeChance;} }
    public int speed { get { return m_speed; } }




}
