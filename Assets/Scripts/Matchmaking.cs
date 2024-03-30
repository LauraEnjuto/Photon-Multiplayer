using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Matchmaking : MonoBehaviour
{
    #region Variables

    [Header("UI Settings")]
    public Button playButton;
    public TMP_Text playerCount;

    private int roomOwnerActorNumber;
    private bool isOwner;
    private PhotonView view;

    private int currentPlayers = 0;

    #endregion

    #region Unity Methods

    private void Start()
    {
        view = GetComponent<PhotonView>();

        if (PhotonNetwork.InRoom)
        {
            isOwner = IsOwner();

            //If player is the owner, activate play button
            if (isOwner)
            {
                Debug.Log("Player is the owner of the room");
                playButton.gameObject.SetActive(true);
                playButton.onClick.AddListener(TaskOnClick);
            }

            else
            {
                Debug.Log("Player is NOT the owner of the room");
                playButton.gameObject.SetActive(false);
            }
        }

        else
        {
            Debug.LogError("Not in a room!");
        }

    }

    private void Update()
    {
        CheckHowManyPlayers();
    }

    #endregion

    #region Private Methods
    //Method to check if the player is the owner of the room
    private bool IsOwner()
    {
        return PhotonNetwork.IsMasterClient;
    }

    //Method to change scene when play button is clicked
    private void TaskOnClick()
    {
        SoundManager.instance.PlaySFX("Button");

        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("ChangeSceneRPC", RpcTarget.AllBuffered, "GameScene");
        }
    }

    //Method to check how many players are currently in the room and update the player count text
    private void CheckHowManyPlayers()
    {     
        int playersOnline = PhotonNetwork.PlayerList.Length;        

        if (currentPlayers < playersOnline)
        {
            SoundManager.instance.PlaySFX("PlayerJoinRoom");
            currentPlayers++;
        }

        playerCount.text = playersOnline.ToString();
    }
    #endregion

    #region RPC Methods
    //RPC Method to change scene for all players
    [PunRPC]
    private void ChangeSceneRPC(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    #endregion

}
