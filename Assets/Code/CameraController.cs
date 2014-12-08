using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
	}
	void FixedUpdate () {
		float moveH = Input.GetAxis ("Horizontal");
		transform.LookAt (player.transform);
		Vector3 movement = new Vector3 ( 0.0f,moveH, 0.0f);
		//transform.Rotate (movement);
		transform.Translate (movement);
		
	}
	// Update is called once per frame
	void LateUpdate () {
		//transform.position = player.transform.position + offset;
		transform.rotation = player.transform.rotation;
	}
}
