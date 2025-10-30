using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBackground : MonoBehaviour
{
    Animator animator;
    List<int> ids = new List<int>(){512,522,532};
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnStartDialogue(int id)
    {
        if (ids.Contains(id))
        {
            animator.Play("garden");
        }
    }

    private void OnEnable()
    {
        EventHandler.DialogueStartdEvent += OnStartDialogue;
    }

    private void OnDisable()
    {
        EventHandler.DialogueStartdEvent -= OnStartDialogue;
    }
}
