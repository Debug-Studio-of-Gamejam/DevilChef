using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eddy : Interactable
{
    public GameObject Leviantan; 
    public override void Interact()
    {
        // TODO : 播放泉水音效
        if (Leviantan.activeInHierarchy == false)
        {
            Leviantan.SetActive(true);
        }
    }
}
