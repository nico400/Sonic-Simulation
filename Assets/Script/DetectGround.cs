using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGround : MonoBehaviour
{
    public float offsetPos;
    public float raius;
    public LayerMask groundMask;
    public bool COOLL(Vector3 dir)
    {
        Vector3 posRay = transform.position + (dir * offsetPos);
        Collider[] coll = Physics.OverlapSphere(posRay, raius, groundMask);
        if (coll.Length > 0)
        {
            return true;
        }
        return false;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Vector3 Pos = transform.position + (-transform.up * offsetPos);
        Gizmos.DrawSphere(Pos, raius);
    }
}
