using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eddy : Interactable
{
    public GameObject Leviantan; 
    public override void Interact()
    {
        if (Leviantan.activeInHierarchy == false)
        {
            AudioManager.Instance.PlaySFX(AudioName.出现利维坦);
            Leviantan.SetActive(true);
        }
    }
}
