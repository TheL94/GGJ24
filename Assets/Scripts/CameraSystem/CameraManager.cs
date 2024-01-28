using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineBrain brain;

    private CameraZone activeZone;
    private Coroutine currentRoutine;

    void Start()
    {
        foreach (var zone in CameraZone.zones)
        {
            zone.OnPlayerEnter += HandlePlayerEnter;
            // zone.OnPlayerExit += HandlePlayerExit;

            if (zone.isInitialZone)
            {
                activeZone = zone;
                zone.VirtualCamera.gameObject.SetActive(true);
            }
            else
            {
                zone.VirtualCamera.gameObject.SetActive(false);
            }
        }
    }

    void HandlePlayerEnter(CameraZone zone, GameObject player)
    {
        ActivateZone(zone);
    }

    void HandlePlayerExit(CameraZone zone, GameObject player)
    {
    }

    void ActivateZone(CameraZone zone)
    {
        if(zone == activeZone) return;
        
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(WaitForCameraTransition(zone));
    }

    IEnumerator WaitForCameraTransition(CameraZone zone)
    {
        var previusZone = activeZone;
        activeZone = zone;
        activeZone.VirtualCamera.gameObject.SetActive(true);
        yield return new WaitUntil(() => !brain.IsBlending);
        if(previusZone != null)
            previusZone.VirtualCamera.gameObject.SetActive(false);
        currentRoutine = null;
    }
}