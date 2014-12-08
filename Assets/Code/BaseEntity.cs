using UnityEngine;
using System.Collections;

public abstract class BaseEntity : MonoBehaviour {
	
	public GameObject ball;
	public WheelCollider wheel;

	private float m_Torque = 300;
	public float GetTorque(){ return m_Torque; }

	private int m_CurrentWayPoint = 0;
	public int GetCurrentWayPoint(){ return m_CurrentWayPoint; }
	public virtual void SetCurrentWayPoint(int waypoint)
	{
		m_LastVisitedWayPoint = LevelManager.Instance().GetWayPoint(m_CurrentWayPoint);
		m_CurrentWayPoint = waypoint; 
	}

	Transform m_LastVisitedWayPoint;
	public Transform GetLastVisitedWayPoint(){ return m_LastVisitedWayPoint; }

	private int m_LapsCompleted = 0;
	public int GetLapsCompleted(){ return m_LapsCompleted; }
	public void LapCompleted()
	{
		m_LapsCompleted++;
		SetCurrentWayPoint(0);
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
			transform.position = LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()).position;
			transform.rotation = LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()).rotation;
		}
		else
		{
			transform.position = GetLastVisitedWayPoint().position;
			transform.rotation = GetLastVisitedWayPoint().rotation;
		}

		rigidbody.velocity = Vector3.zero;
		wheel.brakeTorque = Mathf.Infinity;
	}
	
	public virtual void Awake()
	{
	}
	
	public virtual void Start () 
	{
		ball.renderer.material.SetTexture(0, GameManager.Instance().GetLoadedTexture());
	}
	
	public virtual void FixedUpdate () {
		if(GameManager.Instance().IsRaceStarted)
		{
			if(wheel.motorTorque != GetTorque())
				wheel.motorTorque = GetTorque();

			if(Vector3.Distance(this.transform.position, LevelManager.Instance().GetWayPoint(GetCurrentWayPoint()).position) <= LevelManager.Instance().WayPointDistance())
			{
				if(GetCurrentWayPoint() != LevelManager.Instance().GetNumOfWayPoints() -1)
					SetCurrentWayPoint(GetCurrentWayPoint() + 1);
				else
					LapCompleted();
			}

			if(!m_LastVisitedWayPoint)
				m_LastVisitedWayPoint = LevelManager.Instance().GetWayPoint(LevelManager.Instance().GetNumOfWayPoints() - 1);

			if(wheel.brakeTorque > -500)
				wheel.brakeTorque = 0;
		}

		ball.transform.Rotate((wheel.rpm *0.01f),0.0f,0.0f);
	}
}

