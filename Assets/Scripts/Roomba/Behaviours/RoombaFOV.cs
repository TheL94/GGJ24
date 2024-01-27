using UnityEngine;

public class RoombaFOV : MonoBehaviour
{
    float RayDegree { get => nRay % 2 != 0 ? (float)(degrees / nRay - 1) : (degrees / 2) / (float)nRay; }

    public int nRay;
    public float degrees;
    public float distance;

    private bool ShootStraightRay(out RaycastHit hit, LayerMask mask)
    {
        hit = new RaycastHit();

        if (nRay % 2 != 0)
            return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask);
        else
            return false;
    }

    public int CountRaysSideways(bool rightSide)
    {
        float angle = rightSide ? RayDegree : -RayDegree;

        // ~0 equals "everything" for layermask
        int hits = ShootStraightRay(out RaycastHit hit, ~0) ? 1 : 0;

        for (int i = 1; i < nRay / 2 + 1; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.AngleAxis(angle * i, Vector3.up) * transform.forward, out hit, distance, ~0))
            {
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle * i, Vector3.up) * transform.forward, Color.yellow);
                hits++;
            }
            else
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle * i, Vector3.up) * transform.forward * distance, Color.magenta);
        }

        return hits;
    }

    public bool FindObject(LayerMask mask, out RaycastHit hit)
    {
        return FindObjectSideways(true, out hit, mask) || FindObjectSideways(false, out hit, mask);
    }

    private bool FindObjectSideways(bool rightSide, out RaycastHit hit, LayerMask mask)
    {
        float angle = rightSide ? RayDegree : -RayDegree;

        if (ShootStraightRay(out hit, mask))
            return true;

        for (int i = 1; i < nRay / 2 + 1; i++)
        {
            if (Physics.Raycast(transform.position, Quaternion.AngleAxis(angle * i, Vector3.up) * transform.forward, out hit, distance, mask))
            {
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle * i, Vector3.up) * transform.forward, Color.yellow);
                return true;
            }
            else
                Debug.DrawRay(transform.position, Quaternion.AngleAxis(angle * i, Vector3.up) * transform.forward * distance, Color.magenta);
        }
        return false;
    }
}
