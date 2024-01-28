using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExtractionArea : MonoBehaviour
{
    LayerMask checklayer;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerInput>();
        if(player)
        {
            var slotManager = player.GetComponentInChildren<PlayerSlotManager>();
            GameManager.Instance.Points += slotManager.ReleaseAllItems(transform.position);
            Debug.Log("the score is: " + GameManager.Instance.Points);
        }
    }
    
    
}
