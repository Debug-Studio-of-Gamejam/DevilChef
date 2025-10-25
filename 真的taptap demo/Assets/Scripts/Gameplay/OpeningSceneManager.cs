using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpeningSceneManager : MonoBehaviour
{
    [Header("开场图片序列")]
    public Image displayImage;          // 显示图片的UI组件
    public Sprite[] storyboardImages;   // 在编辑器中拖入图片

    [Header("跳过按钮")]
    public Button skipButton;

    [Header("设置")]
    public float imageDisplayTime = 3f; // 每张图片显示时间

    private int currentImageIndex = 0;

    void Start()
    {
        Debug.Log("OpeningSceneManager 启动");
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance 为 null");
            // 检查场景中是否有Managers对象
            GameObject managers = GameObject.Find("Managers");
            if (managers == null)
            {
                Debug.LogError("场景中找不到Managers对象");
            }
            else
            {
                Debug.Log("找到Managers对象: " + managers.name);
            }
        }
        else
        {
            Debug.Log("GameManager.Instance 正常");
        }

        // 绑定跳过按钮事件
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipToOpeningPlot);
        }

        // 开始播放开场动画
        StartCoroutine(PlayOpeningAnimation());

        // 播放开场音乐（如果有）
        if (AudioManager.Instance != null && AudioManager.Instance.mainMenuMusic != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
    }

    IEnumerator PlayOpeningAnimation()
    {
        Debug.Log("开始播放开场动画");
        // 显示第一张图片
        if (storyboardImages.Length > 0)
        {
            ShowImage(0);
        }

        // 循环显示所有图片
        for (int i = 0; i < storyboardImages.Length; i++)
        {
            ShowImage(i);
            yield return new WaitForSeconds(imageDisplayTime);
        }

        Debug.Log("开场动画播放完毕");
        // 所有图片播放完毕，进入教程场景
        LoadOpeningPlotScene();
    }

    void ShowImage(int index)
    {
        if (index < storyboardImages.Length && storyboardImages[index] != null)
        {
            displayImage.sprite = storyboardImages[index];
            currentImageIndex = index;
            Debug.Log($"显示第 {index + 1} 张图片，共 {storyboardImages.Length} 张");
        }
    }

    public void SkipToOpeningPlot()
    {
        Debug.Log("跳过开场动画");

        // 播放按钮音效
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // 停止所有协程
        StopAllCoroutines();

        // 进入教程场景
        LoadOpeningPlotScene();
    }

    void LoadOpeningPlotScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadScene("OpeningPlotScene");
        }
        else
        {
            Debug.LogError("GameManager.Instance 为 null！");  
            // 备用方案：直接加载场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("OpeningPlotScene");
        }
    }

    void Update()
    {
        // 键盘快捷键：按空格或ESC跳过
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            SkipToOpeningPlot();
        } 
    }
}