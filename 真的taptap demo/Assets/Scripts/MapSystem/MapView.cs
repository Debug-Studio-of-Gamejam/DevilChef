using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    public string currentMapName;

    public void onClickReturnButton()
    {
        TransitionManager.Instance.BackToMap();
    }

    
}
