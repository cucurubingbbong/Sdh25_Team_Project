using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Game/Customer")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public CustomerType customerType;
    public Sprite portrait;
    public List<ItemData> preferredItems;
}

public enum CustomerType
{
    Regular,      // �Ϲ� �մ�, �����ϰ� ������ ����
    BargainHunter, // ���� ���ϴ� �մ�, ���� ������ ��
    Collector,    // Ư�� ��� �����۸� ã�� �մ�
    Impulsive,    // �浿������, �̻��� �����۵� �� �簨
    Thief,        // ��ġ���� �õ��ϴ� �մ�
    Mysterious,   // ��ü �Ҹ�, Ư���� �̺�Ʈ�� ����ų �� ����
}
