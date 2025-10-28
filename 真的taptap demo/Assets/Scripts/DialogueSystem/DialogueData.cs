
using OfficeOpenXml.Table.PivotTable;

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
}


[System.Serializable]
public class Quest
{
    public enum QuestType
    {
        Talk
    }

    public enum QuestState {
        Waitting,
        Appcept,
        Completed
    } 

    public int QuestId;
    public QuestType questType;
    public QuestState questState;
    public string questText;
}
