using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : AbstractEnemy {

    public Transform[] PointOfPatrol;
    private int rankPatrol = 0;

    private int viewRank = 0;
    public float speedRotate = 2;
    public float waitBeforeRotate = 1;
    private NavMeshAgent agent;

    public float waitAfterViewPlayer = 1;
    private float cooldownWaitViewPlayer = 0;
    private bool isWaitPlayerView { get { return cooldownWaitViewPlayer == 0; } }

    public float delayShot = 2;
    public float delayCac = 1;

    private float cooldownShot = 0;
    private float cooldownCac = 0;

    private bool canShot { get { return cooldownShot == 0; } }
    private bool canPunch { get { return cooldownCac == 0; } }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        cooldownCac = Mathf.Max(0, cooldownCac - Time.deltaTime);
        cooldownShot = Mathf.Max(0, cooldownShot - Time.deltaTime);

        if (isPlayerView)
        {

            cooldownWaitViewPlayer = Mathf.Max(0, cooldownWaitViewPlayer - Time.deltaTime);
            if (!isWaitPlayerView)
            {
                return;
            }

            if (Vector3.Distance(playerAggro.transform.position, transform.position) > rangeShot)
            {
                agent.SetDestination(playerAggro.transform.position);
                agent.isStopped = false;
            }
            else
            {
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                transform.LookAt(playerAggro.transform);
                if (Vector3.Distance(playerAggro.transform.position, transform.position) <= rangeCac)
                {
                    if (!canPunch)
                        return;
                    Debug.Log("taper !!!");
                    cooldownCac = delayCac;
                    playerAggro.TakeDamage(damage);
                }
                else if (canShot)
                {
                    cooldownShot = delayShot;
                    var shot = Instantiate(ammo, transform.position, Quaternion.identity).GetComponent<Shot>();
                    shot.SetDirection(transform.forward);
                }
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if(PointOfPatrol.Length > 0)
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
            {
                agent.SetDestination(PointOfPatrol[rankPatrol].position);
                rankPatrol++;
                if(rankPatrol == PointOfPatrol.Length)
                {
                    rankPatrol = 0;
                }
            }
        }
    }

    public override void PlayerInView(PlayerManager player)
    {
        if (playerAggro == null)
        {
            cooldownWaitViewPlayer = waitAfterViewPlayer;
            isPlayerView = true;
            playerAggro = player;
        }
    }
}
