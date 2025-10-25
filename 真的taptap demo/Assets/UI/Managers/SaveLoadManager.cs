// SaveLoadManager.cs
using UnityEngine;
using System.IO; // 用于文件操作

[System.Serializable] // 这个标签让这个类可以被序列化（转换成JSON）
public class GameData
{
    // 这里面的变量要和GameManager里需要保存的一一对应
    public int savedDay;
    public int savedStamina;
    public int savedSanity;
    public int savedMoney;
    public string savedSceneName;
    // 还可以存背包数据等等...
}

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private string saveFilePath; // 存档文件路径

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
        // 1. 创建一个存档数据对象，并从GameManager中获取当前数据
        GameData data = new GameData();
        data.savedDay = GameManager.Instance.currentDay;
        data.savedStamina = GameManager.Instance.playerStamina;
        // ... 填充其他数据

        // 2. 将数据对象转换为JSON字符串
        string jsonData = JsonUtility.ToJson(data);

        // 3. 将JSON字符串写入文件
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("游戏已保存！");
    }

    public void LoadGame()
    {
        // 1. 检查存档文件是否存在
        if (File.Exists(saveFilePath))
        {
            // 2. 从文件读取JSON字符串
            string jsonData = File.ReadAllText(saveFilePath);

            // 3. 将JSON字符串转换回数据对象
            GameData data = JsonUtility.FromJson<GameData>(jsonData);

            // 4. 将数据写回GameManager
            GameManager.Instance.currentDay = data.savedDay;
            GameManager.Instance.playerStamina = data.savedStamina;
            // ... 加载其他数据

            Debug.Log("游戏已加载！");
        }
        else
        {
            Debug.LogWarning("没有找到存档文件。");
        }
    }

    // 可以在游戏退出时自动调用保存（实现自动存档）
    void OnApplicationQuit()
    {
        SaveGame();
    }
}