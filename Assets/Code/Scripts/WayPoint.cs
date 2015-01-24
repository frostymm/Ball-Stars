using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour {

	public bool isMainWayPoint = false;
	public bool isCurrentWaypoint = false;

	private Material hiddenMat;
	private Material activeMat;

	private List<WayPoint> m_MinorWaypoints = new List<WayPoint>();
	public List<WayPoint> GetMinorWaypoints()
	{
		return m_MinorWaypoints;
	}

	// Use this for initialization
	void Start () {
		if(isMainWayPoint)
		{
			hiddenMat = Resources.Load<Material>("SanicBall/Materials/Environment/checkpointHidden");
			activeMat = Resources.Load<Material>("SanicBall/Materials/Environment/checkpointActive");

			GetComponentsInChildren<WayPoint>(m_MinorWaypoints);

			if(m_MinorWaypoints.Contains(this))
				m_MinorWaypoints.Remove(this);
		}
		else
		{
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isMainWayPoint)
		{
			if(isCurrentWaypoint)
			{
				if(renderer.material != activeMat)
					renderer.material = activeMat;
			}
			else
			{
				if(renderer.material != hiddenMat)
					renderer.material = hiddenMat;
			}
		}
		else
		{
			if(GameManager.Instance().isDebugMode)
			{
				if(!renderer.enabled)
					renderer.enabled = true;

				if(isCurrentWaypoint)
				{
					if(renderer.material.color != Color.green)
						renderer.material.color = Color.green;
				}
				else
				{
					if(renderer.material.color != Color.gray)
						renderer.material.color = Color.gray;
				}
			}
			else
			{
				if(renderer.enabled)
					renderer.enabled = false;
			}
		}
	}
}
