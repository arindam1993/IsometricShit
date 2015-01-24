using UnityEngine;
using System.Collections;

public class CameraRotateBetter : MonoBehaviour {

	private Quaternion finalRotation;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		initialRotation = transform.localRotation;
		finalRotation = Quaternion.AngleAxis(180, transform.right);

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			StartCoroutine("rotateFocus");
		}
	
	}

	IEnumerator rotateFocus(){
		while(transform.localRotation != finalRotation){
			transform.localRotation = Quaternion.Lerp(initialRotation, finalRotation, 5 * Time.deltaTime);
			yield return null;
		}
	}
}
