using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public GameObject ItemButton;
   
    protected override void Interact()
    {
        Debug.Log("Interacted With " + gameObject.name);
    }
}
