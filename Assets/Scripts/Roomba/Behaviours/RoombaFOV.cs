using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoombaFOV : MonoBehaviour
{
    public LayerMask mask;
    public int nRay;
    public int degrees;

    void FixedUpdate()
    {
        RaycastHit hit;
        if(nRay % 2 == 0)
        {

        }
        // Does the ray intersect any objects excluding the player layer
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.magenta);
        //    Debug.Log("Did not Hit");
        //}
    }
}
