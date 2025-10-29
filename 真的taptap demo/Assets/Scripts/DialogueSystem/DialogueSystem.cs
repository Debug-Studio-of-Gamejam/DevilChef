using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueType
{
    Normal,     // 普通角色对话（显示立绘+对话框）
    Narrator,   // 旁白（全屏文字）
    Null,       // 无立绘、仅对话框
    Options     // 选项
}

[System.Serializable]
public class DialogueLine
{
    public DialogueType type;
    public string speakerName;
    public string text;
    public List<int> optionIds; // 对应 Options 里的数字
}

[Serializable]
public class SpeakerInfo
{
    public CharacterName name;
    public Sprite avatarBack;
    public Sprite avatarFront;
}
public class DialogueSystem : Singleton<DialogueSystem>
{
    public GameObject dialogue;
    public GameObject narrator;
    public List<GameObject> optionGroup;
    public List<Sprite> playerAvatars;
    //美术图大小不同 NPC特殊处理，显示在 npcAvatar {"三叶虫", "因诺", "大饼", "小葵", "摸摸", "牙牙乐", "蓝七"};
    public List<CharacterName> npcNames = new List<CharacterName>();
        
    
    [Header("UI Elements")]
    // public Image playerAvatar;
    public Image characterAvatarBack;
    public Image characterAvatarFront;
    public Image npcAvatar;
    public TextMeshProUGUI characterNameLable;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI narratorText;
    
    [Header("立绘数据")]
    [SerializeField]
    private List<SpeakerInfo> characterList = new List<SpeakerInfo>();
    private Dictionary<CharacterName, SpeakerInfo> speakerDict;
    
    public float textSpeed = 0.1f;
    
    private int currentDialogueId;
    List<DialogueLine> textList = new List<DialogueLine>();
    private TextMeshProUGUI targetTextLable;
    private int index;
    private bool typingFinished;
    private Coroutine typingCoroutine;
    
    void Awake()
    {
        speakerDict = characterList.ToDictionary(s => s.name, s => s);
        HideAllDialoguePanel();
    }
    
    void Update()
    {
        if (GameManager.Instance.isTalking && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (index == textList.Count)
            {
                HideAllDialoguePanel();
                EventHandler.CallIDialogueFinishedEvent(currentDialogueId);
                return;
            }
            
            if (typingFinished)
            {
                ShowDialogueLine();
            }
            else
            {
                // 停止协程并直接显示完整文字
                StopCoroutine(typingCoroutine);
                targetTextLable.text = textList[index].text;
                typingFinished = true;
                index++;
            }
        }
    }

    /// <summary>
    /// 显示对话界面的入口
    /// </summary>
    /// <param name="dialogueId"></param>
    public void ShowDialogue(int dialogueId)
    {
        
        GameManager.Instance.isTalking = true;
        currentDialogueId = dialogueId;
        GameManager.Instance.triggeredDialogues.Add(dialogueId);
        EventHandler.CallIDialogueStartEvent(dialogueId);
        Dialogue dialogueData = DataLoader.Instance.dialogues[dialogueId];

        if (dialogueData != null)
        {
            textList.Clear();
            index = 0;

            var lineData = dialogueData.dialogueText.Split('\n');
            textList = ParseDialogue(lineData);
            //Debug.Log($"对话 {dialogueData.dialogueId} 有 {textList.Count} 句");
            ShowDialogueLine();
        }
        else
        {
            Debug.LogWarning($"找不到 dialogueId = {dialogueId} 的对话。");
        }
    }

    List<DialogueLine> ParseDialogue(string[] lines)
    {
        List<DialogueLine> result = new List<DialogueLine>();

        for (int i = 0; i < lines.Length; i += 2)
        {
            string speaker = lines[i].Trim();
            string text = (i + 1 < lines.Length) ? lines[i + 1].Trim() : "";

            DialogueLine line = new DialogueLine();

            if (speaker == "Options")
            {
                line.type = DialogueType.Options;
                line.optionIds = text
                    .Split('|')
                    .Select(s => int.Parse(s.Trim()))
                    .ToList();
            }
            else if (speaker == "Narrator")
            {
                line.type = DialogueType.Narrator;
                line.text = text;
            }
            else if (speaker == "Null")
            {
                line.type = DialogueType.Null;
                line.text = text;
            }
            else
            {
                line.type = DialogueType.Normal;
                line.speakerName = speaker;
                line.text = text;
            }

            result.Add(line);
        }

        return result;
    }
    
