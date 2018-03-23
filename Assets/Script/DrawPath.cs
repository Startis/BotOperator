using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawPath : ActiveBehaviour {

    public Camera camera;
    private List<Vector3> positions;
    public float speed = 10;
    private bool canDraw = false;
    private Tweener tweenerMove;

    protected void Awake()
    {
        positions = new List<Vector3>();
    }

    // Update is called once per frame
    protected override void Update () {

        if (positions.Count > 0)
        {
            Debug.DrawRay(transform.position, positions[0] - transform.position, Color.red);
        }
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Debug.DrawRay(positions[i], positions[i + 1] - positions[i], Color.red);
        }


        if (GameManager.pause)
        {
            DrawPathMove();
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
                            positions.Add(hit.point.WithY(1.5f));
                            break;
                        case "Wall":
                            canDraw = false;
                            MoveLine();
                            break;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                canDraw = false;
                MoveLine();
            }
        }
    }


    private void MoveLine()
    {
        if(positions.Count == 0)
        {
            return;
        }

        tweenerMove = transform.DOMove(positions[0], Vector3.Distance(transform.position, positions[0]) / speed).OnComplete(() =>
        {
            positions.RemoveAt(0);
            tweenerMove = null;
            MoveLine();
        }).SetEase(Ease.Linear);
    }

    private void OnMouseDown()
    {
        canDraw = true;
        if(tweenerMove != null)
            tweenerMove.Kill();
        positions.Clear();
    }

    protected override void PauseTweener(bool isPause)
    {
        if(tweenerMove == null)
        {
            return;
        }

        if (isPause)
        {
            if (tweenerMove.IsPlaying())
            {
                tweenerMove.Pause();
            }
        }
        else
        {
            if (!tweenerMove.IsPlaying())
            {
                tweenerMove.Play();
            }
        }
    }
}
