using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TransitionManager : Singleton<TransitionManager>
{
    [SceneName] public string startSceneName;
    public CanvasGroup canvasGroup;
    public float fadeDuration;
    bool isFading = false;
    
    public string CurrentSceneName { get; private set; }

    private void Start()
    {
        // 延迟一帧开始场景切换，确保Persistent场景完全初始化
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        // 等待一帧，确保所有Awake和Start方法执行完成
        yield return null;
        
        // 确保主摄像机存在
        if (Camera.main == null)
        {
            Debug.LogError("启动时主摄像机为null，检查Persistent场景中的摄像机设置");
        }
        else
        {
            Debug.Log("启动时找到主摄像机: " + Camera.main.name);
        }
        
        Transition(string.Empty, startSceneName);
    }

    public void Transition(string fromSceneName, string toSceneName)
    {
        if (!isFading)
            StartCoroutine(TransitionToScene(fromSceneName, toSceneName));
    }

    private IEnumerator TransitionToScene(string fromSceneName, string toSceneName)
    {
        Debug.Log($"开始场景切换: {fromSceneName} -> {toSceneName}");
        
        yield return Fade(1);
        
        if (fromSceneName != string.Empty)
        {
            EventHandler.CallBeforeSceneUnloadEvent(fromSceneName); 
            yield return SceneManager.UnloadSceneAsync(fromSceneName);
        }
        
        yield return SceneManager.LoadSceneAsync(toSceneName, LoadSceneMode.Additive);

        CurrentSceneName = toSceneName;
        Scene newScene = SceneManager.GetSceneByName(toSceneName);
        
        // 确保新场景被激活
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
            Debug.Log($"已激活场景: {toSceneName}");
        }
        else
        {
            Debug.LogError($"场景 {toSceneName} 无效，无法激活");
        }
        
        // 确保主摄像机存在并禁用其他场景中的摄像机
        if (Camera.main == null)
        {
            Debug.LogWarning("主摄像机为null，尝试查找摄像机");
            Camera[] cameras = FindObjectsOfType<Camera>();
            Camera persistentCamera = null;
            
            foreach (Camera cam in cameras)
            {
                // 优先使用Persistent场景中的摄像机
                if (cam.gameObject.scene.name == "Persistent" && cam.CompareTag("MainCamera"))
                {
                    persistentCamera = cam;
                    Debug.Log("找到Persistent场景中的主摄像机: " + cam.name);
                    break;
                }
            }
            
            // 如果没有找到Persistent场景的摄像机，使用其他场景的
            if (persistentCamera == null)
            {
                foreach (Camera cam in cameras)
                {
                    if (cam.CompareTag("MainCamera"))
                    {
                        Debug.Log("找到标记为MainCamera的摄像机: " + cam.name);
                        break;
                    }
                }
            }
        }
        else
        {
            // 确保只有一个主摄像机启用
            Camera[] cameras = FindObjectsOfType<Camera>();
            foreach (Camera cam in cameras)
            {
                if (cam.CompareTag("MainCamera") && cam != Camera.main)
                {
                    Debug.Log("禁用多余的主摄像机: " + cam.name);
                    cam.enabled = false;
                }
            }
        }
        
        EventHandler.CallAfterSceneLoadEvent(toSceneName);
        yield return Fade(0);
        
        Debug.Log($"场景切换完成: {toSceneName}");
    }

    private IEnumerator Fade(float targetAlpha)
    {
        isFading = true;
        canvasGroup.blocksRaycasts = true;
        
        float speed = Mathf.Abs(canvasGroup.alpha - targetAlpha) / fadeDuration;
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        canvasGroup.blocksRaycasts = false;
        isFading = false;
        
    }

    /// <summary>
    /// 返回开始场景
    /// </summary>
    public void ReturnToStartScene()
    {
        if (!isFading)
        {
            Transition(CurrentSceneName, startSceneName);
        }
    }
}
