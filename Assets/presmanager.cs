using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class presmanager : MonoBehaviour {

	public List<GameObject> slides;
	public int currentSlide;

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
		for (int i = 0; i < slides.Count; i++)
		{
			slides [i].SetActive (false);
		}
		switch (currentSlide)
		{
		case 0:
			slides [0].SetActive (true);
			Debug.Log("blabla");
			break;
		case 1:
			slides [1].SetActive (true);
			Debug.Log("blabla1");
			break;
		case 2:
			slides [2].SetActive (true);
			Debug.Log("blabla2");
			break;
		case 3:
			slides [3].SetActive (true);
			Debug.Log("blabla3");
			break;
		}
	}

}
