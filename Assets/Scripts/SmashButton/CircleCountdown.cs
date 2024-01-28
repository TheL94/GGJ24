using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CircleCountdown : MonoBehaviour
{
    Image smeshSprite;
    public Image pressSprite;

    private void Start()
    {
        smeshSprite = GetComponent<Image>();
        smeshSprite.fillAmount = SmashButton.startFillAmount;

        SmashButton.updateFillAmount += (f => UpdateFillAmount(f));
    }

    public void UpdateFillAmount(float f)
    {
        smeshSprite.fillAmount = f;
    }
}
