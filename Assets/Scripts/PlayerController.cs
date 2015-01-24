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

	//Movement Stuff
	public LayerMask _layerMask;
	public float moveSpeed;
	private Vector3 targetPosition;
	private CharacterController con;

	//Camera
	public Camera cullCamera;


	// Use this for initialization
	void Start () {
		floorTransfrom = transform.FindChild("cameraFocus").transform;
		cameraFocusTransform = floorTransfrom;
		roofTransform = transform.FindChild("rotated").transform;
		floorCameraRotation = floorTransfrom.rotation;
		roofCameraRotation = roofTransform.rotation;
		_playerState = PlayerState.Idle;
		_gravityState = GravityState.Floor;

		//playerOnFloorTransform = transform;

		con = GetComponent<CharacterController>();
	
	}
	
	// Update is called once per frame
	void Update () {

		//Idle state: play idle animation
		if(_playerState == PlayerState.Idle){


			//If player presses WSAD
			if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal")!= 0){
				_playerState = PlayerState.Moving;
			}

			//If player presses space switch state
			if(Input.GetKeyDown(KeyCode.Space)){
//				if(_gravityState == GravityState.Floor){
//					_gravityState = GravityState.Roof;
//					_playerState = PlayerState.Switching;
//				}
//				if(_gravityState == GravityState.Roof){
//					_gravityState = GravityState.Floor;
//					_playerState = PlayerState.Switching;
//				}
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


			//For switching to idle check velocity
			if(con.velocity.magnitude < 0.2){
				_playerState = PlayerState.Idle;
			}

			//Spacebar to switch to roof/floor
			if(Input.GetKeyDown(KeyCode.Space)){
				if(_gravityState == GravityState.Floor){
					_gravityState = GravityState.Roof;
					_playerState = PlayerState.Switching;
				}
				if(_gravityState == GravityState.Roof){
					_gravityState = GravityState.Floor;
					_playerState = PlayerState.Switching;
				}
			}
		}
		//Statw in which player switches gravity
		if(_playerState == PlayerState.Switching){
			if(_gravityState == GravityState.Floor){
				//Rotate camera around and flip the model

				//Move the player to the roof
				transform.position = Vector3.MoveTowards(transform.position, playerOnRoofTransfrom.position, playerJumpSpeed * Time.deltaTime);



				//Rotate the camera to the roof
				//cameraFocusTransform.rotation = Quaternion.RotateTowards(cameraFocusTransform.rotation, roofCameraRotation, cameraRotateSpeed * Time.deltaTime);

				//Rotate the player to the roof
				Quaternion lookRotation = Quaternion.LookRotation(playerOnRoofTransfrom.forward, playerOnRoofTransfrom.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, playerOnRoofTransfrom.rotation, cameraRotateSpeed * Time.deltaTime);

				//Switch the state
				if(transform.rotation == playerOnRoofTransfrom.rotation && transform.position == playerOnRoofTransfrom.position){
					_gravityState = GravityState.Roof;
					_playerState = PlayerState.Idle;
				}


			}
			else if(_gravityState == GravityState.Roof){
				//Rotate camera around and flip the model

				//Move the player to the floor
				transform.position = Vector3.MoveTowards(transform.position, playerOnFloorTransform.position, playerJumpSpeed * Time.deltaTime);
				



				//Rotate the camera to the floor
				//cameraFocusTransform.rotation = Quaternion.RotateTowards(cameraFocusTransform.rotation, floorCameraRotation, cameraRotateSpeed * Time.deltaTime);

				//Rotate the player to th floor
				Quaternion lookRotation = Quaternion.LookRotation(playerOnFloorTransform.forward, playerOnFloorTransform.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, playerOnFloorTransform.rotation, cameraRotateSpeed * Time.deltaTime);

				//Switch the state
				if(transform.rotation == playerOnFloorTransform.rotation && transform.position == playerOnFloorTransform.position){
					_gravityState = GravityState.Floor;
					_playerState = PlayerState.Idle;
				}
				
			}

		}

		if(_playerState == PlayerState.Death){

		}
	
	}
}
