using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerObject : MonoBehaviour {

	public Button roomJoinButton;
	public Text roomNameText;

	public RoomInfo room;

	public void JoinRoom()
	{
		NetworkManager.Instance().JoinRoom(room);
	}

	// Use this for initialization
	void Start () {
	
	}

	private bool isFull = false;
	// Update is called once per frame
	void Update () {
		if(room != null)
		{
			if(room.open)
			{
				if(room.playerCount >= room.maxPlayers)
				{
					if(roomJoinButton.IsInteractable())
						roomJoinButton.interactable = false;

					isFull = true;
				}
				else
				{
					if(!roomJoinButton.IsInteractable())
						roomJoinButton.interactable = true;

					isFull = false;
				}


				if(isFull == false)
				{
					if(roomNameText.text != room.name)
						roomNameText.text = room.name;
				}
				else
				{
					string newRoomName = room.name + " (Full)";
					if(roomNameText.text != newRoomName)
						roomNameText.text = newRoomName;
				}
			}
			else
			{
				if(roomJoinButton.IsInteractable())
					roomJoinButton.interactable = false;

				string newRoomName = room.name + " (In Progress)";
				if(roomNameText.text != newRoomName)
					roomNameText.text = newRoomName;
			}
		}
	
	}
}
