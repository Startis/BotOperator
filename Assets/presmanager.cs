using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class presmanager : MonoBehaviour {

	public GameObject parentslides;
	public int currentSlide;
	public int nombreSlide;
	public float movetime;
	public float returnbegginingtime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("o")) {
			currentSlide++;
			UpdateSlide();
		}
	}

	void UpdateSlide (){
		if (currentSlide < nombreSlide){
			switch (currentSlide)
			{
			case 0:
				parentslides.transform.DOLocalMoveX (0, returnbegginingtime, false);
				Debug.Log("blabla");
				break;
			case 1:
				parentslides.transform.DOLocalMoveX (-800, movetime, false);
				Debug.Log("blabla1");
				break;
			case 2:
				parentslides.transform.DOLocalMoveX (-2400, movetime, false);
				Debug.Log("blabla2");
				break;
			case 3:
				parentslides.transform.DOLocalMoveX (-4000, movetime, false);
				Debug.Log("blabla3");
				break;
			case 4:
				parentslides.transform.DOLocalMoveX (-5600, movetime, false);
				Debug.Log ("blabla4");
				break;
			case 5:
				parentslides.transform.DOLocalMoveX (-7200, movetime, false);
				Debug.Log("blabla5");
				break;
			}
		}else{
			currentSlide = 0;
		}

	}

}
