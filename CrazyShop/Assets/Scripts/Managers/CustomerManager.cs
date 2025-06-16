using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public List<CustomerData> CustomerPool;
    public Image guestImageUI;
    public TextMeshProUGUI requestText;
    public GameObject guestPanel;
    public Button giveButton;
    public CustomerData currentGuest;
    public static CustomerManager Instance;

    public void StartGuestPhase()
    {
        guestPanel.SetActive(true);
        SpawnNewGuest();
    }

    void SpawnNewGuest()
    {
        currentGuest = CustomerPool[Random.Range(0, CustomerPool.Count)];
        guestImageUI.sprite = currentGuest.portrait;
        requestText.text = $"\"{currentGuest.preferredItems[1].itemName}\"을(를) 원해요!";
    }

    public void OnClickGive()
    {

    }
}
