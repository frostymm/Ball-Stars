using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerEntity : BaseEntity {

	public Camera m_Camera;
	public Camera GetCamera(){ return m_Camera; }
	public void SetCamera(Camera cam){ m_Camera = cam;}
	public Transform m_CamParent;
	private Vector3 m_CamFollowOffset = new Vector3(0,0, -3);

	RaycastHit[] m_CamHit = new RaycastHit[6];
	public void CameraUpdate()
	{
		Transform Cam = m_Camera.transform;
		Vector3 Origin = m_Camera.transform.position;
		Vector3 LookAtOffset = new Vector3(0, 1.5f, 0);

		m_CamFollowOffset = (-m_CamParent.forward*(GetVelocity().magnitude/15));

		float terrainHeight = Terrain.activeTerrain.SampleHeight(Cam.position) + Terrain.activeTerrain.GetPosition().y;
		float TerrainOffset = 5f;
		if(m_CamParent.position.y > terrainHeight && ((Mathf.Abs(terrainHeight - GetPosition().y) > 10f) || !isGrounded))
			terrainHeight = m_CamParent.position.y;
		else
			terrainHeight += TerrainOffset;

		float camHSpeed = 150f;
		float camVSpeed = 0.1f;

		float dist = Mathf.Abs(Cam.position.y - m_CamParent.position.y);
		camVSpeed = dist * 5;

		dist = Vector3.Distance(new Vector3(Cam.position.x, 0, Cam.position.z), 
		                        new Vector3(m_CamParent.position.x, 0, m_CamParent.position.z));
		camHSpeed = dist * 8;

		Vector3 newVPos = Vector3.MoveTowards(Cam.position, new Vector3(Cam.position.x, terrainHeight, Cam.position.z), camVSpeed * Time.deltaTime);
		Vector3 newHPos = Vector3.MoveTowards(Cam.position, new Vector3(m_CamParent.position.x, Cam.position.y, m_CamParent.position.z), camHSpeed * Time.deltaTime);
		Vector3 newPos = new Vector3(newHPos.x, newVPos.y, newHPos.z);

		RaycastHit rayHit = new RaycastHit();
		Debug.DrawLine(Cam.position, Cam.position + (new Vector3(newPos.x, Cam.position.y, newPos.z) - Cam.position)*15);
		if(Physics.Linecast(Cam.position, Cam.position + (new Vector3(newPos.x, Cam.position.y, newPos.z) - Cam.position)*15, out rayHit))
		{
			Debug.Log("newPos requires going through terrain");
			newPos.y = Mathf.MoveTowards(newPos.y, rayHit.point.y + 100f, 60*Time.deltaTime);
		}

		Cam.position = newPos;
		Cam.LookAt(transform.position + LookAtOffset);
	}
	public enum CamHits
	{
		up,
		down,
		left,
		right,
		forward,
		back
	}

	public override void SetCurrentWayPoint(int waypoint)
	{
		LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()).isCurrentWaypoint = false;
		LevelManager.Instance().GetWayPoint(waypoint).isCurrentWaypoint = true;

		base.SetCurrentWayPoint(waypoint);
	}


	//Doesn't work (Trying to check if pushing button instead of tapping to jump, jumps regardless right now)
	public bool GetIsTouchingButton()
	{
		//EventSystem eventSystem = EventSystem.current;

		//if(eventSystem && eventSystem.IsPointerOverGameObject())
			//if(eventSystem. == "Button")
				//Debug.Log("TouchingButton");

		return false;
	}

	public override void Awake()
	{
		base.Awake();
	}
	
	public override void Start () {
		base.Start();

		m_Camera.transform.SetParent(null);
	}

	private float turnSpeed = 2.4f;
	public override void FixedUpdate () {
		base.FixedUpdate();

		if(GameManager.Instance().IsRaceStarted)
		{
			float moveH = (Input.GetAxis ("Horizontal") + Input.acceleration.x) * turnSpeed;

			if(isGrounded && (Input.GetButton("Jump") 
			                  || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && !GetIsTouchingButton()))
			{
				Jump();
			}

			transform.Rotate(0.0f,moveH,0.0f);
		}

		CameraUpdate();
	}

	public override void Update()
	{
		base.Update ();
	}
}
