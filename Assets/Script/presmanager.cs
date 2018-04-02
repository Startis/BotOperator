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
		if(Input.GetKeyUp("i")){
			if (currentSlide > 0) {
			currentSlide = currentSlide -1;
			UpdateSlide ();
		}else{
			currentSlide = 0;
		}
		}
		if(Input.GetKey("1")){
			currentSlide = 0;
			UpdateSlide ();
		}
	}

	void UpdateSlide (){
		if (currentSlide < nombreSlide){
			switch (currentSlide) {
			case 0:
				parentslides.transform.DOLocalMoveX (780, returnbegginingtime, false);
				Debug.Log ("blabla");
				break;
			case 1:
				parentslides.transform.DOLocalMoveX (-815, movetime, false);
				Debug.Log ("blabla1");
				break;
			case 2:
				parentslides.transform.DOLocalMoveX (-2415, movetime, false);
				Debug.Log ("blabla2");
				break;
			case 3:
				parentslides.transform.DOLocalMoveX (-4015, movetime, false);
				Debug.Log ("blabla3");
				break;
			case 4:
				parentslides.transform.DOLocalMoveX (-5615, movetime, false);
				Debug.Log ("blabla4");
				break;
			case 5:
				parentslides.transform.DOLocalMoveX (-7215, movetime, false);
				Debug.Log ("blabla5");
				break;
			case 6:
				parentslides.transform.DOLocalMoveX (-8815, movetime, false);
				Debug.Log ("blabla5");
				break;
			case 7:
				parentslides.transform.DOLocalMoveX (-10415, movetime, false);
				break;
			case 8:
				parentslides.transform.DOLocalMoveX (-12015, movetime, false);
				Debug.Log ("blabla7");
				break;
			case 9:
				parentslides.transform.DOLocalMoveX (-13615, movetime, false);
				Debug.Log ("blabla8");
				break;
			case 10:
				parentslides.transform.DOLocalMoveX (-15215, movetime, false);
				Debug.Log ("blabla9");
				break;
			case 11:
				parentslides.transform.DOLocalMoveX (-16815, movetime, false);
				Debug.Log ("blabla10");
				break;
			case 12:
				parentslides.transform.DOLocalMoveX (-18415, movetime, false);
				Debug.Log ("blabla11");
				break;
			}
		}else{
			currentSlide = 0;
			UpdateSlide ();
		}

	}

}
