using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{

    public Inventory inventory;
    public GameObject ItemButton;
   
    protected override void Interact()
    {
        Debug.Log("Interacted With " + gameObject.name);
        
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.IsFull[i] == false)
            {
                // Add The Object To The Inventory

                inventory.IsFull[i] = true;

                Instantiate(ItemButton, inventory.slots[i].transform, false);

                Destroy(gameObject);

                break;
            }
        }
    }
}
