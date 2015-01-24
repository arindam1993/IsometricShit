using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public enum PlayerState{
		Idle,
		Moving,
		Switching,
		Death
	}

	public enum GravityState{
		Roof,
		Floor
	}
	//State variables
	public PlayerState _playerState;
	public GravityState _gravityState;

	//Tranforms for gravity camera switch
	public Transform roofTransform;
	public Transform floorTransfrom;
	private Transform cameraFocusTransform;

	//Transforms for player gravity stwitch
	public Transform playerOnRoofTransfrom;
	public Transform playerOnFloorTransform;



	//Rotation bitches
	private Quaternion floorCameraRotation;
	private Quaternion roofCameraRotation;

	//Speeds 
	public float cameraRotateSpeed;
	public float playerJumpSpeed;
	public float playerSpeed;

	//Movement Stuff
	private LayerMask _layerMask;
	public LayerMask _layerMaskFloor;
	public LayerMask _layerMaskRoof;
	public float moveSpeed;
	private Vector3 targetPosition;
	private CharacterController controller;
	public Transform character;
	private Transform characterGeometry;

	//Camera
	public Camera cullCamera;

	//Gravity Vector
	private Vector3 g;


	// Use this for initialization
	void Start () {
		cameraFocusTransform = floorTransfrom;
		_playerState = PlayerState.Moving;
		_gravityState = GravityState.Floor;
		characterGeometry = character.FindChild("characterGeometry");
		controller = character.GetComponent<CharacterController>();
		_layerMask = _layerMaskFloor;
		g = new Vector3(0, - 10 , 0);
	}
	
	// Update is called once per frame
	void Update () {

		//Idle state: play idle animation and look at mouse
		if(_playerState == PlayerState.Idle){

			RaycastHit hit;
			if (Physics.Raycast(cullCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, _layerMask)) {
				targetPosition = hit.point;
				Debug.Log(targetPosition);
			}
			characterGeometry.LookAt(targetPosition);

			controller.Move(g);

			//If player presses WSAD
			if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal")!= 0){
				_playerState = PlayerState.Moving;
			}

			//If player presses space switch state
			if(Input.GetKeyDown(KeyCode.Space)){
				_playerState = PlayerState.Switching;

				//Determine the culling mask
				if(_gravityState == GravityState.Floor){
					cullCamera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX")) 
						| (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water"))
							| (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Roof"));
				}
				if(_gravityState == GravityState.Roof){
					cullCamera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX")) 
						| (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water"))
							| (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Floor"));
				}
			}

		}

		if(_playerState == PlayerState.Moving){


			RaycastHit hit;
			if (Physics.Raycast(cullCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000, _layerMask)) {
				targetPosition = hit.point;
				Debug.Log(targetPosition);
			}
			characterGeometry.LookAt(targetPosition);

			Vector3 strafe = -1 * character.forward * Input.GetAxis("Vertical");
			Vector3 forward = -1 * character.right * Input.GetAxis("Horizontal");
			controller.Move(forward * playerSpeed * Time.deltaTime);
			controller.Move(strafe * playerSpeed * Time.deltaTime);
			controller.Move(g);

			transform.position = character.position;
			

			//For switching to idle check velocity
//			if(controller.velocity.magnitude < 0.2){
//				_playerState = PlayerState.Idle;
//			}

			//Spacebar to switch to roof/floor
			if(Input.GetKeyDown(KeyCode.Space)){
				_playerState = PlayerState.Switching;
				
				//Determine the culling mask
				if(_gravityState == GravityState.Floor){
					cullCamera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX")) 
						| (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water"))
							| (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Roof"));
				}
				if(_gravityState == GravityState.Roof){
					cullCamera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX")) 
						| (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water"))
							| (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Floor"));
				}
			}
		}
		//Statw in which player switches gravity
		if(_playerState == PlayerState.Switching){
			if(_gravityState == GravityState.Floor){
				//Rotate camera around and flip the model


				//Move the camera to the roof
				Vector3 finalPosition = new Vector3(transform.position.x, 7.65f, transform.position.z);
				transform.position = Vector3.MoveTowards(transform.position, finalPosition, playerJumpSpeed * Time.deltaTime);

				//Move the player to the roof
				character.position = Vector3.MoveTowards(transform.position, finalPosition, playerJumpSpeed * Time.deltaTime);

				//Rotate the camera to the roof
				transform.rotation = Quaternion.RotateTowards(transform.rotation, playerOnRoofTransfrom.rotation, cameraRotateSpeed * Time.deltaTime);

				//Rotate the player to the roof
				character.rotation = Quaternion.RotateTowards(transform.rotation, playerOnRoofTransfrom.rotation, cameraRotateSpeed * Time.deltaTime);

				_layerMask = _layerMaskRoof;

				//Switch the state
				if(transform.rotation == playerOnRoofTransfrom.rotation && transform.position == finalPosition){
					_gravityState = GravityState.Roof;
					_playerState = PlayerState.Idle;
					g *= -1;
				}


			}
			else if(_gravityState == GravityState.Roof){
				//Rotate camera around and flip the model

				//Move the player to the floor
				Vector3 finalPosition = new Vector3(transform.position.x, 2.65f, transform.position.z);
				transform.position = Vector3.MoveTowards(transform.position, finalPosition, playerJumpSpeed * Time.deltaTime);

				//Move the player to the floor
				character.position = Vector3.MoveTowards(transform.position, finalPosition, playerJumpSpeed * Time.deltaTime);



				//Rotate the camera to the floor
				//cameraFocusTransform.rotation = Quaternion.RotateTowards(cameraFocusTransform.rotation, floorCameraRotation, cameraRotateSpeed * Time.deltaTime);

				//Rotate the camera to th floor
				Quaternion lookRotation = Quaternion.LookRotation(playerOnFloorTransform.forward, playerOnFloorTransform.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, playerOnFloorTransform.rotation, cameraRotateSpeed * Time.deltaTime);

				//Rotate the player to the floor
				character.rotation = Quaternion.RotateTowards(transform.rotation, playerOnFloorTransform.rotation, cameraRotateSpeed * Time.deltaTime);

				_layerMask = _layerMaskFloor;

				//Switch the state
				if(transform.rotation == playerOnFloorTransform.rotation && transform.position == finalPosition){
					_gravityState = GravityState.Floor;
					_playerState = PlayerState.Idle;
					g *= -1;
				}
				
			}

		}

		if(_playerState == PlayerState.Death){

		}
	
	}
}
