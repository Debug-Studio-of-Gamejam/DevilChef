using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public void StartGame()
    {
        AudioManager.Instance.PlaySFX(AudioName.主界面按键音);
        GameManager.Instance.StartNewGame();
    }

    public void ContinueGame()
    {
        AudioManager.Instance.PlaySFX(AudioName.主界面按键音);
        GameManager.Instance.ContinueGame();
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlaySFX(AudioName.按键音效);
        UIManager.Instance.ShowSettingPanel();
    }

}
