using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager
{
	public NetworkManager()
	{
	}
	
	private static NetworkManager m_Instance = null;
	public static NetworkManager Instance()
	{
		if (m_Instance == null) 
		{
			m_Instance = new NetworkManager();
		}
		
		return m_Instance;
	}

	public void OnApplicationQuit()
	{
		Network.Disconnect();
		MasterServer.UnregisterHost();
	}

	private NetworkView m_NetView;
	public NetworkView GetNetView()
	{
		return m_NetView;
	}
	public void SetNetView(NetworkView NetView)
	{
		m_NetView = NetView;
	}

	/*public void StartServer()
	{
		NetworkConnectionError error =
			Network.InitializeServer(32, 25002, !Network.HavePublicAddress());
		MasterServer.RegisterHost("BallStars", "Niggas Game", "some niggas playin bball");
	}*/

	static int listenPort = 25000;
	static int numConnections = 32;
	static bool useNat = !Network.HavePublicAddress();
	static int listenLow = 25000, listenhigh = 26000;
	static int startTries = 5;
	public bool StartServer() 
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
			NetworkConnectionError error = Network.InitializeServer(numConnections, listenPort, useNat);
			if (error == NetworkConnectionError.NoError) {
				//SetupPlayer();
				//me.networkView.RPC("SetHost", RPCMode.AllBuffered, Network.player);
				Debug.Log("Server Success");
				MasterServer.RegisterHost("BallStars", "bball time", "Stuff and things");
				return true;
			} if (startTries-- > 0) {
				listenPort = UnityEngine.Random.Range(listenLow, listenhigh);
				return StartServer();
			} else {
				//UDPConnection.error += error.ToString();
				return false;
			}
		} else
			Debug.LogWarning("no nets!");
		return false;
	}
	
	public HostData[] GetHostList()
	{
		MasterServer.RequestHostList("BallStars");
		HostData[] data = MasterServer.PollHostList();
		return data;
	}

	public void JoinRoom(HostData room)
	{
		Network.Connect(room);
	}

	public void LeaveRoom()
	{
		Network.Disconnect();
	}

	public Player[] GetPlayerList()
	{
		List<Player> players = new List<Player>();

		players.Add(GetMyPlayer()); //Add client player to list

		foreach(NetworkPlayer p in Network.connections)
		{
			players.Add(new Player(p.ipAddress));
		}

		return players.ToArray();
	}

	private Player m_Player;
	public Player GetMyPlayer()
	{
		if (m_Player == null) 
		{
			m_Player = new Player("Poonigga");
		}
		
		return m_Player; 
	}

	public void StartGame()
	{
		Debug.Log ("Starting Game");
	}

	public void CallStartGameOnAllClients()
	{
		GetNetView().RPC("StartGame", RPCMode.All);
	}

	public void OnFailedToConnect()
	{
		Application.LoadLevel("MainMenu");
	}

	public void OnDisconnectedFromServer()
	{
		Application.LoadLevel("MainMenu");
	}

}

public class Player
{
	public string Name = "Poop";
	public Player(string name)
	{
		Name = name;
	}

}
