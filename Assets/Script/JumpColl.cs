using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpColl : MonoBehaviour
{
    public Movement player;
    public float force;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "sonic")
        {
            player.rb.velocity = Vector3.zero;
            player.speedCur = 0;
            player.rb.AddForce(transform.up * force, ForceMode.Impulse);
        }
    }
}
