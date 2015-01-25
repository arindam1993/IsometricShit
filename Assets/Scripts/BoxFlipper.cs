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
	public float pushForce = 0.005f;
//	public GameObject  bullet;
	// Use this for initialization
	void Start () {
		if (this.gameObject.layer == LayerMask.NameToLayer ("Roof")) {
			_gravityState = BoxGravityState.Roof;
		} else {
			_gravityState = BoxGravityState.Floor;
		}
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
					this.gameObject.layer = LayerMask.NameToLayer("Roof");
					hitState = false;
				}
			}
			else if(_gravityState == BoxGravityState.Roof)
			{
				Vector3 finalPosition = new Vector3(moveVector.x, 3.85f, moveVector.z);
				transform.position = Vector3.MoveTowards(moveVector, finalPosition, boxJumpSpeed * Time.deltaTime);
				if(transform.position == finalPosition){
					_gravityState = BoxGravityState.Floor;
					this.gameObject.layer = LayerMask.NameToLayer("Floor");
					hitState = false;
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider colldBox)
	{Debug.Log (colldBox.tag);
		if (colldBox.tag == "Projectile") {
						hitState = true;
						Destroy (colldBox.gameObject);
		}
//		if (colldBox.tag == "Player") {
//					colldBox.gameObject.GetComponent<CharacterController>().Move(colldBox.gameObject.transform.forward * -1 * pushForce);
//				}
	}

	void OnTriggerStay(Collider colldBox)
	{
		if (colldBox.tag == "Player") {
			Vector3 pushVector = (this.transform.position - colldBox.gameObject.transform.position)* -1 * pushForce;
			colldBox.gameObject.GetComponent<CharacterController>().Move(pushVector.normalized);
		}
		
	}
}
