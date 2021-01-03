﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Datapad : Interactable
{
    [SerializeField] private Image datapadImage;
    protected override void SpecificAction()
    {
        datapadImage.enabled = true;
    }

    protected override void UndoSpecificAction()
    {
        datapadImage.enabled = false;
    }
    
    private void Update()
    {
        if (Active && (Input.GetButtonDown("UndoInteract") || Input.GetMouseButtonDown(0)))
        {
            UndoAction();
        }
    }
}
