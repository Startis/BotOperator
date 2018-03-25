using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyTurret : AbstractEnemy {

    public float[] rotateView;
    private int viewRank = 0;
    public float speedRotate = 2;
    public float waitBeforeRotate = 1;
    private Tweener tweenerRotate;
    private Tween tweenWait;
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
        View();
    }

    // Update is called once per frame
    protected override void Update () {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        base.Update();
        if (GameManager.pause)
        {
            agent.isStopped = true;
            return;
        }

        cooldownCac = Mathf.Max(0, cooldownCac - Time.deltaTime);
        cooldownShot = Mathf.Max(0, cooldownShot - Time.deltaTime);

        if (isPlayerView)
        {
            if(tweenerRotate != null && tweenerRotate.IsPlaying())
                tweenerRotate.Pause();
            if (tweenWait != null && tweenWait.IsPlaying())
                tweenWait.Pause();

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
                if(Vector3.Distance(playerAggro.transform.position, transform.position) <= rangeCac)
                {
                    if (!canPunch)
                        return;
                    Debug.Log("taper !!!");
                    cooldownCac = delayCac;
                    playerAggro.TakeDamage(damage);
                }
                else if(canShot)
                {
                    cooldownShot = delayShot;
                    var shot = Instantiate(ammo, transform.position, Quaternion.identity).GetComponent<Shot>();
                    shot.SetDirection(transform.forward, Shot.Emetteur.enemy);
                }
            }
        }
	}

    private void View()
    {
        if(rotateView.Length > 0)
        {
            tweenerRotate = transform.DORotate(Vector3.up * rotateView[viewRank], speedRotate).OnComplete(() =>
            {
                viewRank++;
                if(viewRank == rotateView.Length)
                {
                    viewRank = 0;
                }
                tweenWait = DOVirtual.DelayedCall(waitBeforeRotate, View);
            });
        }
    }

    public override void PlayerInView(PlayerController player)
    {
        if (playerAggro == null)
        {
            cooldownWaitViewPlayer = waitAfterViewPlayer;
            isPlayerView = true;
            playerAggro = player;
        }
    }

    protected override void PauseTweener(bool isPause)
    {
        if (isPause)
        {
            if(tweenerRotate != null)
            {
                tweenerRotate.Pause();
            }
        }
        else
        {
            if (tweenerRotate != null)
            {
                tweenerRotate.Play();
            }
        }
    }
}
