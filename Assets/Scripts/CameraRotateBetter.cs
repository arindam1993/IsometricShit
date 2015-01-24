using UnityEngine;
using System.Collections;

public class CameraRotateBetter : MonoBehaviour {

	public Transform finalTransform;
	private Quaternion finalRotation;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		initialRotation = transform.rotation;
		finalRotation = finalTransform.rotation;

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			StartCoroutine("rotateFocus");
		}
	
	}

	IEnumerator rotateFocus(){
		while(true){
			transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRotation, 5);
			yield return null;
		}
	}
}
