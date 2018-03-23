using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyTurret : AbstractEnemy {

    public float[] rotateView;
    private int viewRank = 0;
    public float speedRotate = 2;
    public float waitBeforeRotate = 1;
    private Tweener tweenerRotate;
    private Tween tweenWait;

    private void Awake()
    {
        View();
    }

    // Update is called once per frame
    void Update () {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
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
}
