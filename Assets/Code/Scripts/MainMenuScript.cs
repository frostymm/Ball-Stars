using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
	public GameObject OptionsPanel;
	public GameObject MainPanel;

	public void SinglePlayer()
	{
		GameManager.Instance().LoadRace("GreenHillZoneBasic");
	}

	public void SetTexturePath(string textPath)
	{
		GameManager.Instance().SetTexturePath(textPath);
	}

	public void LoadSave()
	{
		GameManager.Instance().Load();
		LoadOptions();
	}

	public void Save()
	{
		GameManager.Instance().Save();
	}

	public void CloseMenus()
	{
		if(OptionsPanel.activeSelf)
			OptionsPanel.SetActive(false);
		if(MainPanel.activeSelf)
			MainPanel.SetActive(false);
	}

	public void LoadMain()
	{
		CloseMenus();

		if(!MainPanel.activeSelf)
			MainPanel.SetActive(true);
	}

	public void LoadOptions()
	{
		CloseMenus();

		if(!OptionsPanel.activeSelf)
			OptionsPanel.SetActive(true);

		InitializeOptions();
	}

	public void LoadImage()
	{
		GameManager.Instance().LoadTexture();
		InitializeOptions();
	}

	public RawImage PreviewImage;
	public void InitializeOptions()
	{
		PreviewImage.texture = GameManager.Instance().GetLoadedTexture();
	}

	string currentMenu = "Main";
	Vector2 scrollViewVector = Vector2.zero;
	/*void OnGUI()
	{
		// Main Menu
		if (currentMenu == "Main")
		{
			GameManager.Instance().isOnline = false;
			GUI.Box(new Rect((Screen.width / 2) - 100, Screen.height / 8, 200, 250), "Main Menu");
			
			if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 4), 100, 50), "Single Player"))
			{		
				GameManager.Instance().LoadRace("GreenHillZoneBasic");
			}
			if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 4) + 60, 100, 50), "Multi-Player"))
			{				
				currentMenu = "Multi";
			}
			if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 4) + 120, 100, 50), "Options"))
			{				
				currentMenu = "Options";
			}
		}
		// Multiplayer ServerList Menu
		else if ((currentMenu == "Multi"))
		{
			GameManager.Instance().isOnline = true;
			//GameManager.Instance().IsOnline = true;
			GUILayout.BeginArea(new Rect((Screen.width / 2) - Screen.width * 0.3f, Screen.height / 8 - 20, Screen.width * 0.6f, Screen.height * 0.6f));
			GUILayout.Box("Multiplayer", GUILayout.Height(Screen.height));
			//NetworkManager.Instance();

			if(GUI.Button(new Rect(0, 0, 90, 30), "Create Server"))
			{				
				NetworkManager.Instance().StartServer();
				currentMenu = "InRoom";
			}


			GUILayout.BeginArea(new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.9f, Screen.height * 0.9f));

			if(NetworkManager.Instance().GetHostList().Length != 0)
			{
				scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
				foreach(HostData room in NetworkManager.Instance().GetHostList())//for (int i = 0; i < NetworkManager.Instance().GetHostList().Length; i++)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(room.gameName, GUILayout.Width(250));
					//GUILayout.FlexibleSpace();
					if(GUILayout.Button("Join", GUILayout.Width(50)))
					{
						NetworkManager.Instance().JoinRoom(room);
						currentMenu = "InRoom";
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
			}
			else
				GUILayout.Label("No Hosts Found");
			GUILayout.EndArea();
			
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(Screen.width * 0.5f - 150,0,300,100));
			GUILayout.BeginHorizontal();
			GUILayout.Label("Your Name: ", GUILayout.Width (100));
			NetworkManager.Instance().GetMyPlayer().Name = GUILayout.TextField(NetworkManager.Instance().GetMyPlayer().Name);
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		// Multiplayer In Room Menu
		else if(currentMenu == "InRoom")
		{
			GUILayout.BeginArea(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.2f, Screen.height *0.5f));
			if (GUILayout.Button("Leave"))
			{
				NetworkManager.Instance().LeaveRoom();
				currentMenu = "Multi";
			}

			foreach(Player player in NetworkManager.Instance().GetNetPlayerList())
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(player.Name, GUILayout.Width(250));

				if (Network.isServer && player != NetworkManager.Instance().GetMyPlayer())
					if(GUILayout.Button("Kick", GUILayout.Width(50)))
						Debug.Log("Can't kick a nigga yet");
				GUILayout.EndHorizontal();
			}

			if (Network.isServer)
			{
				if(GUILayout.Button("Start Game"))
				{
					NetworkManager.Instance().CallStartGameOnAllClients();
				}
			}

			GUILayout.EndArea();

		}
		//In options menu
		else if(currentMenu == "Options")
		{
			if(GUI.Button(new Rect(0,0, 100,100), "Load"))
				GameManager.Instance().Load();

			if(GUI.Button(new Rect(0,100, 100,100), "Save"))
				GameManager.Instance().Save();

			GUILayout.BeginArea(new Rect((Screen.width / 2) - Screen.width * 0.4f, Screen.height / 8 - 20, Screen.width * 0.8f, Screen.height * 0.8f));
			GUI.Box(new Rect(0,0, Screen.width * 0.8f, Screen.height * 0.8f), "Options");

			GUILayout.BeginArea(new Rect(0, 30, Screen.width * 0.8f, Screen.height * 0.7f));

			GUILayout.Label ("Player Image to Load");
			GUILayout.BeginHorizontal();
			GameManager.Instance().SetTexturePath(GUILayout.TextField(GameManager.Instance().GetTexturePath(), GUILayout.Width(300)));
			//GUILayout.Button("Browse...");
			GUILayout.EndHorizontal();

			if(GUILayout.Button("Load"))
			{
				GameManager.Instance().LoadTexture();
			}

			GUILayout.Box(GameManager.Instance().GetLoadedTexture());

			GUILayout.EndArea();
			GUILayout.EndArea();
		}
	}*/

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}

