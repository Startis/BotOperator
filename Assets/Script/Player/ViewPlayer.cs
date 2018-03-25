using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPlayer : MonoBehaviour {

    public PlayerController playerManager { get; set; }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, other.transform.position - transform.position, out hit) && hit.transform.tag == "Enemy" && (playerManager.enemyAggro == null || 
                (Vector3.Distance(transform.position, playerManager.enemyAggro.transform.position) > Vector3.Distance(transform.position, other.transform.position))))
            {
                playerManager.enemyAggro = other.GetComponent<AbstractEnemy>();
            }
        }
    }
}
