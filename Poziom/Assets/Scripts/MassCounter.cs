using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCounter : MonoBehaviour
{
    public int TotalMass;
    private void FixedUpdate()
    {
        SphereCast();
    }

    public void SphereCast()
    {
        RaycastHit[] hit;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        hit = Physics.SphereCastAll(transform.position, 5, transform.forward);
        TotalMass = 0;
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.tag == "CanBeLifted")
            {
                TotalMass += hit[i].collider.GetComponent<ObjectParam>().Mass;
            }
        }
    }
}
