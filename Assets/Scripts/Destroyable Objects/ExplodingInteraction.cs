using UnityEngine;

public class ExplodingInteraction : MonoBehaviour, IExplodable
{
    public GameObject brokenVase;
    public float breakForce;

    public void Break()
    {
        brokenVase.SetActive(true);
        gameObject.SetActive(false);
    }
}
