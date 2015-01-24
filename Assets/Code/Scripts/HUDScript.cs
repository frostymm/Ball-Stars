using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDScript : MonoBehaviour {
	public PlayerEntity player;
	public Text HUDPosition;
	public Text HUDLap;
	public Text HUDRecord;
	public Text HUDTime;

	public GameObject MainPanel;

	public GameObject ReadyGoPanel;
	public Text HUDReadyGo;

	public GameObject arrow;
	public GameObject arrowPointingAt;

	public GameObject PauseMenuPanel;

	public void PlayAgain()
	{
		GameManager.Instance().RestartLevel();
	}

	public void Pause()
	{
		if(!GameManager.Instance().isPaused)
		{
			if(!PauseMenuPanel.activeSelf)
				PauseMenuPanel.SetActive(true);
		}
		else
		{
			if(PauseMenuPanel.activeSelf)
				PauseMenuPanel.SetActive(false);
		}

		GameManager.Instance().Pause();
	}

	public void QuitToMenu()
	{
		GameManager.Instance().ExitRace();
	}

	public void Quit()
	{
		GameManager.Instance().Quit();
	}

	public void Respawn()
	{
		player.Respawn();
	}

	float goTimer = 0f;
	public string GetReadyGoText()
	{
		float t = LevelManager.Instance().GetTimeTillStart();

		if(t > 1)
			return "Ready?";
		if(t > 0)
			return "Set?";
		else
		{
			goTimer = Time.time + 1f;
			return "Go!";
		}
	}

	public Text conclusionText;
	public GameObject finishedPanel;
	void LoadFinishedMenu()
	{
		if(MainPanel.activeSelf)
			MainPanel.SetActive(false);

		if(!finishedPanel.activeSelf)
			finishedPanel.SetActive(true);

		string conclusion;
		if(player.hasSetNewRecord)
		{
			conclusion = "Congratulations! \n" 
				+ "Your new record was: \n" 
					+ BasicUtils.GetTimeString(player.GetBestLapTime());
		}
		else
			conclusion = "Sorry, no new record was set.";

		if(conclusionText.text != conclusion)
			conclusionText.text = conclusion;
	}

	void FixedUpdate()
	{
		if(!GameManager.Instance().IsRaceFinished)
		{
			HUDPosition.text = "Place: " + player.GetRacePosition();
			HUDLap.text = "Lap: " + (player.GetLapsCompleted() + 1);
			if(GameManager.Instance().GetBestScore() == Mathf.Infinity)
				HUDRecord.text = "Record: " + "N/A";
			else
				HUDRecord.text = "Record: " + BasicUtils.GetTimeString(GameManager.Instance().GetBestScore());
			HUDTime.text = "Time: " + BasicUtils.GetTimeString(player.GetLapTime());
			
			if(player.GetCurrentArrowWayPoint() != null)
			{
				arrow.transform.LookAt(player.GetCurrentArrowWayPoint().transform.position);
				arrowPointingAt = player.GetCurrentArrowWayPoint().gameObject;
			}
			
			if(LevelManager.Instance().GetTimeTillStart() > 0)
				HUDReadyGo.text = GetReadyGoText();
			else if(ReadyGoPanel.activeSelf)
			{
				if(goTimer == 0f)
					HUDReadyGo.text = GetReadyGoText();
				if(Time.time > goTimer)
					ReadyGoPanel.SetActive(false);
			}
		}
		else
		{
			HUDLap.text = "Lap: " + GameManager.Instance().GetLapsInRace();

			LoadFinishedMenu();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
