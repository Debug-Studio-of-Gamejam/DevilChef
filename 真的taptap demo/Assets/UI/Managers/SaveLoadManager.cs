// SaveLoadManager.cs
using UnityEngine;
using System.IO; // �����ļ�����

[System.Serializable] // �����ǩ���������Ա����л���ת����JSON��
public class GameData
{
    // ������ı���Ҫ��GameManager����Ҫ�����һһ��Ӧ
    public int savedDay;
    public int savedStamina;
    public int savedSanity;
    public int savedMoney;
    public string savedSceneName;
    // �����Դ汳�����ݵȵ�...
}

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private string saveFilePath; // �浵�ļ�·��

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Application.persistentDataPath + "/savedata.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        // 1. ����һ���浵���ݶ��󣬲���GameManager�л�ȡ��ǰ����
        GameData data = new GameData();
        data.savedDay = GameManager.Instance.currentDay;
        data.savedStamina = GameManager.Instance.playerStamina;
        // ... �����������

        // 2. �����ݶ���ת��ΪJSON�ַ���
        string jsonData = JsonUtility.ToJson(data);

        // 3. ��JSON�ַ���д���ļ�
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("��Ϸ�ѱ��棡");
    }

    public void LoadGame()
    {
        // 1. ���浵�ļ��Ƿ����
        if (File.Exists(saveFilePath))
        {
            // 2. ���ļ���ȡJSON�ַ���
            string jsonData = File.ReadAllText(saveFilePath);

            // 3. ��JSON�ַ���ת�������ݶ���
            GameData data = JsonUtility.FromJson<GameData>(jsonData);

            // 4. ������д��GameManager
            GameManager.Instance.currentDay = data.savedDay;
            GameManager.Instance.playerStamina = data.savedStamina;
            // ... ������������

            Debug.Log("��Ϸ�Ѽ��أ�");
        }
        else
        {
            Debug.LogWarning("û���ҵ��浵�ļ���");
        }
    }

    // ��������Ϸ�˳�ʱ�Զ����ñ��棨ʵ���Զ��浵��
    void OnApplicationQuit()
    {
        SaveGame();
    }
}