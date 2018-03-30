using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Text.RegularExpressions;
using System;

public class DrawPath : ActiveBehaviour {

    public Camera cameraDraw;
    public List<Vector3> positions { get; private set; }
    public float speed = 10;
    private bool canDraw = false;
    private Tweener tweenerMove;
    private Tweener tweenerRotate;
    public PlayerController playerController { get; set; }
    public bool isMoving { get { return tweenerMove != null && tweenerMove.IsPlaying(); } }

    private bool isSkillView = false, useSkillView = false;
    public float delaySkillView = 0.5f;
    private Tweener tweenerSkillView = null;
    public int skillWeapon { get; set; }
    public float delaySkillAttack = 1;
    public Vector3 posMortier { get; set; }
    public GameObject mortier;
    public LineRenderer line;

    protected void Awake()
    {
        positions = new List<Vector3>();
        line = GetComponent<LineRenderer>();
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

        line.positionCount = positions.Count + 1;
        line.SetPosition(0, transform.position);
        for(int i = 0; i < positions.Count; i++)
        {
            line.SetPosition(i + 1, positions[i]);
        }

        if(skillWeapon == 1 || skillWeapon == 2)
        {
            PauseTweener(true);
            return;
        }

        DrawPathMove();
        base.Update();
    }

    private void DrawPathMove()
    {
        if (canDraw)
        {
            if (Input.GetMouseButton(0))
            {
                //positions.Add(camera.ScreenToWorldPoint(Input.mousePosition).WithY(1));
                Ray rayPos = cameraDraw.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(rayPos, out hit)) {
                    switch (hit.collider.tag)
                    {
                        case "Ground":
                            if(positions.Count == 0 || Vector3.Distance(hit.point.WithY(1.583333f), positions[positions.Count - 1]) > 0.3f)
                                positions.Add(hit.point.WithY(1.5f));
                            break;
                        case "Wall":
                            LeftConditionDraw();
                            break;
                        case "Destructible":
                            if(tag == "Player_Strong")
                            {
                                if (positions.Count == 0 || Vector3.Distance(hit.point.WithY(1.583333f), positions[positions.Count - 1]) > 0.3f)
                                    positions.Add(hit.point.WithY(1.5f));
                            }
                            else
                            {
                                /*line.positionCount = positions.Count;
                                line.SetPositions(positions.ToArray());*/
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

        if (isSkillView && tweenerSkillView == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray rayPos = cameraDraw.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(rayPos, out hit))
                {
                    tweenerSkillView = transform.DOLookAt(hit.point, delaySkillView).OnComplete(() =>
                    {
                        isSkillView = false;
                        useSkillView = true;
                        tweenerSkillView = null;
                        if (tweenerMove != null)
                        {
                            tweenerMove.Play();
                        }
                    });
                }
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
            useSkillView = false;
            return;
        }

        tweenerMove = transform.DOMove(positions[0], Vector3.Distance(transform.position, positions[0]) / speed).OnComplete(() =>
        {
            positions.RemoveAt(0);
            tweenerMove = null;
            MoveLine();
        }).SetEase(Ease.Linear);
        if (!useSkillView)
        {
            tweenerRotate = transform.DOLookAt(positions[0], Vector3.Distance(transform.position, positions[0]) / (speed * 2)).OnComplete(() =>
            {
                tweenerRotate = null;
            });
        }
    }

    private void OnMouseDown()
    {
        if(skillWeapon == 1 || skillWeapon == 2)
        {
            return;
        }
        canDraw = true;
        isSkillView = false;
        if (tweenerMove != null)
        {
            tweenerMove.Kill();
        }
        if(tweenerRotate != null)
        {
            tweenerRotate.Kill();
        }
        positions.Clear();
    }

    protected override void PauseTweener(bool isPause)
    {
        if(tweenerMove != null)
        {
            if(isPause && tweenerMove.IsPlaying())
            {
                tweenerMove.Pause();
            }else if(!isPause && !tweenerMove.IsPlaying() && !isSkillView)
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
        if(tag == "Player_Strong" && collision.gameObject.tag == "Destructible")
        {
            collision.gameObject.SetActive(false); // possible de remplacer par un collider desactiver et un changement de mesh
        }
        else if (collision.gameObject.name.Contains("SkillSphere"))
        {
            var id_s = Regex.Matches(collision.gameObject.name, "[0-9]");
            int temp = 0;
            string tempS = "";
            foreach(Match m in id_s)
            {
                tempS = m.Value;
            }
            temp = int.Parse(tempS);
            Debug.Log("skill " + temp);
            playerController.UseSkill(temp);
            Destroy(collision.gameObject);
        }
    }

    public void KillViewInMove()
    {
        if(tweenerRotate != null)
        {
            tweenerRotate.Kill();
            tweenerRotate = null;
        }
        if(tweenerMove != null && tweenerMove.IsPlaying())
        {
            tweenerMove.Pause();
        }
        isSkillView = true;
    }

    public IEnumerator SkillAttack(params object[] parameter)
    {
        yield return new WaitForEndOfFrame();
        if(skillWeapon == 2)
        {
            yield return new WaitForSeconds(delaySkillAttack);
            ((Delegate)parameter[0]).DynamicInvoke(transform.position);
            skillWeapon = 0;
            PauseTweener(false);

        }else if(skillWeapon == 1)
        {
            PauseTweener(true);
            var mor = Instantiate(mortier, transform.position + transform.forward * 2, Quaternion.identity);
            Destroy(mor, 0.5f);
            yield return new WaitForSeconds(delaySkillAttack);
            ((Delegate)parameter[0]).DynamicInvoke(posMortier);
            skillWeapon = 0;
            PauseTweener(false);
        }
        yield return null;
    }

    public void PauseDeguelasse(bool pause)
    {
        PauseTweener(pause);
    }
}
