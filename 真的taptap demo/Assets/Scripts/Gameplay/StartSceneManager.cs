<<<<<<< Updated upstream

using UnityEngine;
using UnityEngine.UI; // Èç¹ûÐèÒª²Ù×÷UI×é¼þµÄ»°

public class StartSceneManager : MonoBehaviour
{
    // Èç¹ûÐèÒªÒýÓÃ³¡¾°ÖÐµÄÌØ¶¨UIÔªËØ£¬¿ÉÒÔÔÚÕâÀïÉùÃ÷
=======
using UnityEngine;
using UnityEngine.UI; // å¦‚æžœéœ€è¦æ“ä½œUIç»„ä»¶çš„è¯

public class StartSceneManager : MonoBehaviour
{
    // å¦‚æžœéœ€è¦å¼•ç”¨åœºæ™¯ä¸­çš„ç‰¹å®šUIå…ƒç´ ï¼Œå¯ä»¥åœ¨è¿™é‡Œå£°æ˜Ž
>>>>>>> Stashed changes
    [Header("UI References")]
    public Button startButton;
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton; 

    void Start()
    {
<<<<<<< Updated upstream
        // ³õÊ¼»¯°´Å¥ÊÂ¼þ£¨Èç¹û²»ÔÚ±à¼­Æ÷ÖÐ°ó¶¨£©
        InitializeButtons();

        // È·±£ÓÎÏ·Ê±¼äÕý³££¨´ÓÔÝÍ£×´Ì¬»Ö¸´£©
        Time.timeScale = 1f;

        // ²¥·Å¿ªÊ¼³¡¾°µÄ±³¾°ÒôÀÖ
=======
        // åˆå§‹åŒ–æŒ‰é’®äº‹ä»¶ï¼ˆå¦‚æžœä¸åœ¨ç¼–è¾‘å™¨ä¸­ç»‘å®šï¼‰
        InitializeButtons();

        // ç¡®ä¿æ¸¸æˆæ—¶é—´æ­£å¸¸ï¼ˆä»Žæš‚åœçŠ¶æ€æ¢å¤ï¼‰
        Time.timeScale = 1f;

        // æ’­æ”¾å¼€å§‹åœºæ™¯çš„èƒŒæ™¯éŸ³ä¹
>>>>>>> Stashed changes
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
    }

    void InitializeButtons()
    {
<<<<<<< Updated upstream
        // Èç¹ûÍ¨¹ý±à¼­Æ÷ÍÏ×§°ó¶¨ÁË°´Å¥£¬ÕâÀï¿ÉÒÔÁô¿Õ
=======
        // å¦‚æžœé€šè¿‡ç¼–è¾‘å™¨æ‹–æ‹½ç»‘å®šäº†æŒ‰é’®ï¼Œè¿™é‡Œå¯ä»¥ç•™ç©º
>>>>>>> Stashed changes

    }

    public void OnStartGameClick()
    {
<<<<<<< Updated upstream
        Debug.Log("¿ªÊ¼ÓÎÏ·°´Å¥±»µã»÷£¡");

        // ²¥·Å°´Å¥ÒôÐ§
=======
        Debug.Log("å¼€å§‹æ¸¸æˆæŒ‰é’®è¢«ç‚¹å‡»ï¼");

        // æ’­æ”¾æŒ‰é’®éŸ³æ•ˆ
>>>>>>> Stashed changes
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

<<<<<<< Updated upstream
        // µ÷ÓÃGameManager¿ªÊ¼ÐÂÓÎÏ·
=======
        // è°ƒç”¨GameManagerå¼€å§‹æ–°æ¸¸æˆ
>>>>>>> Stashed changes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }
        else
        {
<<<<<<< Updated upstream
            Debug.LogError("GameManager.Instance Îª null£¡");
=======
            Debug.LogError("GameManager.Instance ä¸º nullï¼");
>>>>>>> Stashed changes
        }
    }

    public void OnContinueGameClick()
    {
<<<<<<< Updated upstream
        Debug.Log("¼ÌÐøÓÎÏ·°´Å¥±»µã»÷£¡");

        // ²¥·Å°´Å¥ÒôÐ§
=======
        Debug.Log("ç»§ç»­æ¸¸æˆæŒ‰é’®è¢«ç‚¹å‡»ï¼");

        // æ’­æ”¾æŒ‰é’®éŸ³æ•ˆ
>>>>>>> Stashed changes
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

<<<<<<< Updated upstream
        // µ÷ÓÃGameManager¼ÌÐøÓÎÏ·
=======
        // è°ƒç”¨GameManagerç»§ç»­æ¸¸æˆ
>>>>>>> Stashed changes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
<<<<<<< Updated upstream
            Debug.LogError("GameManager.Instance Îª null£¡");
        }
    }

    public void OnSettingsClick()
    {
        Debug.Log("ÉèÖÃ°´Å¥±»µã»÷£¡");

        // ²¥·Å°´Å¥ÒôÐ§
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // ´ò¿ªÉèÖÃÃæ°å
        if (UIManager.Instance != null)
        {
            UISettingsManager.Instance.OpenSettings();
        }
        else
        {
            Debug.LogError("UIManager.Instance Îª null£¡");
        }
=======
            Debug.LogError("GameManager.Instance ä¸º nullï¼");
        }
    }

    // æ³¨æ„ï¼šè¿™ä¸ªæ–¹æ³•çŽ°åœ¨å¯ä»¥ä¿ç•™ä½œä¸ºå¤‡ç”¨ï¼Œä½†æŽ¨èä½¿ç”¨LocalSettingsButtonè„šæœ¬
    public void OnSettingsClick()
    {
        Debug.Log("StartSceneè®¾ç½®æŒ‰é’®è¢«ç‚¹å‡»ï¼");

        // ä½¿ç”¨å…¨å±€è®¾ç½®ç®¡ç†å™¨æ‰“å¼€è®¾ç½®åœºæ™¯
        GlobalSettingsManager.OpenSettingsScene();
>>>>>>> Stashed changes
    }

    public void OnQuitGameClick()
    {
<<<<<<< Updated upstream
        Debug.Log("ÍË³öÓÎÏ·°´Å¥±»µã»÷£¡");

        // ²¥·Å°´Å¥ÒôÐ§
=======
        Debug.Log("é€€å‡ºæ¸¸æˆæŒ‰é’®è¢«ç‚¹å‡»ï¼");

        // æ’­æ”¾æŒ‰é’®éŸ³æ•ˆ
>>>>>>> Stashed changes
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

<<<<<<< Updated upstream
        // ÍË³öÓÎÏ·
#if UNITY_EDITOR
            // ÔÚ±à¼­Æ÷ÖÐÍ£Ö¹²¥·Å
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // ÔÚ´ò°üºóµÄÓÎÏ·ÖÐÍË³ö
=======
        // é€€å‡ºæ¸¸æˆ
#if UNITY_EDITOR
            // åœ¨ç¼–è¾‘å™¨ä¸­åœæ­¢æ’­æ”¾
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // åœ¨æ‰“åŒ…åŽçš„æ¸¸æˆä¸­é€€å‡º
>>>>>>> Stashed changes
        Application.Quit();
#endif
    }

    void Update()
    {
<<<<<<< Updated upstream
        // ¿ÉÒÔÔÚÕâÀïÌí¼Ó¼üÅÌ¿ì½Ý¼üÖ§³Ö
=======
        // å¯ä»¥åœ¨è¿™é‡Œæ·»åŠ é”®ç›˜å¿«æ·é”®æ”¯æŒ
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
