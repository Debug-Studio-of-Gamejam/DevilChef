
using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public int dialogueId;
    public string dialogueText;
    public string description;
}
[System.Serializable]
public class Option
{
    public int optionsId;
    public string optionText;
    public int nextDialogueId;
    public string getItemId;
}

[System.Serializable]
public class CharacterEvent
{
    public CharacterName characterName;
    public List<int> specialRounds;
    public int specialDialogueID;
    public int normalDialogueID;
    public int conditionRound;
    public int requiredDialogueID;
    public int successDialogueID;
    public ItemName requiredItem;
    public IngredientName rewardIngredient;
}

// DataWrapper，用于一次性序列化/反序列化
[System.Serializable]
public class DataWrapper
{
    public Dictionary<int, Dialogue> dialogues = new Dictionary<int, Dialogue>();
    public Dictionary<int, Option> options = new Dictionary<int, Option>();
    public Dictionary<CharacterName, CharacterEvent> characterEvents = new Dictionary<CharacterName, CharacterEvent>();
}


// [System.Serializable]
// public class Quest
// {
//     public enum QuestType
//     {
//         Talk
//     }
//
//     public enum QuestState {
//         Waitting,
//         Appcept,
//         Completed
//     } 
//
//     public int QuestId;
//     public QuestType questType;
//     public QuestState questState;
//     public string questText;
// }
