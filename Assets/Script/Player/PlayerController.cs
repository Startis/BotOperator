using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

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

    public int id = 0;

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

    public GameObject canvasSkill;
    public GameObject[] skillsSphere;
    private Vector3 posSkill;
    public GameObject targetMortier;

    private List<Delegate> skills;

    private void Awake()
    {
        playerUI = FindObjectOfType<PlayerUI>();
        (playerDrawPath = GetComponent<DrawPath>()).playerController = this;
        (viewPlayer = GetComponentInChildren<ViewPlayer>()).playerController = this;

        agent = GetComponent<NavMeshAgent>();

        if (id == 0)
        {
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

        skills = new List<Delegate>(PlayerSkill.skills);
    }


    protected override void Update () {
        Skill();

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
        if(playerUI.currentIdPlayer == id)
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

    private void Skill()
    {
        if (Input.GetMouseButtonDown(1) && playerDrawPath.positions.Count > 0)
        {
            Ray rayPos = playerDrawPath.cameraDraw.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(rayPos, out hit))
            {
                foreach(Vector3 v in playerDrawPath.positions)
                {
                    if(Vector3.Distance(v, hit.point.WithY(1.583333f)) <= 1)
                    {
                        /*var s = Instantiate(skills[0], hit.point.WithY(1.583333f), Quaternion.identity);
                        Destroy(s, 3);*/
                        posSkill = hit.point.WithY(1.583333f);
                        canvasSkill.transform.position = posSkill;
                        canvasSkill.SetActive(true);
                        return;
                    }
                }
                if (canvasSkill.activeInHierarchy)
                {
                    canvasSkill.SetActive(false);
                }
            }
        }
    }

    protected override void PauseTweener(bool isPause)
    {
        
    }

    private void OnMouseDown()
    {
        playerUI.ChangeRobotState(id, agressiveState);
        playerUI.sliderHp.value = life;
    }

    public void SkillInstanciate(int id)
    {
        canvasSkill.SetActive(false);
        var s = Instantiate(skillsSphere[id], posSkill, Quaternion.identity);
        if(id == 1)
        {
            StartCoroutine(TargetMortier());
        }
    }

    public void UseSkill(int id)
    {
        playerDrawPath.skillWeapon = id;
        switch (id)
        {
            case 0: //deplacement
                skills[id].DynamicInvoke(playerDrawPath);
                break;
            case 1: //mortier
                StartCoroutine(playerDrawPath.SkillAttack(skills[id]));
                //skills[id].DynamicInvoke(Vector3.zero);
                break;
            case 2: //mine
                //skills[id].DynamicInvoke(transform.position);
                StartCoroutine(playerDrawPath.SkillAttack(skills[id]));
                break;
            case 3:
                //skills[id].DynamicInvoke();
                break;
        }
    }

    private IEnumerator TargetMortier()
    {
        playerDrawPath.skillWeapon = 1;
        playerDrawPath.PauseDeguelasse(true);
        yield return new WaitUntil(() =>
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray rayPos = playerDrawPath.cameraDraw.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(rayPos, out hit))
                {
                    playerDrawPath.posMortier = Instantiate(targetMortier, hit.point, Quaternion.identity);
                    playerDrawPath.skillWeapon = 0;
                    playerDrawPath.PauseDeguelasse(false);
                    return true;
                }
            }
            return false;
        });
    }
}
