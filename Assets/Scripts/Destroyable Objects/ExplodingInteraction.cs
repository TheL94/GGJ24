using UnityEngine;

public class ExplodingInteraction : MonoBehaviour, IExplodable
{
    public GameObject brokenVase;

    public void Break()
    {
        brokenVase.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var roomba = collision.gameObject.GetComponent<RoombaBrain>();
        var player = collision.gameObject.GetComponent<PlayerPhysicMovement>();

        if (player != null || roomba != null) 
            Break();
    }
}
