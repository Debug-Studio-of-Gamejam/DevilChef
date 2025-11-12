using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioInfoListSO", menuName = "Audio/Audio Info List")]
public class AudioInfoListSO : ScriptableObject
{
    public List<AudioInf> audioInfos = new List<AudioInf>();

    public AudioInf GetAudioInfo(AudioName audioName)
    {
        return audioInfos.Find(x => x.audioName == audioName);
    }
}


[System.Serializable]
public class AudioInf
{
    public AudioName audioName;
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float volume;
    public bool loop;
}