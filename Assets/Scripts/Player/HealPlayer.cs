using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    SmashButton smashButton;

    private void Start()
    {
        smashButton = GetComponent<SmashButton>();

        smashButton.onSmeshWin += () =>
        {
            Damage damageable = FindObjectOfType<Damage>();
            if (damageable != null)
            {
                damageable.Heal(1);
                Destroy(gameObject);
            }
        };
    }
}
