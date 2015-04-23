using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomParticipant : MonoBehaviour {

	public Text playerText;
	public Image readyImage;
	public PhotonPlayer player;

	// Use this for initialization
	void Start () {
		if(player.name != "")
			playerText.text = player.name;
		else
			playerText.text = "NoName";
	}
	
	// Update is called once per frame
	void Update () {
		if(!player.isMasterClient)
		{
			if((bool)player.customProperties["Ready"])
				readyImage.color = Color.green;
			else
				readyImage.color = Color.gray;
		}
		else
		{
			readyImage.color = Color.blue;
		}
	}
}
