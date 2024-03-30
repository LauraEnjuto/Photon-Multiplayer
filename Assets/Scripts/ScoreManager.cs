using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static ScoreManager instance;
    #endregion

    #region Variables

    public Dictionary<int, int> playerScores = new Dictionary<int, int>();

    [HideInInspector] public int player1Score;
    [HideInInspector] public int player2Score;
    [HideInInspector] public int player3Score;
    [HideInInspector] public int player4Score;

    private PhotonView view;

    private bool destroyOnNextLoad = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        instance = this;

        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (destroyOnNextLoad)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    #region RPC Methods
    //Method to update the player's score for all players in the network
    public void UpdatePlayerScore(int actorNumber, int score)
    {
        view.RPC("UpdateScoreRPC", RpcTarget.All, actorNumber, score);
    }

    //RPC Method invoked on all clients to update each player's score
    [PunRPC]
    private void UpdateScoreRPC(int actorNumber, int score)
    {
        if (playerScores.ContainsKey(actorNumber))
        {
            playerScores[actorNumber] = score;
        }
        else
        {
            playerScores.Add(actorNumber, score);
        }
    }
    #endregion

    #region Public Methods
    //Set the flag to destroy this object on next scene load
    public void DestroyThisOnLoad()
    {
        destroyOnNextLoad = true;
    }

    //Method to get the player score based on his actor number
    public int GetPlayerScore(int playerNumber)
    {
        if (ScoreManager.instance.playerScores.ContainsKey(playerNumber))
        {
            return ScoreManager.instance.playerScores[playerNumber];
        }
        else
        {
            return 0; 
        }
    }
    #endregion
}
