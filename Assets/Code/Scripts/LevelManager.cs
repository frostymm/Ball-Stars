using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Transform[] spawnPoints;
	public Transform[] wayPoints;
	public Transform GetWayPoint(int i){ return wayPoints[i]; }
	public int GetNumOfWayPoints(){ return wayPoints.Length; }

	private float m_WayPointRadii = 200;
	public float WayPointDistance(){ return m_WayPointRadii; }

	private static LevelManager m_Instance;
	public static LevelManager Instance(){ return m_Instance; }
	
	void Start () {
		LevelManager.m_Instance = this;
	}

	void Update () {
		
	}

	void FixedUpdate()
	{
		if(!GameManager.Instance().IsRaceStarted && !Application.isLoadingLevel)
		{
			GameManager.Instance().StartRace();
		}
	}
}
