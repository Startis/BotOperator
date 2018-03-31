using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.tag == "Enemy")
        {
            other.GetComponent<AbstractEnemy>().TakeDamage(damage);
        }else if(other.tag == "Destructible")
        {
            other.gameObject.SetActive(false);
        }
    }
}
