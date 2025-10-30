using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;


public class DataLoader: Singleton<DataLoader>
{
    public Dictionary<int, Dialogue> dialogues = new Dictionary<int, Dialogue>();
    public Dictionary<int, Option> options = new Dictionary<int, Option>(); 
    public Dictionary<CharacterName, CharacterEvent> characterEvents = new Dictionary<CharacterName, CharacterEvent>();

    void Awake()
    {
        // 延迟加载，避免资源系统初始化问题
        StartCoroutine(LoadDataAsync());
    }

    System.Collections.IEnumerator LoadDataAsync()
    {
        // 等待一帧，确保资源系统完全初始化
        yield return null;
        
        TextAsset jsonText = Resources.Load<TextAsset>("database");
        if (jsonText == null)
        {
            Debug.LogError("Cannot find database.json in Resources!");
            yield break;
        }

        try
        {
            var wrapper = JsonConvert.DeserializeObject<DataWrapper>(jsonText.text);
            dialogues = wrapper.dialogues;
            options = wrapper.options;
            characterEvents = wrapper.characterEvents;
            Debug.Log("Data loaded successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse database.json: {e.Message}");
        }
    }
}