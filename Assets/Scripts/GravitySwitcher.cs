using UnityEngine;
using System.Collections;

public class GravitySwitcher : MonoBehaviour {

	private Vector3 finalPositon;
	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		finalPositon = transform.position + new Vector3(0, 5, 0);
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space)){
			StartCoroutine("moveToRoof");
		}
	
	}

	IEnumerator moveToRoof(){
		while(transform.position != finalPositon){
			transform.position = Vector3.Lerp(initialPosition, finalPositon, 5 * Time.deltaTime);
			yield return null;
		}
	}
}
