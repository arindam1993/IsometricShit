﻿using UnityEngine;
using System.Collections;

public class VideoScript : MonoBehaviour {


	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.P)){
			anim.SetTrigger("pan");
		}

	}
}
