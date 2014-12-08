using UnityEngine;
using System.Collections;
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
	
	public bool isOnline = false;
	public void OnApplicationQuit()
	{
		Debug.Log("Quitting Application");
		if(isOnline)
			NetworkManager.Instance().OnApplicationQuit();
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/BallStarz.dat");

		SaveData data = new SaveData();
		//Set SaveData
		data.SetVariables();

		bf.Serialize(file, data);
		file.Close();
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
		}
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
	}
}

[Serializable]
class SaveData
{
	public byte[] loadedTexture;
	public string texturePath;
	
	public SaveData()
	{

	}

	public void SetVariables()
	{
		loadedTexture = GameManager.Instance().GetLoadedTexture().EncodeToPNG();
		texturePath = GameManager.Instance().GetTexturePath();
	}

	public void RetrieveVariables()
	{
		GameManager.Instance().LoadTexture(loadedTexture);
		GameManager.Instance().SetTexturePath(texturePath);
	}
}
