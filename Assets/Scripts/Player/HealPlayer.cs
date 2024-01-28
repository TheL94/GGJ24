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
            if (FindObjectOfType<PlayerMovementBehaviour>() is IDamageable damageable)
            {
                damageable.Heal(1);
                Destroy(gameObject);
            }
        };
    }
}
