using UnityEngine;
using System.Collections;

public class MoveAround2 : MonoBehaviour {

	// Use this for initialization

	public LayerMask layerMask;

	public float speed = 10.0F;
    public float rotateSpeed = 250;
    // public float lerpSpeed = 0.01; 

    private Vector3 targetPosition;
    public GameObject other;


	void Start () {
		targetPosition = transform.position;
	}
	
	void Update () {
        CharacterController controller = GetComponent<CharacterController>();
		RaycastHit hit;
        if (Physics.Raycast(other.camera.ScreenPointToRay(Input.mousePosition), out hit, 1000, layerMask)) {
       		targetPosition = hit.point;
        }

        Vector3 yChanging = new Vector3(targetPosition.x,this.transform.position.y,targetPosition.z);
        this.transform.LookAt(yChanging);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 sidetoside = transform.TransformDirection(Vector3.right);

        float curSpeed = speed * Input.GetAxis("Vertical");
        float strafe = speed * Input.GetAxis("Horizontal");

        controller.SimpleMove(curSpeed * forward);
        controller.SimpleMove(strafe * sidetoside);
	}

}
