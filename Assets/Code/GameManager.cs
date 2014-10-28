using UnityEngine;
using System.Collections;

public class GameManager
{
	public GameManager()
	{
	}
	
	private static GameManager m_Instance = null;
	public static GameManager Instance()
	{
		if (m_Instance == null) 
		{
			m_Instance = new GameManager();
		}
		
		return m_Instance;
	}

	public bool isOnline = false;

	public void OnApplicationQuit()
	{
		Debug.Log("Quitting Application");
		if(isOnline)
			NetworkManager.Instance().OnApplicationQuit();
	}
}
