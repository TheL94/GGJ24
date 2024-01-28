using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator bottonRaccon;
    public Animator middleRaccon;
    public Animator topRaccon;

    private PlayerPhysicMovement playerMovement;
    private Animator activeBottomRacoon;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerPhysicMovement>();
        playerMovement.OnBottonRacoonChange += Handle_OnBottonRacoonChange;
    }

    private void OnDisable()
    {
        playerMovement.OnBottonRacoonChange -= Handle_OnBottonRacoonChange;
    }

    private void Handle_OnBottonRacoonChange(GameObject racoon)
    {
        activeBottomRacoon = racoon.GetComponentInChildren<Animator>();
    }

    public void SetRun()
    {
        if (!activeBottomRacoon.GetCurrentAnimatorStateInfo(0).IsName("GoToRunning"))
            activeBottomRacoon.SetTrigger("GoToRunning");
    }

    public void SetIdle()
    {
        if (!activeBottomRacoon.GetCurrentAnimatorStateInfo(0).IsName("GoToIdle"))
            activeBottomRacoon.SetTrigger("GoToIdle");
    }

    public void SetPickUp()
    {
        if (activeBottomRacoon != bottonRaccon)
        {
            topRaccon.SetTrigger("GoToPickUp");
            middleRaccon.SetTrigger("GoToPickUp");
        }
        else
        {
            activeBottomRacoon.SetTrigger("GoToPickUp");
        }
    }
}