using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour 
{
	string currentMenu = "Main";
	Vector2 scrollViewVector = Vector2.zero;
	void OnGUI()
	{
		// Main Menu
		if (currentMenu == "Main")
		{
			GameManager.Instance().isOnline = false;
			GUI.Box(new Rect((Screen.width / 2) - 100, Screen.height / 8, 200, 250), "Main Menu");
			
			if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 4), 100, 50), "Single Player"))
			{		
				//Application.LoadLevel("TestScene");
			}
			if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 4) + 60, 100, 50), "Multi-Player"))
			{				
				currentMenu = "Multi";
			}
			if(GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 4) + 120, 100, 50), "Options"))
			{				

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
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}

