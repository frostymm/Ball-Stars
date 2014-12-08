using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		NetworkManager.Instance().SetNetView((NetworkView)GetComponent<NetworkView>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnFailedToConnect()
	{
		NetworkManager.Instance().OnFailedToConnect();
	}

	void OnDisconnectedFromServer()
	{
		NetworkManager.Instance().OnDisconnectedFromServer();
	}

	void OnServerInitialized()
	{
		NetworkManager.Instance().OnServerInitialized();
	}

	void OnConnectedToServer()
	{
		NetworkManager.Instance().OnConnectedToServer();
	}

	void OnPlayerConnected()
	{
		NetworkManager.Instance().OnPlayerConnected();
	}

	[RPC]
	void StartGame()
	{
		NetworkManager.Instance().StartGame();
	}

	[RPC]
	void AddPlayer(string ip, string name)
	{
		NetworkManager.Instance().AddPlayer(ip, name);
	}

	[RPC]
	void RemovePlayer(string ip, string name)
	{
		NetworkManager.Instance().RemovePlayer(ip, name);
	}
}
