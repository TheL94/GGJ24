using UnityEngine;

public class ExplodingInteraction : MonoBehaviour, IExplodable
{
    public GameObject brokenVase;
    public int probability = 30;

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
        {
            int random = Random.Range(1, 100);
            if(random <= probability)
             Break();
        }
    }
}
