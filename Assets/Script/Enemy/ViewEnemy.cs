using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewEnemy : MonoBehaviour {

    public float coneView = 60;
    private AbstractEnemy self;

    private void Awake()
    {
        transform.forward = transform.parent.forward;
        self = transform.parent.GetComponent<AbstractEnemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        RaycastHit hit;
        if (other.tag.Contains("Player") && Physics.Raycast(transform.position, other.transform.transform.position - transform.position, out hit) && hit.transform.tag.Contains("Player"))
        {
            float angle = Vector3.Angle(transform.position - other.transform.position, -transform.forward);
            if (angle <= coneView)
            {
                self.PlayerInView(other.GetComponent<PlayerController>());
            }
        }
    }
}
