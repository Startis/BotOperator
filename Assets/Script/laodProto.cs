using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void DestroyShot();

public class laodProto : MonoBehaviour {


	public static event DestroyShot destroyShot;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadTheProto(){
		Debug.Log ("proto load");
		SceneManager.LoadScene(1, LoadSceneMode.Additive);
	}

	public void ReloadTheUI(){
		Debug.Log ("unload proto");
		SceneManager.UnloadSceneAsync (1);
		if (destroyShot != null){
			destroyShot ();
		}
	}
}
