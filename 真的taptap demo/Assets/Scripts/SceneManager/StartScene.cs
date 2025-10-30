using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
    }

    public void OpenSettings()
    {
        UIManager.Instance.ShowSettingPanel();
    }

}
