﻿using UnityEngine;
using UnityEngine.UI;

public class Window : Interactable
{
    
    [SerializeField] private Image cityImage;
    [SerializeField] private MessageBox messageBox;

    private void Awake()
    {
        cityImage.enabled = false;
    }

    protected override void SpecificAction()
    {
        cityImage.enabled = true;
    }

    protected override void SpecificUpdate()
    {
        
    }
    protected override void UndoSpecificAction()
    {
        cityImage.enabled = false;
        messageBox.ShowMonologue("Jordan", Texts.WindowSkylineMonologue);
    }
    
}