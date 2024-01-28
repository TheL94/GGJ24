using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExtractionArea : MonoBehaviour
{
    LayerMask checklayer;

    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerInput>();
        if(player)
        {
            m_audioSource.Play();
            var slotManager = player.GetComponentInChildren<PlayerSlotManager>();
            GameManager.Instance.Points += slotManager.ReleaseAllItems(transform.position);
            Debug.Log("the score is: " + GameManager.Instance.Points);
        }
    }
    
    
}
