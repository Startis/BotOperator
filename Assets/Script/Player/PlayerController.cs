using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : ActiveBehaviour {

    public enum AgressiveState
    {
        pacifism,
        inView,
        agressif
    }

    public PlayerUI playerUI { get; set; }
    public DrawPath playerDrawPath { get; set; }
    public ViewPlayer viewPlayer { get; set; }

    private NavMeshAgent agent;

    public AbstractEnemy enemyAggro { get; set; }
    public AbstractEnemy enemySelected { get; set; }
    public AgressiveState agressiveState = AgressiveState.pacifism;

    //state player
    public float life = 100;
    public float rangeShot = 5;
    public float delayShot = 2;
    private bool canShot { get { return cooldownShot == 0; } }
    private float cooldownShot = 0;
    public GameObject ammo;
    public float rangePunch = 2;
    public float delayPunch = 1;
    private float cooldownPunch = 0;
    private bool canPunch { get { return cooldownPunch == 0; } }
    public float damagePunch = 3;

    private void Awake()
    {
        (playerUI = GetComponent<PlayerUI>()).playerController = this;
        (playerDrawPath = GetComponent<DrawPath>()).playerManager = this;
        (viewPlayer = GetComponentInChildren<ViewPlayer>()).playerManager = this;

        agent = GetComponent<NavMeshAgent>();

        switch (agressiveState)
        {
            case AgressiveState.pacifism:
                playerUI.pacifism.image.color = Color.red;
                break;
            case AgressiveState.inView:
                playerUI.inView.image.color = Color.red;
                break;
            case AgressiveState.agressif:
                playerUI.agressif.image.color = Color.red;
                break;
        }
    }


    protected override void Update () {
        base.Update();
        if (GameManager.pause)
        {
            return;
        }
        cooldownPunch = Mathf.Max(0, cooldownPunch - Time.deltaTime);
        cooldownShot = Mathf.Max(0, cooldownShot - Time.deltaTime);
        if (!playerDrawPath.isMoving)
        {
            if(enemySelected != null)
            {
                transform.LookAt(enemySelected.transform);
            }
            else if(enemyAggro != null)
            {
                transform.LookAt(enemyAggro.transform);
                switch (agressiveState)
                {
                    case AgressiveState.pacifism:
                        break;
                    case AgressiveState.inView:
                        Attack();
                        break;
                    case AgressiveState.agressif:
                        if(Vector3.Distance(transform.position, enemyAggro.transform.position) > rangeShot)
                        {
                            agent.SetDestination(enemyAggro.transform.position);
                            agent.isStopped = false;
                        }
                        else
                        {
                            agent.isStopped = true;
                            Attack();
                        }
                        break;
                }
            }
        }
	}

    public void TakeDamage(float damage)
    {
        life = Mathf.Max(0, life - damage);
        playerUI.sliderHp.value = life;
    }

    public void Attack()
    {
        AbstractEnemy enemy = enemySelected != null ? enemySelected : enemyAggro;
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (distance <= rangePunch)
        {
            if (!canPunch)
            {
                return;
            }
            cooldownPunch = delayPunch;
            enemy.TakeDamage(damagePunch);
            Debug.Log("Punch !!!");
        }
        else if(distance <= rangeShot)
        {
            if (!canShot)
            {
                return;
            }
            cooldownShot = delayShot;
            var shot = Instantiate(ammo, transform.position, Quaternion.identity).GetComponent<Shot>();
            shot.SetDirection(transform.forward, Shot.Emetteur.player);
            Debug.Log("Shot !");
        }
    }

    protected override void PauseTweener(bool isPause)
    {
        
    }
}
