using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public bool isGrounded = false;
	public ContactPoint pointOfGround;

	void OnCollisionEnter(Collision col)
	{

	}

	void OnCollisionStay(Collision col)
	{
		bool grounded = false;
		foreach(ContactPoint contact in col.contacts)
		{
			if(contact.otherCollider.tag == "Terrain")
			{
				grounded = true;
				pointOfGround = contact;
			}
		}
		isGrounded = grounded;
	}

	void OnCollisionExit(Collision col)
	{
		if(col.collider.tag == "Terrain")
			isGrounded = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
