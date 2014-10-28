using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
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

	[RPC]
	void StartGame()
	{
		NetworkManager.Instance().StartGame();
	}
}
