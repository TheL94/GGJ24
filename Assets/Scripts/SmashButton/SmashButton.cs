using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SmashButton : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction smashAction;

    public static UnityAction<float> updateFillAmount;
    public static UnityAction onSmeshWin;
    public static UnityAction onSmeshLose;

    public static float startFillAmount = 0.3f;
    public float increment;
    public float decrement;

    float fillAmount;
    Coroutine countdownCoroutine;

    public static bool isSmeshActive;

    private CircleCountdown circleCountdown; 

    private void Start()
    {
        smashAction = playerInput.actions.FindAction("Smash");

        smashAction.performed -= OnButtonPress;
        smashAction.performed += OnButtonPress;

        fillAmount = startFillAmount;
        isSmeshActive = true;
    }

    public void StartCountDown(CircleCountdown countdown)
    {
        circleCountdown = countdown;
        countdownCoroutine = StartCoroutine(Countdown());
    }

    private void OnButtonPress(InputAction.CallbackContext ctx)
    {
        fillAmount += increment;
        updateFillAmount?.Invoke(fillAmount);
    }

    IEnumerator Countdown()
    {
        float counter = 0;

        while (true)
        {
            counter += Time.deltaTime;

            if (counter >= 1)
            {
                fillAmount -= decrement;
                updateFillAmount?.Invoke(fillAmount);
                counter = 0;
            }

            if (fillAmount >= 1)
            {
                Debug.Log("Hai vinto");
                onSmeshWin?.Invoke();
                isSmeshActive = false;
                countdownCoroutine = null;
                yield break;
            }
            if (fillAmount <= 0.02f)
            {
                onSmeshLose?.Invoke();
                Debug.Log("Hai perso");
                isSmeshActive = false;
                countdownCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }

    public void ResetState()
    {
        fillAmount = startFillAmount;
        updateFillAmount?.Invoke(fillAmount);
    }
}
