using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class DrawPathNavMesh : ActiveBehaviour {

    public Camera camera;
    public List<Vector3> positions { get; private set; }
    public float speed = 10;
    private bool canDraw = false;
    private Tweener tweenerMove;
    private Tweener tweenerRotate;
    private NavMeshAgent agent;
    public PlayerController playerManager { get; set; }

    protected void Awake()
    {
        positions = new List<Vector3>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected override void Update () {

        if (positions.Count > 0)
        {
            Debug.DrawRay(transform.position, positions[0] - transform.position, Color.red);
            Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        }
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Debug.DrawRay(positions[i], positions[i + 1] - positions[i], Color.red);
        }


        DrawPathMove();
        //Debug.Log("destination = " + agent.destination);
        if(positions.Count > 0 && !agent.isStopped && agent.destination == transform.position.WithY(agent.destination.y))
        {
            positions.RemoveAt(0);
            MoveLine();
        }
        base.Update();
    }

    private void DrawPathMove()
    {
        if (canDraw)
        {
            if (Input.GetMouseButton(0))
            {
                //positions.Add(camera.ScreenToWorldPoint(Input.mousePosition).WithY(1));
                Ray rayPos = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(rayPos, out hit)) {
                    switch (hit.collider.tag)
                    {
                        case "Ground":
                            if(positions.Count == 0 || Vector3.Distance(hit.point.WithY(0.6f), positions[positions.Count - 1]) > 0.3f)
                                positions.Add(hit.point.WithY(1.5f));
                            break;
                        case "Wall":
                            LeftConditionDraw();
                            break;
                        case "Destructible":
                            if(tag == "Player_Strong")
                            {
                                if (positions.Count == 0 || Vector3.Distance(hit.point.WithY(0.6f), positions[positions.Count - 1]) > 0.3f)
                                    positions.Add(hit.point.WithY(1.5f));
                            }
                            else
                            {
                                LeftConditionDraw();
                            }
                            break;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                LeftConditionDraw();
            }
        }
    }

    private void LeftConditionDraw()
    {
        canDraw = false;
        MoveLine();
    }

    private void MoveLine()
    {
        if(positions.Count == 0)
        {
            return;
        }

        agent.SetDestination(positions[0]);

        /*tweenerMove = transform.DOMove(positions[0], Vector3.Distance(transform.position, positions[0]) / speed).OnComplete(() =>
        {
            positions.RemoveAt(0);
            tweenerMove = null;
            MoveLine();
        }).SetEase(Ease.Linear);
        tweenerRotate = transform.DOLookAt(positions[0], Vector3.Distance(transform.position, positions[0]) / (speed * 2)).OnComplete(() =>
        {
            tweenerRotate = null;
        });*/
    }

    private void OnMouseDown()
    {
        canDraw = true;
        /*if (tweenerMove != null)
        {
            tweenerMove.Kill();
        }
        if(tweenerRotate != null)
        {
            tweenerRotate.Kill();
        }*/
        positions.Clear();
    }

    protected override void PauseTweener(bool isPause)
    {
        if(tweenerMove != null)
        {
            if(isPause && tweenerMove.IsPlaying())
            {
                tweenerMove.Pause();
            }else if(!isPause && !tweenerMove.IsPlaying())
            {
                tweenerMove.Play();
            }
        }

        if (tweenerRotate != null)
        {
            if (isPause && tweenerRotate.IsPlaying())
            {
                tweenerRotate.Pause();
            }
            else if (!isPause && !tweenerRotate.IsPlaying())
            {
                tweenerRotate.Play();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if(tag == "Player_Strong" && collision.gameObject.tag == "Destructible")
        {
            collision.gameObject.SetActive(false); // possible de remplacer par un collider desactiver et un changement de mesh
        }
    }
}
