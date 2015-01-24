using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform roofTransform;
	public Camera mainCamera;
	public Transform lookAtTarget;
	public float cameraSpeed;

	private Transform floorTransform;
	private State _state;

	public enum State{
		Floor,
		Roof
	}


	// Use this for initialization
	void Start () {
		floorTransform = mainCamera.transform;
		_state = State.Floor;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(_state == State.Floor){
				StartCoroutine("switchToRoof");
				_state = State.Roof;
			}
			if(_state == State.Roof){

			}
		}
	
	}


	IEnumerator switchToRoof(){
		while(floorTransform.rotation != roofTransform.rotation){
			mainCamera.transform.localRotation = Quaternion.Lerp(floorTransform.localRotation, roofTransform.localRotation, cameraSpeed * Time.deltaTime);
			mainCamera.transform.localPosition = Vector3.Lerp(floorTransform.localPosition, roofTransform.localPosition, cameraSpeed * Time.deltaTime);
			//mainCamera.transform.LookAt(lookAtTarget.position);
			yield return null;
		}

	}

	IEnumerator switchToFloor(){
		while(roofTransform.rotation != floorTransform.rotation){
			mainCamera.transform.rotation = Quaternion.Slerp(roofTransform.rotation, floorTransform.rotation, cameraSpeed * Time.deltaTime);
			mainCamera.transform.position = Vector3.Slerp(roofTransform.position, floorTransform.position, cameraSpeed * Time.deltaTime);
			//mainCamera.transform.LookAt(lookAtTarget.position);
			yield return null;
		}
		
	}
}
