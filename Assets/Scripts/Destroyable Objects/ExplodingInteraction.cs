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

    //TO RREPLACE WITH INTERACTIONS WITH EXPLOSIONS/FORCE
    private void OnMouseDown()
    {
        Break();
    }
}
