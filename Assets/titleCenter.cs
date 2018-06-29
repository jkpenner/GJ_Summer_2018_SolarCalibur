using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleCenter : MonoBehaviour {

	// Use this for initialization
	void Start (){
		Vector3 pos = transform.position;
		pos.x = Screen.width / 2;
		transform.position = pos;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
