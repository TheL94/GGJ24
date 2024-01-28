using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashInteraction : MonoBehaviour, IInteractable
{
    public SmashButton SmashButton;
    public GameObject smashCanvas;

    public LayerMask itemMask;
    
    public void Interact()
    {
        smashCanvas.SetActive(true);
        smashCanvas.transform.position = new Vector3(transform.position.x, 1000f, transform.position.z);
        Physics.Raycast(new Ray(smashCanvas.transform.position, Vector3.down), out RaycastHit hit, Mathf.Infinity, itemMask);
        smashCanvas.transform.position = new Vector3(transform.position.x, hit.point.y + 0.05f, transform.position.z);

        var countdown = smashCanvas.GetComponentInChildren<CircleCountdown>();
        SmashButton.StartCountDown(countdown);

        SmashButton.onSmeshWin -= SmashButton_onSmeshWin;
        SmashButton.onSmeshWin += SmashButton_onSmeshWin;
        SmashButton.onSmeshLose -= SmashButton_onSmeshLose;
        SmashButton.onSmeshLose += SmashButton_onSmeshLose;
    }

    private void SmashButton_onSmeshLose()
    {
        SmashButton.ResetState();
        smashCanvas.SetActive(false);
    }

    private void SmashButton_onSmeshWin()
    {
        SmashButton.ResetState();
        smashCanvas.SetActive(false);
    }
}
