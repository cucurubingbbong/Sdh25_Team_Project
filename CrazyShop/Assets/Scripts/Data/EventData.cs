using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Game/EventData")]
public class EventData : ScriptableObject
{
    public string eventName;
    [TextArea(2, 5)] public string description;

    [TextArea(2, 5)] public List<string> dialogueLines;

    public int minDayRequired;

    public bool hasChoices;

    [System.Serializable]
    public class EventChoice
    {
        public string choiceText;
        [TextArea(2, 5)] public string resultText;
        public int goldChange;
        public int reputationChange;
    }

    public List<EventChoice> choices;
}
