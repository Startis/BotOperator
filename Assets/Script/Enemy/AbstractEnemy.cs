using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour {

    public float life = 10;
    public float damage = 3;
    //public float speed;
    public float rangeShot = 5;
    public float rangeCac = 1;
    protected bool isPlayerView = false;
    protected PlayerManager playerAggro;
    public GameObject ammo;

    public abstract void PlayerInView(PlayerManager player);
}
