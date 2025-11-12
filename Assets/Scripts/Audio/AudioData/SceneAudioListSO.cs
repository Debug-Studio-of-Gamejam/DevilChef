using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneAudioListSO",menuName = "Audio/Scene Audio List")]
public class SceneAudioListSO : ScriptableObject
{
    public List<SceneAudioInfo> sceneAudioList = new List<SceneAudioInfo>();

    public SceneAudioInfo GetSceneAudioInfo(string sceneName)
    {
        return sceneAudioList.Find(s => s.sceneName == sceneName);
    }
}

[System.Serializable]
public class SceneAudioInfo
{
    [SceneName] public string sceneName;
    public AudioName backgroundMusic;
    public AudioName ambientMusic;
}
