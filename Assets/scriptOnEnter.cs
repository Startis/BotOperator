using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptOnEnter : MonoBehaviour {

	public GameObject objectFirst;
	public GameObject objectAfter;
	private Collider col;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision){
		if (collision.transform.tag == "Player_Strong") {
			objectFirst.SetActive (false);
			objectAfter.SetActive (true);
			col.enabled = false;
		}
	}
}
