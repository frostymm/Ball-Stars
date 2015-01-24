using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

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

	bool m_DebugMode = false;
	public bool isDebugMode
	{
		get{ return m_DebugMode; }
		set{ m_DebugMode = value; }
	}

	private int m_LapsInRace = 3;
	public int GetLapsInRace(){ return m_LapsInRace; }
	public void SetLapsInRace(int laps){ m_LapsInRace = laps; }

	public void RestartLevel()
	{
		ResetRacingVariables();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void LoadRace(string level)
	{
		Application.LoadLevel(level);
	}

	private string m_TexturePath = "C:\\poop.png";
	public string GetTexturePath(){ return m_TexturePath; }
	public void SetTexturePath( string path ){ m_TexturePath = path; }

	private Texture2D m_LoadedImage;
	public Texture2D GetLoadedTexture()
	{
		if(m_LoadedImage == null)
			m_LoadedImage = Resources.Load<Texture2D>("Textures/sanic");

		return m_LoadedImage;
	}
	public void LoadTexture()
	{
		if(File.Exists(m_TexturePath))
		{
			byte[] fileData;
			fileData = File.ReadAllBytes(m_TexturePath);
			GetLoadedTexture().LoadImage(fileData);
		}
	}
	public void LoadTexture(byte[] fileData)
	{
		GetLoadedTexture().LoadImage(fileData);
	}

	private List<float> m_HighScores = new List<float>();
	private int m_NumberOfHighScores = 10;
	public bool SetHighScore(float score)
	{
		m_HighScores.Add(score);
		m_HighScores.Sort();

		if(m_HighScores.Count > m_NumberOfHighScores)
			m_HighScores.RemoveAt(m_HighScores.Count - 1);

		if(m_HighScores.Contains(score))
			return true;
		else
			return false;
	}

	public float[] GetScores()
	{
		return m_HighScores.ToArray();
	}

	public float GetBestScore()
	{
		if(m_HighScores.Count > 0)
			return m_HighScores[0];
		else
			return Mathf.Infinity;
	}

	private void ResetRacingVariables()
	{
		IsRaceStarted = false;
		IsRaceFinished = false;
	}

	public void ExitRace()
	{
		if(isPaused)
			ForceUnPause();
		ResetRacingVariables();
		Save ();

		Application.LoadLevel("MainMenu");
	}
	
	public bool isOnline = false;
	public void OnApplicationQuit()
	{
		Debug.Log("Quitting Application");
		if(isOnline)
			NetworkManager.Instance().OnApplicationQuit();
	}

	public void Save()
	{
		if(m_HighScores.Count > 0)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (Application.persistentDataPath + "/BallStarz.dat");

			SaveData data = new SaveData();
			//Set SaveData
			data.SetVariables();

			bf.Serialize(file, data);
			file.Close();

			if(isDebugMode)
				Debug.Log("Saving File to: " + Application.persistentDataPath + "/BallStarz.dat");
		}
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/BallStarz.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/BallStarz.dat", FileMode.Open);

			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();

			//Set SaveData
			data.RetrieveVariables();

			Debug.Log("Data Loaded");
		}
	}

	public void DeleteSave()
	{
		if(File.Exists(Application.persistentDataPath + "/BallStarz.dat"))
		{
			File.Delete(Application.persistentDataPath + "/BallStarz.dat");
		}

		m_HighScores.Clear();

		Debug.Log("Save Deleted");
	}

	public void Quit()
	{
		if(m_HighScores.Count > 0)
			Save();

		Application.Quit();
	}

	private bool m_Paused = false;
	public bool isPaused
	{
		get{ return m_Paused; }
		set{ m_Paused = value; }
	}

	public void Pause()
	{
		if(isPaused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;

		isPaused = !isPaused;
	}

	private void ForceUnPause()
	{
		Time.timeScale = 1;
		isPaused = false;
	}

	private bool m_RaceFinished = false;
	public bool IsRaceFinished
	{
		get{ return m_RaceFinished; }
		set{ m_RaceFinished = value; }
	}

	private bool m_RaceStarted = false;
	public bool IsRaceStarted
	{
		get{ return m_RaceStarted; }
		set{ m_RaceStarted = value; }
	}

	public void StartRace()
	{
		IsRaceStarted = true;
	}

	public void Start()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	public void Update()
	{
		Debug.Log("DebugMode: " + isDebugMode);
		if(Input.GetKeyDown(KeyCode.BackQuote))
			isDebugMode = !isDebugMode;

		Debug.Log("Checking for back button");
		if(Input.GetKeyDown(KeyCode.Escape))
			Debug.Log("Pause");
	}
}

[Serializable]
class SaveData
{
	public float[] scores;
	
	public SaveData()
	{

	}

	public void SetVariables()
	{
		scores = GameManager.Instance().GetScores();
	}

	public void RetrieveVariables()
	{
		foreach(float score in scores)
		{
			GameManager.Instance().SetHighScore(score);
		}
	}
}
