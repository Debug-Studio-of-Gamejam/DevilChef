using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questable : MonoBehaviour
{
    public Quest quest;

    public void DelegateQuest()
    {
        
        if (quest.questState == Quest.QuestState.Waitting)
        {
            //接取一个任务
        }
        else
        {
            
        }
    }
}
