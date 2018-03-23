using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class ActiveBehaviour : MonoBehaviour {

	// Update is called once per frame
	protected virtual void Update () {
        PauseTweener(GameManager.pause);
        if (GameManager.pause)
        {
            return;
        }
	}

    protected abstract void PauseTweener(bool isPause);
}
