using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static bool pause = false;
    public Image iconPause;
    public Sprite[] icon;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }
	}

    public void Pause()
    {
        pause = !pause;
        iconPause.sprite = pause ? icon[0] : icon[1];
    }
}
