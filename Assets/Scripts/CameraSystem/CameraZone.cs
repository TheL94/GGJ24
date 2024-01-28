using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraZone : MonoBehaviour
{
    public static List<CameraZone> zones = new List<CameraZone>();

    public UnityAction<CameraZone, GameObject> OnPlayerEnter;
    public UnityAction<CameraZone, GameObject> OnPlayerExit;
    
    public CinemachineVirtualCamera VirtualCamera;
    public bool isInitialZone = false;

    void Awake()
    {
        zones.Add(this);
    }

    void OnDestroy()
    {
        zones.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerInput>();
        if (player)
        {
            OnPlayerEnter?.Invoke(this, player.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerInput>();
        if (player)
        {
            OnPlayerExit?.Invoke(this, player.gameObject);
        }
    }
}
