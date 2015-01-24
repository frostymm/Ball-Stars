using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//----------------------------------Not used currently--------------------------
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
		if(Network.isServer || Network.isClient)
		{
			LeaveRoom();
			MasterServer.UnregisterHost();
		}
	}

	private NetworkView m_NetView;
	public NetworkView GetNetView(){ return m_NetView; }
	public void SetNetView(NetworkView NetView){ m_NetView = NetView; }

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
		GetNetView().RPC("RemovePlayer", RPCMode.All, GetMyPlayer().IP, GetMyPlayer().Name);
		Network.Disconnect();
	}

	List<Player> m_NetworkPlayers = new List<Player>();
	public List<Player> GetNetPlayerList()
	{
		Debug.Log("GetNetPlayer Length: " + m_NetworkPlayers.Count);

		return m_NetworkPlayers;
	}

	public void AddPlayer(string ip, string name)
	{
		bool playerExists = false;
		foreach(Player p in m_NetworkPlayers)
		{
			if(p.IP == ip && p.Name == name)
			{
				playerExists = true;
			}
		}

		if(!playerExists)
		{
			m_NetworkPlayers.Add(new Player(ip, name));
		}
	}

	public void RemovePlayer(string ip, string name)
	{
		for(int i = m_NetworkPlayers.Count - 1; i >= 0; i--)
		{
			if(m_NetworkPlayers[i].IP == ip && m_NetworkPlayers[i].Name == name)
			{
				m_NetworkPlayers.RemoveAt(i);
			}
		}
	}

	private Player m_Player;
	public Player GetMyPlayer()
	{
		if (m_Player == null) 
		{
			m_Player = new Player(Network.player.ipAddress, "Player");
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
		//Application.LoadLevel("MainMenu");
	}

	public void OnDisconnectedFromServer()
	{
		Debug.Log("Something disconnected from server");
		m_NetworkPlayers.Clear(); //Clear List of players
		//Application.LoadLevel("MainMenu");
	}

	public void OnServerInitialized()
	{
		AddPlayer(Network.player.ipAddress, GetMyPlayer().Name);
	}

	public void OnConnectedToServer()
	{
		GetNetView().RPC("AddPlayer", RPCMode.All, Network.player.ipAddress, GetMyPlayer().Name);
	}

	public void OnPlayerConnected()
	{
		foreach(Player p in m_NetworkPlayers)
		{
			GetNetView().RPC("AddPlayer", RPCMode.All, p.IP, p.Name);
		}
	}

}

public class Player
{
	public string Name = "Poop";
	public string IP = "";
	public Player(string ip, string name)
	{
		IP = ip;
		Name = name;
	}

}
