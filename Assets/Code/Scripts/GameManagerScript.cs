using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	public float[] Scores;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameManager.Instance().Update();
	}

	void FixedUpdate()
	{
		Scores = GameManager.Instance().GetScores();
	}

	void OnApplicationQuit()
	{
		GameManager.Instance().OnApplicationQuit();
	}
}
