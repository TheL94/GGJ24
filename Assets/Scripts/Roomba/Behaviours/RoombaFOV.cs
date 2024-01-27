using UnityEngine;

public class RoombaFOV : MonoBehaviour
{
    public LayerMask mask;
    public int nRay;
    public float degrees;
    public float distance;

    public int leftCasted = 0;
    public int rightCasted = 0;

    void FixedUpdate()
    {
        leftCasted = 0;
        rightCasted = 0;

        RaycastHit hit;
        float rayForDegree = 0f;
        if(nRay % 2 != 0)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                leftCasted++;
                rightCasted++;
            }
            else
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance, Color.magenta);

            rayForDegree = (float)(degrees / nRay - 1);
        }
        else
            rayForDegree = (degrees / 2) / (float)nRay;

        //LEFT
        for (int i = 1; i < nRay / 2 + 1; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-rayForDegree * i, Vector3.up) * transform.forward, out hit, distance, mask))
            {
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(-rayForDegree * i, Vector3.up) * transform.forward, Color.yellow);
                leftCasted++;
            }
            else
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(-rayForDegree * i, Vector3.up) * transform.forward * distance, Color.magenta);
        }

        //RIGHT
        for (int i = 1; i < nRay / 2 + 1; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.AngleAxis(rayForDegree * i, Vector3.up) * transform.forward, out hit, distance, mask))
            {
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(rayForDegree * i, Vector3.up) * transform.forward, Color.yellow);
                rightCasted++;
            }
            else
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(rayForDegree * i, Vector3.up) * transform.forward * distance, Color.magenta);
        }
    }
}
