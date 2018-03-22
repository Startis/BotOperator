using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawPath : MonoBehaviour {

    public Camera camera;
    private List<Vector3> positions;
    public float speed = 10;
    private bool canDraw = false;

    private void Awake()
    {
        positions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update () {
        DrawPathMove();
	}

    private void DrawPathMove()
    {
        if (positions.Count > 0)
        {
            Debug.DrawRay(transform.position, positions[0] - transform.position, Color.red);
        }
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Debug.DrawRay(positions[i], positions[i + 1] - positions[i], Color.red);
        }

        if (canDraw)
        {
            if (Input.GetMouseButtonDown(0))
            {
                positions.Clear();
            }
            else if (Input.GetMouseButton(0))
            {
                //positions.Add(camera.ScreenToWorldPoint(Input.mousePosition).WithY(1));
                Ray rayPos = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(rayPos, out hit);
                positions.Add(hit.point.WithY(1.5f));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                foreach (Vector3 v in positions)
                {
                    Debug.Log("pos = " + v);
                }
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

        transform.DOMove(positions[0], Vector3.Distance(transform.position, positions[0]) / speed).OnComplete(() =>
        {
            positions.RemoveAt(0);
            MoveLine();
        });
    }

    private void OnMouseDown()
    {
        canDraw = true;
    }
}
