using UnityEngine;
using System.Collections;

public class BoxFlipper : MonoBehaviour {
	public enum BoxGravityState{
		Roof,
		Floor
	}
	public BoxGravityState _gravityState;
	public float boxJumpSpeed = 3.5f;
	public bool hitState = false;
//	public GameObject  bullet;
	// Use this for initialization
	void Start () {
		_gravityState = BoxGravityState.Floor;
	}
	
	// Update is called once per frame
	void Update () {
		if (hitState) {			
			Vector3 moveVector = transform.position;
			if(_gravityState == BoxGravityState.Floor)
			{
				Vector3 finalPosition = new Vector3(moveVector.x, 11.85f, moveVector.z);
				transform.position = Vector3.MoveTowards(moveVector, finalPosition, boxJumpSpeed * Time.deltaTime);
				if(transform.position == finalPosition){
					_gravityState = BoxGravityState.Roof;
					hitState = false;
				}
			}
			else if(_gravityState == BoxGravityState.Roof)
			{
				Vector3 finalPosition = new Vector3(moveVector.x, 3.85f, moveVector.z);
				transform.position = Vector3.MoveTowards(moveVector, finalPosition, boxJumpSpeed * Time.deltaTime);
				if(transform.position == finalPosition){
					_gravityState = BoxGravityState.Floor;
					hitState = false;
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider colldBox)
	{
		if (colldBox.tag == "Projectile") {
			Debug.Log("Collided");
			hitState = true;
			Destroy(colldBox.gameObject);
		}
	}
}
