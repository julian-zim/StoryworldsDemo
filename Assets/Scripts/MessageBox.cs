﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    
    #region Constants
    
    private const float Speed = 0.025f;
    
    #endregion
    
    #region Variables
    
    private Image box;
    private Text text;
    
    private bool messageBusy;
    private bool messageDone;

    private int messagesLeft;
    private bool messagesFollowing;

    private LinkedList<string> messages;
    private LinkedList<string> authors;
    
    #endregion
    
    private void Start()
    {
        
        #region Initialization
        
        box = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();
        box.enabled = false;
        text.enabled = false;
        messagesLeft = 0;
        
        #endregion
        
    }

    private void Update()
    {
        
        #region Message Queueing
        
        // there are messages left
        if (messagesFollowing)
        {
            
            // the last message has been fully formulated
            if (messageDone)
            {
                
                // press space to show the next message in the next frame
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    messageDone = false;
                    if (messagesLeft < 1)
                    {
                        messagesFollowing = false;
                        box.enabled = false;
                        text.enabled = false;
                    }

                }

            }
            
            // the next message can be shown
            else if (!messageBusy)
            {
                messageBusy = true;
                text.text = authors.First.Value + ": ";
                authors.RemoveFirst();
                StartCoroutine(ShowMessage(messages.First.Value));
                messages.RemoveFirst();
                messagesLeft--;
            }
            
        }
        
        #endregion
        
    }
    
    #region Helper Functions

    public void ShowMessage(string author, string message)
    {
        LinkedList<string> authors = new LinkedList<string>();
        authors.AddLast(author);
        LinkedList<string> messages = new LinkedList<string>();
        messages.AddLast(message);
        ShowMessages(authors, messages);
    }
    
    public void ShowMessages(LinkedList<string> authors, LinkedList<string> messages)
    {
        
        if (authors.Count < 1 || messages.Count < 1)
        {
            throw new ArgumentException("@authors and @messages cannot be zero");
        }
        if (authors.Count != messages.Count)
        {
            throw new ArgumentException("@authors and @messages cannot be of different size");
        }

        if (!GetMessageActive())
        {
            box.enabled = true;
            text.enabled = true;
            this.authors = authors;
            this.messages = messages;
            messagesLeft = messages.Count;
            messagesFollowing = true;
        }
        
    }
    
    #endregion
    
    #region Coroutines
    
    private IEnumerator ShowMessage(string message)
    {
        foreach (char character in message)
        { 
            text.text += character;
            yield return new WaitForSeconds(Speed);
        }
        messageBusy = false;
        messageDone = true;
    }
    
    #endregion
    
    #region Setter & Getter
    
    public bool GetMessageActive()
    {
        return messageDone || messageBusy;
    }

    #endregion
    
}
