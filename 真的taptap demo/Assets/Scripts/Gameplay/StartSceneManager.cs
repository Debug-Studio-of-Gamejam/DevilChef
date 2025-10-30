<<<<<<< Updated upstream

using UnityEngine;
using UnityEngine.UI; // �����Ҫ����UI����Ļ�

public class StartSceneManager : MonoBehaviour
{
    // �����Ҫ���ó����е��ض�UIԪ�أ���������������
=======
using UnityEngine;
using UnityEngine.UI; // 如果需要操作UI组件的话

public class StartSceneManager : MonoBehaviour
{
    // 如果需要引用场景中的特定UI元素，可以在这里声明
>>>>>>> Stashed changes
    [Header("UI References")]
    public Button startButton;
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton; 

    void Start()
    {
<<<<<<< Updated upstream
        // ��ʼ����ť�¼���������ڱ༭���а󶨣�
        InitializeButtons();

        // ȷ����Ϸʱ������������ͣ״̬�ָ���
        Time.timeScale = 1f;

        // ���ſ�ʼ�����ı�������
=======
        // 初始化按钮事件（如果不在编辑器中绑定）
        InitializeButtons();

        // 确保游戏时间正常（从暂停状态恢复）
        Time.timeScale = 1f;

        // 播放开始场景的背景音乐
>>>>>>> Stashed changes
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
    }

    void InitializeButtons()
    {
<<<<<<< Updated upstream
        // ���ͨ���༭����ק���˰�ť�������������
=======
        // 如果通过编辑器拖拽绑定了按钮，这里可以留空
>>>>>>> Stashed changes

    }

    public void OnStartGameClick()
    {
<<<<<<< Updated upstream
        Debug.Log("��ʼ��Ϸ��ť�������");

        // ���Ű�ť��Ч
=======
        Debug.Log("开始游戏按钮被点击！");

        // 播放按钮音效
>>>>>>> Stashed changes
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

<<<<<<< Updated upstream
        // ����GameManager��ʼ����Ϸ
=======
        // 调用GameManager开始新游戏
>>>>>>> Stashed changes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }
        else
        {
<<<<<<< Updated upstream
            Debug.LogError("GameManager.Instance Ϊ null��");
=======
            Debug.LogError("GameManager.Instance 为 null！");
>>>>>>> Stashed changes
        }
    }

    public void OnContinueGameClick()
    {
<<<<<<< Updated upstream
        Debug.Log("������Ϸ��ť�������");

        // ���Ű�ť��Ч
=======
        Debug.Log("继续游戏按钮被点击！");

        // 播放按钮音效
>>>>>>> Stashed changes
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

<<<<<<< Updated upstream
        // ����GameManager������Ϸ
=======
        // 调用GameManager继续游戏
>>>>>>> Stashed changes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
<<<<<<< Updated upstream
            Debug.LogError("GameManager.Instance Ϊ null��");
        }
    }

    public void OnSettingsClick()
    {
        Debug.Log("���ð�ť�������");

        // ���Ű�ť��Ч
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // ���������
        if (UIManager.Instance != null)
        {
            UISettingsManager.Instance.OpenSettings();
        }
        else
        {
            Debug.LogError("UIManager.Instance Ϊ null��");
        }
=======
            Debug.LogError("GameManager.Instance 为 null！");
        }
    }

    // 注意：这个方法现在可以保留作为备用，但推荐使用LocalSettingsButton脚本
    public void OnSettingsClick()
    {
        Debug.Log("StartScene设置按钮被点击！");

        // 使用全局设置管理器打开设置场景
        GlobalSettingsManager.OpenSettingsScene();
>>>>>>> Stashed changes
    }

    public void OnQuitGameClick()
    {
<<<<<<< Updated upstream
        Debug.Log("�˳���Ϸ��ť�������");

        // ���Ű�ť��Ч
=======
        Debug.Log("退出游戏按钮被点击！");

        // 播放按钮音效
>>>>>>> Stashed changes
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

<<<<<<< Updated upstream
        // �˳���Ϸ
#if UNITY_EDITOR
            // �ڱ༭����ֹͣ����
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ڴ�������Ϸ���˳�
=======
        // 退出游戏
#if UNITY_EDITOR
            // 在编辑器中停止播放
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在打包后的游戏中退出
>>>>>>> Stashed changes
        Application.Quit();
#endif
    }

    void Update()
    {
<<<<<<< Updated upstream
        // ������������Ӽ��̿�ݼ�֧��
=======
        // 可以在这里添加键盘快捷键支持
>>>>>>> Stashed changes
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnQuitGameClick();
        }
    }
<<<<<<< Updated upstream
}
=======
}
>>>>>>> Stashed changes
