using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour, IInteractable
{
    [Range(1f, 3f)]
    public int weight;

    public bool picked;

    public void Interact()
    {
        Debug.Log("interacted with " + name);
    }

    private void ToggleOutline(bool value)
    {
        Debug.Log("outlined " + name);
    }
}
