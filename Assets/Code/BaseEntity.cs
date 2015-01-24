using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseEntity : MonoBehaviour {
	
	public GameObject ball;

	public Vector3 GetPosition(){ return ball.transform.position; }
	public Rigidbody GetRigidbody(){ return ball.rigidbody; }
	public Vector3 GetVelocity(){ return ball.rigidbody.velocity; }
	public Collider GetCollider(){ return ball.collider; }

	public BallScript GetBallScript(){ return ball.GetComponent<BallScript>(); }
	
	private bool m_IsGrounded = false;
	public bool isGrounded
	{
		get{ return m_IsGrounded; }
		set{ m_IsGrounded = value; }
	}

	private float m_Torque = 320;
	public float GetTorque(){ return m_Torque; }

	private int m_CurrentWayPoint = 0;
	public int GetCurrentWayPoint(){ return m_CurrentWayPoint; }
	public virtual void SetCurrentWayPoint(int waypoint)
	{
		m_LastVisitedWayPoint = LevelManager.Instance().GetWayPointTransform(m_CurrentWayPoint);
		m_CurrentWayPoint = waypoint; 
	}

	Transform m_LastVisitedWayPoint;
	public Transform GetLastVisitedWayPoint(){ return m_LastVisitedWayPoint; }

	public void CheckCurrentWayPoint()
	{
		if(Vector3.Distance(this.transform.position, LevelManager.Instance().GetWayPointTransform(GetCurrentWayPoint()).position) <= LevelManager.Instance().WayPointDistance())
		{
			if(GetCurrentWayPoint() != LevelManager.Instance().GetNumOfWayPoints() -1)
				SetCurrentWayPoint(GetCurrentWayPoint() + 1);
			else
				LapCompleted();
		}
		
		if(!m_LastVisitedWayPoint)
			m_LastVisitedWayPoint = LevelManager.Instance().GetWayPointTransform(LevelManager.Instance().GetNumOfWayPoints() - 1);
	}

	private WayPoint m_CurrentArrowWayPoint;
	public WayPoint GetCurrentArrowWayPoint(){ return m_CurrentArrowWayPoint; }
	public void SetCurrentArrowWayPoint(WayPoint wp){ m_CurrentArrowWayPoint = wp; }

	public void FindNearestArrowWayPoint()
	{
		List<WayPoint> wp = LevelManager.Instance().GetMinorWaypoints(LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()));

		//Default to current waypoint
		float dist = Vector3.Distance(this.transform.position, LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()).transform.position);
		float closestDist = dist;
		WayPoint closestPoint = LevelManager.Instance().GetWayPoint(GetCurrentWayPoint());

		//Check if any arrow waypoints are closer
		for(int i = 0; i < wp.Count; i++)
		{
			dist = Vector3.Distance(this.transform.position, wp[i].transform.position);
			
			if(dist < closestDist)
			{
				closestDist = dist;
				closestPoint = wp[i];
			}
		}

		SetCurrentArrowWayPoint(closestPoint);
	}

	//Find next waypoint
	public void FindNextArrowWayPoint()
	{
		List<WayPoint> wp = LevelManager.Instance().GetMinorWaypoints(LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()));

		int index = wp.IndexOf(GetCurrentArrowWayPoint());

		if(index == wp.Count -1)
		{
			SetCurrentArrowWayPoint(LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()));
		}
		else
			m_CurrentArrowWayPoint = wp[index + 1];
	}

	//reset waypoint on respawn
	public void ResetArrowWayPoint()
	{
		SetCurrentArrowWayPoint(LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()).GetMinorWaypoints()[0]);
	}

	//Update check to find next waypoint
	private float m_TooFarAway = 1000f;
	public void CheckCurrentArrowWayPoint()
	{
		if(GetCurrentArrowWayPoint() == null)
		{
			ResetArrowWayPoint();
		}
		else
		{
			float dist = Vector3.Distance(this.transform.position, GetCurrentArrowWayPoint().transform.position);

			float distToCheck = 0f;
			if(GetCurrentArrowWayPoint().isMainWayPoint)
				distToCheck = LevelManager.Instance().WayPointDistance();
			else
				distToCheck = LevelManager.Instance().MinorWayPointDistance();

			if(dist <= distToCheck)
			{
				FindNextArrowWayPoint();
			}

			if(dist >= m_TooFarAway)
			{
				FindNearestArrowWayPoint();
			}
		}
	}

	private bool m_SetNewRecord = false;
	public bool hasSetNewRecord
	{
		get{ return m_SetNewRecord; }
		set{ m_SetNewRecord = value; }
	}

	private int m_LapsCompleted = 0;
	public int GetLapsCompleted(){ return m_LapsCompleted; }
	public void LapCompleted()
	{
		m_LapsCompleted++;
		SetCurrentWayPoint(0);

		if(m_CurrentLapTime < m_BestLapTime)
		{
			SetBestLapTime(m_CurrentLapTime);
		}

		GameManager.Instance().SetHighScore(m_CurrentLapTime);

		if(GetBestLapTime() <= GameManager.Instance().GetBestScore())
			hasSetNewRecord = true;

		SetLapTime(0f);

		if(m_LapsCompleted == GameManager.Instance().GetLapsInRace())
			GameManager.Instance().IsRaceFinished = true;
	}

	private int m_PositionInRace = 0;
	public int GetRacePosition()
	{
		return m_PositionInRace;
	}

	public void Respawn()
	{
		if(GameManager.Instance().isDebugMode)
		{
			ball.transform.position = LevelManager.Instance().GetWayPointTransform(GetCurrentWayPoint()).position;
			ball.transform.rotation = LevelManager.Instance().GetWayPointTransform(GetCurrentWayPoint()).rotation;
		}
		else
		{
			ball.transform.position = GetLastVisitedWayPoint().position;
			ball.transform.rotation = GetLastVisitedWayPoint().rotation;
			transform.rotation = GetLastVisitedWayPoint().rotation;
		}

		ResetArrowWayPoint();

		ball.rigidbody.velocity = Vector3.zero;
	}

	float jumpTimer = 0f;
	public void Jump()
	{
		if(Time.time >= jumpTimer)
		{
			ContactPoint cp = GetBallScript().pointOfGround;
			ball.rigidbody.AddForce(cp.normal * 30, ForceMode.Impulse);
			jumpTimer += 1f;
		}
	}

	private float m_CurrentLapTime = 0f;
	public float GetLapTime(){ return m_CurrentLapTime; }
	public void SetLapTime(float t){ m_CurrentLapTime = t; }

	public float m_BestLapTime = Mathf.Infinity;
	public float GetBestLapTime(){ return m_BestLapTime; }
	public void SetBestLapTime(float t){ m_BestLapTime = t; }
	
	public virtual void Awake()
	{
	}
	
	public virtual void Start () 
	{
		ball.transform.parent = null;
	}
	
	public virtual void FixedUpdate () {
		if(GameManager.Instance().IsRaceStarted && !GameManager.Instance().IsRaceFinished)
		{
			m_CurrentLapTime += Time.fixedDeltaTime;

			gameObject.transform.position = ball.transform.position;

			CheckCurrentWayPoint();

			CheckCurrentArrowWayPoint();

			ball.rigidbody.AddTorque(gameObject.transform.right * GetTorque());
		}
		else
			ball.rigidbody.velocity = new Vector3(0, -9, 0);

		isGrounded = GetBallScript().isGrounded;
	}

	public virtual void Update()
	{
	}
}

