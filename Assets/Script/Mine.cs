using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    public float damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<AbstractEnemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
