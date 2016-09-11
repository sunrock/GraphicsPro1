using UnityEngine;
using System.Collections;

public class CameraMoving : MonoBehaviour {

	public float speed = 100.0f; // Default speed sensitivity


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float yaw = Input.GetAxis ("Mouse X");
		float pitch = Input.GetAxis ("Mouse Y");

		transform.eulerAngles += new Vector3 (-pitch, yaw, 0.0f);

		// move right
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			//transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
			transform.position += transform.rotation * Vector3.right * speed *Time.deltaTime;
		}

		// move left
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			//transform.Translate(new Vector3(-speed * Time.deltaTime,0,0));
			transform.position += transform.rotation * Vector3.left * speed *Time.deltaTime;
		}

		// move backward
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			//transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));
			transform.position += transform.rotation * Vector3.back * speed *Time.deltaTime;
		}

		// move forward
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			//transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
			transform.position += transform.rotation * Vector3.forward * speed *Time.deltaTime;
		}
	
	}
}
