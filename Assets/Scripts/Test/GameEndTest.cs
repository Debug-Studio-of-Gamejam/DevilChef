using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndTest : MonoBehaviour
{
    public TMP_InputField textField;
    public Toggle toggle;

    public void TestGameEnd()
    {
        if (int.TryParse(textField.text, out int value))
        {
            GameManager.Instance.triggeredDialogues.Add(512);
            GameManager.Instance.triggeredDialogues.Add(712);
            GameManager.Instance.triggeredDialogues.Add(812);
            GameManager.Instance.currentRound = value;
            GameManager.Instance.OnScoreEvaluated(toggle.isOn);
        }
    }
}
