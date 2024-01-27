using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SmashButton : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction smashAction;

    public static UnityAction<float> updateFillAmount;
    public static UnityAction onShowPressSprite;
    public static UnityAction onSmeshWin;
    public static UnityAction onSmeshLose;

    public static float startFillAmount = 0.3f;
    public float increment;
    public float decrement;

    float fillAmount;
    Coroutine countdownCoroutine;

    public static bool isSmeshActive;

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        smashAction = playerInput.actions.FindAction("Smash");

        smashAction.performed -= OnButtonPress;
        smashAction.performed += OnButtonPress;

        fillAmount = startFillAmount;
        countdownCoroutine = StartCoroutine(Countdown());
        isSmeshActive = true;
    }

    private void OnButtonPress(InputAction.CallbackContext ctx)
    {
        fillAmount += increment;
        updateFillAmount?.Invoke(fillAmount);
        onShowPressSprite?.Invoke();
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
                smashAction.performed -= OnButtonPress;
                isSmeshActive = false;
                countdownCoroutine = null;
                yield break;
            }
            if (fillAmount <= 0.02f)
            {
                onSmeshLose?.Invoke();
                smashAction.performed -= OnButtonPress;
                Debug.Log("Hai perso");
                isSmeshActive = false;
                countdownCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }
}
