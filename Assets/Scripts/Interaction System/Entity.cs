using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("interacted with " + name);
    }

    private void ToggleOutline(bool value)
    {
        Debug.Log("outlined " + name);
    }
}
