using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTips : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI DescText;
    
    public void UpdateItemTips(ItemDetails item)
    {
        nameText.text = item.name.ToString();
        DescText.text = item.description;
    }
}
