using UnityEngine;

public class RoombaBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var roomba = other.GetComponent<RoombaBrain>();
        if (roomba)
        {
            roomba.transform.position = GameManager.Instance.enemySpawner.spawnPoints[0].transform.position;
        }
    }
}
