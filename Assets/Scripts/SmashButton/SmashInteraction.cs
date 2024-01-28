using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashInteraction : MonoBehaviour, IInteractable
{
    public SmashButton SmashButton;
    
    public GameObject canvasPrefab;
    GameObject canvas;
    
    public void Interact()
    {
        canvas = Instantiate(canvasPrefab, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);

        var countdown = canvas.GetComponentInChildren<CircleCountdown>();
        SmashButton.StartCountDown(countdown);
        SmashButton.onSmeshWin += SmashButton_onSmeshWin;
        SmashButton.onSmeshLose += SmashButton_onSmeshLose;
    }

    private void SmashButton_onSmeshLose()
    {
        SmashButton.onSmeshWin -= SmashButton_onSmeshWin;
        DestroyImmediate(canvas);
    }

    private void SmashButton_onSmeshWin()
    {
        SmashButton.onSmeshLose -= SmashButton_onSmeshLose;
        DestroyImmediate(canvas);
    }
}
