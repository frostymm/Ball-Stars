using UnityEngine;
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
		float Distance = 8f;
		float CheckingDistance = 6f;
		Vector3 LookAtOffset = new Vector3(0, 1.5f, 0);

		m_CamFollowOffset = (-m_CamParent.forward*(rigidbody.velocity.magnitude/15));
		/*
		Vector3 DirectionToDefaultPoint = (m_CamParent.position + m_CamFollowOffset) - Cam.position;
		if(DirectionToDefaultPoint.magnitude <= 2.7)
			DirectionToDefaultPoint = Vector3.zero;
		else
			DirectionToDefaultPoint = DirectionToDefaultPoint / DirectionToDefaultPoint.magnitude;

		Vector3 Translation = Vector3.zero;
		Vector3 Direction = Vector3.zero;
		Direction = m_CamParent.forward;
		Debug.DrawLine(Origin, Origin + (Direction * Distance), Color.blue);
		if(Physics.Linecast(Origin, Origin + (Direction * Distance), out m_CamHit[(int)CamHits.forward]))
		{
			if(m_CamHit[(int)CamHits.forward].distance < CheckingDistance)
				Translation += -Direction;
			DirectionToDefaultPoint.z = 0;
			//Debug.Log("Hitting Forward Line");
		}
		Direction = -m_CamParent.forward;
		Debug.DrawLine(Origin, Origin + (Direction * Distance), Color.blue);
		if(Physics.Linecast(Origin, Origin + (Direction * Distance), out m_CamHit[(int)CamHits.back]))
		{
			if(m_CamHit[(int)CamHits.back].distance < CheckingDistance)
				Translation += -Direction;
			DirectionToDefaultPoint.z = 0;
			//Debug.Log("Hitting Backward Line");
		}
		Direction = m_CamParent.up;
		Debug.DrawLine(Origin, Origin + (Direction * Distance), Color.blue);
		if(Physics.Linecast(Origin, Origin + (Direction * Distance), out m_CamHit[(int)CamHits.up]))
		{
			if(m_CamHit[(int)CamHits.up].distance < CheckingDistance)
				Translation += -Direction;
			DirectionToDefaultPoint.y = 0;
			//Debug.Log("Hitting Up Line");
		}
		Direction = -m_CamParent.up;
		Debug.DrawLine(Origin, Origin + (Direction * Distance), Color.blue);
		if(Physics.Linecast(Origin, Origin + (Direction * Distance), out m_CamHit[(int)CamHits.down]))
		{
			if(m_CamHit[(int)CamHits.down].distance < CheckingDistance)
				Translation += -Direction;
			DirectionToDefaultPoint.y = 0;
			//Debug.Log("Hitting Down Line");
		}

		DirectionToDefaultPoint.x = 0;
		Translation.x = 0;
		*/

		//Cam.Translate((Translation + (DirectionToDefaultPoint*1f)) * 5f * Time.deltaTime);

		float terrainHeight = Terrain.activeTerrain.SampleHeight(Cam.position) + Terrain.activeTerrain.GetPosition().y;
		float TerrainOffset = 5f;
		if(m_CamParent.position.y > terrainHeight && ((Mathf.Abs(terrainHeight - ball.transform.position.y) > 10f) || !wheel.isGrounded))
			terrainHeight = m_CamParent.position.y;
		else
			terrainHeight += TerrainOffset;

		float camSpeed = 0.1f;
		if(Cam.position.y > terrainHeight)
			camSpeed = 5f;
		else
			camSpeed = 15f;


		Cam.position = Vector3.MoveTowards(Cam.position, new Vector3(Cam.position.x, terrainHeight, Cam.position.z), camSpeed * Time.deltaTime);

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
		LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()).renderer.material = 
			Resources.Load<Material>("SanicBall/Materials/Environment/checkpointHidden");
		LevelManager.Instance().GetWayPoint(waypoint).renderer.material = 
			Resources.Load<Material>("SanicBall/Materials/Environment/checkpointActive");

		base.SetCurrentWayPoint(waypoint);
	}

	public override void Awake()
	{
		base.Awake();
	}
	
	public override void Start () {
		base.Start();
	}

	public override void FixedUpdate () {
		base.FixedUpdate();

		float moveH;
		moveH = (Input.GetAxis ("Horizontal") + Input.acceleration.x) * 3f;
		 
		//float moveV = Input.GetAxis ("Vertical");

		transform.Rotate(0.0f,moveH,0.0f);

		CameraUpdate();
	}
}
