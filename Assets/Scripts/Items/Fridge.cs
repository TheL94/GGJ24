using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : Item
{
    private void OnDestroy()
    {
        UIManager.Instance.stealTheFridge.gameObject.SetActive(false);
    }
}
