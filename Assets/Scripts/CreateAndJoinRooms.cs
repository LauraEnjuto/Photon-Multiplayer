using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateRoom()
    {
        //We modified the settings for Room Options so only 4 players can enter the room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        SoundManager.instance.PlaySFX("Button");
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }

    public void JoinRoom()
    {
        SoundManager.instance.PlaySFX("Button");
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("WaitingRoom");
    }
}
