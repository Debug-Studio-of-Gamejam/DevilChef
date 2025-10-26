using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StringGameObjectPair
{
    public string key;
    public GameObject value;
}
public class DialogueSystem : MonoBehaviour
{
    
    public GameObject dialogue;
    public GameObject narrator;
    public GameObject option;
    
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI narratorText;
    
    [SerializeField]
    private List<StringGameObjectPair> characterList = new List<StringGameObjectPair>();
    private Dictionary<string, GameObject> characterDict;
    
    [Header("测试用的文本")]
    public TextAsset textAsset;

    public int index;
    public float textSpeed = 0.1f;
    public bool textFinished;
    List<string> textList = new List<string>();
    
    void Awake()
    {
        BuildDictionary();
        ReadText(textAsset);
    }
    void Start()
    {
        HideAllCharacters();
        StartCoroutine(UpdateText());
    }
    
    void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)) && textFinished)
        {
            if (index == textList.Count)
            {
                dialogue.SetActive(false);
                narrator.SetActive(false);
                return;
            }

            if (textFinished)
            {
                StartCoroutine(UpdateText());
            }
            
        }
    }

    IEnumerator UpdateText()
    {
        var textLable = dialogueText;
        var currentText = textList[index];

        if (characterDict != null && characterDict.ContainsKey(currentText))
        {
            Debug.Log(currentText);
            HideAllCharacters();
            if (currentText == "Narrator")
            {
                textLable = narratorText;
                dialogue.SetActive(false);
                narrator.SetActive(true);
                
            }
            else if (currentText == "Option")
            {
                //TODO
            }
            else
            {
                dialogue.SetActive(true);
                narrator.SetActive(false);
                characterDict[currentText].SetActive(true);
            }
            index++;
        }
        
        textLable.text = "";
        textFinished = false;
        for (int i = 0; i < textList[index].Length; i++)
        {
            textLable.text += textList[index][i];
            yield return new WaitForSeconds(textSpeed);
        }

        textFinished = true;
        index++;
    }

    void ReadText(TextAsset textAsset)
    {
        textList.Clear();
        index = 0;

        var lineData = textAsset.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    private void HideAllCharacters()
    {
        foreach (var go in characterDict.Values)
        {
            go.SetActive(false);
        }
    }

    /// <summary>
    /// 把objectList转换成Dictionary
    /// </summary>
    private void BuildDictionary()
    {
        characterDict = new Dictionary<string, GameObject>();
        foreach (var pair in characterList)
        {
            if (string.IsNullOrEmpty(pair.key))
            {
                Debug.LogWarning($"空的key被忽略。");
                continue;
            }

            if (characterDict.ContainsKey(pair.key))
            {
                Debug.LogWarning($"重复的key：{pair.key}");
                continue;
            }

            characterDict.Add(pair.key, pair.value);
        }
    }

    
#if UNITY_EDITOR
    // ⚡ 编辑器下，当Inspector内容变化时自动重建字典，方便调试
    private void OnValidate()
    {
        BuildDictionary();
    }
#endif
}