    void ShowDialogueLine()
    {
        var line = textList[index];
        switch (line.type)
        {
            case DialogueType.Normal:
                // 根据 speakerName 从字典里取 SpeakerInfo
                HideAvatars();
                characterNameLable.gameObject.SetActive(true);
                if (line.speakerName.StartsWith("主角"))
                {
                    characterNameLable.text = "主角";//GameManager.Instance.playerName;
                    string numberPart = Regex.Replace(line.speakerName, @"[^\d]", "");
                    if (int.TryParse(numberPart, out int index))
                    {
                        // 安全地从列表中取立绘
                        if (index >= 0 && index < playerAvatars.Count)
                        {
                            characterAvatarFront.gameObject.SetActive(true);
                            characterAvatarFront.sprite = playerAvatars[index];
                        }
                        else
                        {
                            Debug.LogWarning($"主角编号 {index} 超出 playerAvatars 范围！");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"无法从 {line.speakerName} 提取数字！");
                    }
                }
                else
                {
                    // 把字符串转换成 CharacterName枚举
                    if (Enum.TryParse(line.speakerName, out CharacterName speakerName))
                    {
                        
                        SpeakerInfo speaker = speakerDict[speakerName];
                        characterNameLable.text = line.speakerName;
                        if (speakerDict.ContainsKey(speakerName))
                        {
                            // NPC 的图片位置特殊处理
                            if (npcNames.Contains(speaker.name))
                            {
                                npcAvatar.gameObject.SetActive(true);
                                npcAvatar.sprite = speaker.avatarFront;
                                npcAvatar.SetNativeSize();
                            }
                            else
                            {
                                if (speaker.avatarBack)
                                {
                                    characterAvatarBack.gameObject.SetActive(true);
                                    characterAvatarBack.sprite = speaker.avatarBack;
                                }

                                if (speaker.avatarFront)
                                {
                                    characterAvatarFront.gameObject.SetActive(true);
                                    characterAvatarFront.sprite = speaker.avatarFront;
                                }
                            }
                            
                        }
                        else
                        {
                            Debug.LogWarning($"立绘数据字典中没有找到 {speakerName}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"无法把字符串 {line.speakerName} 转换成有效的 CharacterName");
                    }
                }
                dialogue.SetActive(true);
                narrator.SetActive(false);
                targetTextLable = dialogueText;
                typingCoroutine = StartCoroutine(UpdateText());
                break;

            case DialogueType.Narrator:
                dialogue.SetActive(false);
                narrator.SetActive(true);
                targetTextLable = narratorText;
                typingCoroutine = StartCoroutine(UpdateText());
                break;

            case DialogueType.Null:
                HideAvatars();
                dialogue.SetActive(true);
                narrator.SetActive(false);
                characterNameLable.gameObject.SetActive(false);
                targetTextLable = dialogueText;
                typingCoroutine = StartCoroutine(UpdateText());
                // ShowDialogueBox(null, line.text); // 无角色名
                break;

            case DialogueType.Options:
                // 找出选项数据
                List<Option> currentOptions = new List<Option>();
                foreach (var id in line.optionIds)
                {
                    Option opt = DataLoader.Instance.options[id];
                    if (opt != null)
                        currentOptions.Add(opt);
                }
                ShowOptions(currentOptions);
                break;
        }
    }
    
    void ShowOptions(List<Option> options)
    {
        HideOptions();
        GameManager.Instance.isTalking = false;
        for (int i = 0; i < options.Count && i < optionGroup.Count; i++)
        {
            var optionObj = optionGroup[i];
            optionObj.SetActive(true);  // 显示这个按钮

            // 设置按钮文本
            var text = optionObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text)
                text.text = options[i].optionText;

            // 绑定点击事件（先清空旧的）
            var button = optionObj.GetComponent<Button>();
            if (button)
            {
                button.onClick.RemoveAllListeners();
                int index = i; // 避免闭包问题
                button.onClick.AddListener(() => OnOptionSelected(options[index]));
            }
        }
    }

    void OnOptionSelected(Option option)
    {
        HideOptions();
        if (option.nextDialogueId == 0)
        {
            HideAllDialoguePanel();
            EventHandler.CallIDialogueFinishedEvent(currentDialogueId);
        }
        else
        {
            ShowDialogue(option.nextDialogueId);
        }

    }
    
    public void AddItemByName(string itemNameStr)
    {
        if (Enum.TryParse<ItemName>(itemNameStr, out var item))
        {
            // items.Add(item);
            Debug.Log($"获得物品：{item}");
        }
        else
        {
            Debug.LogWarning($"未找到物品名：{itemNameStr}");
        }
    }

    private void HideAvatars()
    {
        npcAvatar.gameObject.SetActive(false);
        characterAvatarBack.gameObject.SetActive(false);
        characterAvatarFront.gameObject.SetActive(false);
    }

    private void HideOptions()
    {
        foreach (var go in optionGroup)
        {
            go.SetActive(false);
        }
    }

    private void HideAllDialoguePanel()
    {
        GameManager.Instance.isTalking = false;
        dialogue.SetActive(false);
        narrator.SetActive(false);
        HideOptions();
        HideAvatars();
    }


    IEnumerator UpdateText()
    {
        targetTextLable.text = "";
        typingFinished = false;
        foreach (var c in textList[index].text)
        {
            targetTextLable.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        typingFinished = true;
        index++;
    }
}
