using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTest : MonoBehaviour
{
    
    public TMP_InputField textField;
    public TextMeshProUGUI dataInfo;

    void Start()
    {
        
        dataInfo.text = $"对话表有{DataLoader.Instance.dialogues.Count}条数据， 选项表有{DataLoader.Instance.options.Count}条数据";
    }

    public void TestDialogue()
    {
        string input = textField.text;

        if (int.TryParse(input, out int dialogueId))
        {
            Debug.Log($"Testing dialogue {dialogueId}");
            DialogueSystem.Instance.ShowDialogue(dialogueId);
        }
        else
        {
            Debug.LogWarning("请输入正确的数字！");
        }
    }
}
