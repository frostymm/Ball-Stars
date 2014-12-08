using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDScript : MonoBehaviour {
	public PlayerEntity player;
	public Text HUDPosition;
	public Text HUDLap;

	public void Respawn()
	{
		player.Respawn();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		HUDPosition.text = "" + player.GetRacePosition();
		HUDLap.text = "Lap: " + (player.GetLapsCompleted() + 1);
	}
}
