﻿using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnApplicationQuit()
	{
		GameManager.Instance().OnApplicationQuit();
	}
}
