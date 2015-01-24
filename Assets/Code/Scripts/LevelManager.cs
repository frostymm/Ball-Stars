using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public Transform[] spawnPoints;
	public WayPoint[] wayPoints;
	public WayPoint GetWayPoint(int i){ return wayPoints[i]; }
	public Transform GetWayPointTransform(int i){ return wayPoints[i].transform; }
	public int GetNumOfWayPoints(){ return wayPoints.Length; }

	public List<WayPoint> GetMinorWaypoints(WayPoint wp)
	{
		return wp.GetMinorWaypoints();
	}

	private float m_WayPointRadii = 200;
	public float WayPointDistance(){ return m_WayPointRadii; }

	private float m_MinorWayPointRadii = 300;
	public float MinorWayPointDistance(){ return m_MinorWayPointRadii; }

	private static LevelManager m_Instance;
	public static LevelManager Instance(){ return m_Instance; }
	
	void Start () {
		LevelManager.m_Instance = this;
	}

	void Update () {
		
	}

	private float m_TimeTillStart = 2f;
	public float GetTimeTillStart(){ return m_TimeTillStart; }

	void FixedUpdate()
	{
		if(!GameManager.Instance().IsRaceStarted && !Application.isLoadingLevel)
		{
			m_TimeTillStart -= Time.fixedDeltaTime;

			if(m_TimeTillStart <= 0)
			{
				GameManager.Instance().StartRace();
			}
		}
	}
}
