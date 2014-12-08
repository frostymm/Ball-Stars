using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		GameManager.Instance().Update();
	}

	void OnApplicationQuit()
	{
		GameManager.Instance().OnApplicationQuit();
	}
}
