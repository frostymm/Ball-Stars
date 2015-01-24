using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageSelectGuiScript : MonoBehaviour {
	public Text bestScore;

	// Use this for initialization
	void Start () {
		string time;

		if(GameManager.Instance().GetBestScore() != Mathf.Infinity)
			time = BasicUtils.GetTimeString(GameManager.Instance().GetBestScore());
		else
			time = "No Time set";

		bestScore.text = "Best Score \n" + time;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
