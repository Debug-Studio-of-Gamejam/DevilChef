using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderPressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioName audioName;
    private Slider slider;
    

    private void Reset()
    {
        slider = GetComponent<Slider>();
    }

    // 玩家按下 Slider（手指/鼠标落下）
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(audioName);
    }

    // 玩家松开 Slider（手指/鼠标抬起）
    public void OnPointerUp(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioName.None);
    }
}