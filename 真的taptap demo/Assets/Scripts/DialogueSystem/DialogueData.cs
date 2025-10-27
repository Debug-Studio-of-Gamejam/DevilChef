
using OfficeOpenXml.Table.PivotTable;

public class Dialogue
{
    public int dialogueId;
    public string dialogueText;
    public string Description;
}

public class Option
{
    public int OptionsId;
    public string OptionText;
    public int nextDialogueId;
}

public class Quest
{
    public enum QuestType
    {
        Gathering,
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
