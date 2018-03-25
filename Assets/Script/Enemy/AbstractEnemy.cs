using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : ActiveBehaviour {

    public float life = 10;
    public float damage = 3;
    //public float speed;
    public float rangeShot = 5;
    public float rangeCac = 1;
    protected bool isPlayerView = false;
    protected PlayerController playerAggro;
    public GameObject ammo;

    public abstract void PlayerInView(PlayerController player);

    public void TakeDamage(float damage)
    {
        life -= damage;
        if(life <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
