using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beel : Interactable
{

    public override void Interact()
    {
       CookingManager.Instance.ShowCookingPanel();
    }

}
