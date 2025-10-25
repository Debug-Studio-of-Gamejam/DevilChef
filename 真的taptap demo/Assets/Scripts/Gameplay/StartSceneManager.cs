
using UnityEngine;
using UnityEngine.UI; // �����Ҫ����UI����Ļ�

public class StartSceneManager : MonoBehaviour
{
    // �����Ҫ���ó����е��ض�UIԪ�أ���������������
    [Header("UI References")]
    public Button startButton;
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton; 

    void Start()
    {
        // ��ʼ����ť�¼���������ڱ༭���а󶨣�
        InitializeButtons();

        // ȷ����Ϸʱ������������ͣ״̬�ָ���
        Time.timeScale = 1f;

        // ���ſ�ʼ�����ı�������
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
    }

    void InitializeButtons()
    {
        // ���ͨ���༭����ק���˰�ť�������������

    }

    public void OnStartGameClick()
    {
        Debug.Log("��ʼ��Ϸ��ť�������");

        // ���Ű�ť��Ч
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // ����GameManager��ʼ����Ϸ
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }
        else
        {
            Debug.LogError("GameManager.Instance Ϊ null��");
        }
    }

    public void OnContinueGameClick()
    {
        Debug.Log("������Ϸ��ť�������");

        // ���Ű�ť��Ч
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // ����GameManager������Ϸ
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
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
    }

    public void OnQuitGameClick()
    {
        Debug.Log("�˳���Ϸ��ť�������");

        // ���Ű�ť��Ч
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // �˳���Ϸ
#if UNITY_EDITOR
            // �ڱ༭����ֹͣ����
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ڴ�������Ϸ���˳�
        Application.Quit();
#endif
    }

    void Update()
    {
        // ������������Ӽ��̿�ݼ�֧��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnQuitGameClick();
        }
    }
}
