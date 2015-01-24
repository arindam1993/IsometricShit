using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

	public float bulletSpeed;

	private float currentSpeed;

	// Use this for initialization
	void Start () {
		firBullet();
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * currentSpeed * Time.deltaTime;	
	}

	public void firBullet(){
		currentSpeed = bulletSpeed;
	}
}
