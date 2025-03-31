using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public string message;

    public UnityEvent onInteraction;

    public void Interact()
    {
        onInteraction.Invoke();
    }


}
