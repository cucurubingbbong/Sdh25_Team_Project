using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    [Header("이벤트 데이터")]
    public List<EventData> allEvents;

    [Header("UI 요소")]
    public GameObject eventPanel;
    public TMP_Text eventNameText;
    public TMP_Text eventDescText;
    public TMP_Text dialogueText;
    public Button nextButton;

    public GameObject choiceGroup;
    public Button[] choiceButtons; 

    private Queue<string> dialogueQueue;
    private EventData currentEvent;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TryRunEvent(int currentDay)
    {
        List<EventData> validEvents = new List<EventData>();

        foreach (var e in allEvents)
        {
            if (e.minDayRequired <= currentDay)
            {
                validEvents.Add(e);
            }
        }

        if (validEvents.Count == 0)
        {
            Debug.Log("오늘은 이벤트가 없습니다.");
            return;
        }

        EventData selected = validEvents[Random.Range(0, validEvents.Count)];
        RunEvent(selected);
    }

    public void RunEvent(EventData data)
    {
        currentEvent = data;
        eventPanel.SetActive(true);
        eventNameText.text = data.eventName;
        eventDescText.text = data.description;

        dialogueQueue = new Queue<string>(data.dialogueLines);
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextDialogue);

        choiceGroup.SetActive(false);
        NextDialogue();
    }

    void NextDialogue()
    {
        if (dialogueQueue.Count > 0)
        {
            dialogueText.text = dialogueQueue.Dequeue();
        }
        else
        {
            nextButton.gameObject.SetActive(false);

            if (currentEvent.hasChoices && currentEvent.choices.Count > 0)
            {
                ShowChoices();
            }
            else
            {
                dialogueText.text = "이벤트가 종료되었습니다.";
                StartCoroutine(CloseAfterDelay(2f));
            }
        }
    }

    void ShowChoices()
    {
        choiceGroup.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentEvent.choices.Count)
            {
                var choiceData = currentEvent.choices[i];
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = choiceData.choiceText;

                int index = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() =>
                {
                    ApplyChoice(index);
                });
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void ApplyChoice(int index)
    {
        var choice = currentEvent.choices[index];

        dialogueText.text = choice.resultText;
        choiceGroup.SetActive(false);

        GoldManager.Instance.AddGold(choice.goldChange);
        GManager.instance.reputation += choice.reputationChange;
        eventPanel.SetActive(false);
        GManager.instance.EndDay();
    }

    IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        eventPanel.SetActive(false);
        nextButton.gameObject.SetActive(true);
    }
}
