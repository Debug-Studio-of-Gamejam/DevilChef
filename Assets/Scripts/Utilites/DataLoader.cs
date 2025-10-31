using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class DataLoader: Singleton<DataLoader>
{
    public Dictionary<int, Dialogue> dialogues = new Dictionary<int, Dialogue>();
    public Dictionary<int, Option> options = new Dictionary<int, Option>(); 
    public Dictionary<CharacterName, CharacterEvent> characterEvents = new Dictionary<CharacterName, CharacterEvent>();

    void Awake()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("database");
        if (jsonText == null)
        {
            Debug.LogError("Cannot find database.json in Resources!");
            return;
        }

        var wrapper = JsonConvert.DeserializeObject<DataWrapper>(jsonText.text);
        dialogues = wrapper.dialogues;
        options = wrapper.options;
        characterEvents = wrapper.characterEvents;
    }
}