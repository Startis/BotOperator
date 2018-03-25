using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class ActiveBehaviour : MonoBehaviour {

	// Update is called once per frame
	protected virtual void Update () {
        PauseTweener(GameManager.pause);
	}

    protected abstract void PauseTweener(bool isPause);
}
