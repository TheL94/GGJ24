using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CircleCountdown : MonoBehaviour
{
    Image smeshSprite;
    public Image pressSprite;

    Coroutine showPressSpriteCoroutine;

    private void Start()
    {
        smeshSprite = GetComponent<Image>();
        smeshSprite.fillAmount = SmashButton.startFillAmount;

        SmashButton.updateFillAmount += (f => UpdateFillAmount(f));
        SmashButton.onShowPressSprite += PressSprite;
    }

    public void UpdateFillAmount(float f)
    {
        smeshSprite.fillAmount = f;
    }

    public void PressSprite()
    {
        if (showPressSpriteCoroutine == null)
            showPressSpriteCoroutine = StartCoroutine(ShowPressSprite());
    }

    IEnumerator ShowPressSprite()
    {
        pressSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        pressSprite.gameObject.SetActive(false);

        showPressSpriteCoroutine = null;
        yield break;
    }
}
