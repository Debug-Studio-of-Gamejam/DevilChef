using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEndingScene : MonoBehaviour
{

    // √: 评分失败的ID ：910 对话结束，出现图片成为鱼食，然后回到开始界面
    // √：评分成功：王的认可认可作为背景，对话ID根据轮次判断（1开头的ID）。 -> 进入下一个轮次的大地图
    //  第六轮， 成功之后 ->
    //                    分支1： 可以判断前置对话IDList 额外触发：920,
    //                          √ 选项 921(对话 921): 显示图片逃离不能（动画）回开始界面
    //                          √ 选项 922(对话 922): 进入第七轮的
    //                     分支2 ：直接回大地图
            
    // √ : 第七轮 王的认可认出现，换背景（待上传）触发对话 930 。对话后出现图片 永陷轮回然后回到开始界面。
    // 
    // 
    // //TODO: Bug 512,522 532 触发场景背景图闪烁
    //              712 722 732 触发人物立绘闪烁动画
    //              812,822,832,配好的乱码
    

    [Header("初始背景图")]
    public SpriteRenderer background;
    public Sprite successSprite;
    public Sprite failSprite;
    public Sprite finalSprite;
    public Sprite noEscapeSprite;
    public Sprite endlessSprite;
    public Animator animator;

    [Header("基本对话")]
    public const int failDialogueID = 910;
    private List<int> successDialogueID = new List<int>(){102,103,104,105,106,107};

    [Header("特殊对话")]
    public const int Round6DialogueID = 920;
    public const int noEscapeDialogueID = 921;
    public const int Round6successDialogueID = 922;
    public const int endlessDialogueID = 930;
    public List<List<int>> prerequisiteDialogueID = new List<List<int>>
    {
        new List<int> { 512, 522, 532 },
        new List<int> { 712, 722, 732 },
        new List<int> { 812, 822, 832 }
    };
    
    private void Start()
    {
        animator.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        successDialogueID = new List<int>(){102,103,104,105,106,107};
        bool isSuccess = GameManager.Instance.isSuccess;
        int currentRound = GameManager.Instance.currentRound;
        background.sprite = isSuccess ? successSprite : failSprite;
        if (!isSuccess)
        {
            DialogueSystem.Instance.ShowDialogue(failDialogueID);
        }
        else
        {
            if (currentRound == 7)
            {
                StartCoroutine(StartRound7Flow());
            }
            else
            {
                Debug.Log("currentRound : " + currentRound);
                DialogueSystem.Instance.ShowDialogue(successDialogueID[currentRound-1]);
            }
        }
    }

    private void OnDialogueFinished(int dialogueID)
    {
        if (dialogueID == failDialogueID)
        {
            StartCoroutine(ShowAndReturnToStart(failSprite));
        }
        else if (dialogueID == endlessDialogueID)
        {
            StartCoroutine(ShowAndReturnToStart(endlessSprite));
        }
        else if (dialogueID == noEscapeDialogueID)
        {
            animator.gameObject.SetActive(true);
            background.gameObject.SetActive(false);
            StartCoroutine(ShowNoEscape());
            //StartCoroutine(ShowAndReturnToStart(noEscapeSprite));
        }
        else if(dialogueID == 107)
        {
            //第六天的特殊对话
            bool allGroupsSatisfied = prerequisiteDialogueID.All(
                group => group.Any(id => GameManager.Instance.triggeredDialogues.Contains(id))
            );
            if (allGroupsSatisfied)
            {
                DialogueSystem.Instance.ShowDialogue(Round6DialogueID);
            }
            else
            {
                GameManager.Instance.StartNewRound();
            }
            
        }
        else if (dialogueID == Round6successDialogueID || successDialogueID.Contains(dialogueID))
        {
            GameManager.Instance.StartNewRound();
        }
    }

    private IEnumerator WaitAndGoToMap()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.StartNewRound();
    }

    private IEnumerator StartRound7Flow()
    {
        yield return new WaitForSeconds(2f);
        background.sprite = finalSprite;
        DialogueSystem.Instance.ShowDialogue(endlessDialogueID);
    }

    
    private IEnumerator ShowNoEscape()
    {
        string clipName = "noescape";
        animator.Play(clipName);
        yield return null;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(clipName))
        {
            yield return null;
        }
        float time = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time + 2f);
        TransitionManager.Instance.ReturnToStartScene();
    }

    private IEnumerator ShowAndReturnToStart(Sprite sprite)
    {
        background.sprite = sprite;
        yield return new WaitForSeconds(2f);
        TransitionManager.Instance.ReturnToStartScene();
    }

    private void OnEnable()
    {
        EventHandler.DialogueFinishedEvent += OnDialogueFinished;
    }

    private void OnDisable()
    {
        EventHandler.DialogueFinishedEvent -= OnDialogueFinished;
    }
}
