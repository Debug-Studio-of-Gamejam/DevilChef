// UIManager.cs
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // ��Unity�༭�������ЩUI�����ק��ֵ��
    public GameObject startMenuPanel;
    public GameObject settingPanel;
    public GameObject inventoryPanel; // �������
    // ... ����ȫ��UI���

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // UIManagerͨ��Ҳ�ڿ�ʼ���������������Ը�����Ҫ�����Ƿ�DontDestroyOnLoad
            // ���ÿ��������UI����ͬ���ǾͲ���ҪDontDestroyOnLoad��������ÿ�������������á�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ��ʾ/���ؿ�ʼ�˵�
    public void ToggleStartMenu(bool show)
    {
        startMenuPanel.SetActive(show);
    }

    // ��ʾ/�����������
    public void ToggleSettingPanel(bool show)
    {
        settingPanel.SetActive(show);
    }

    // ��ʾ/���ر���
    public void ToggleInventory(bool show)
    {
        inventoryPanel.SetActive(show);
        // ��������ʱ��������ͣ��Ϸ
        GameManager.Instance.isPaused = show;
        Time.timeScale = show ? 0 : 1; // 0������ͣ��1���������ٶ�
    }
}