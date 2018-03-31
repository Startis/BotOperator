using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changetext : MonoBehaviour {

	public Text myText;
	private string newstringvalue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		newstringvalue = System.DateTime.Now.ToString();
		myText.text =  newstringvalue;
	}
}
