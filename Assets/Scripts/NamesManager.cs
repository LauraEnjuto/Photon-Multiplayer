using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamesManager : MonoBehaviour
{
    #region Singleton
    public static NamesManager instance;
    #endregion

    #region Variables
    public Dictionary<int, string> playerNames = new Dictionary<int, string>();

    [HideInInspector] public string player1Name;
    [HideInInspector] public string player2Name;
    [HideInInspector] public string player3Name;
    [HideInInspector] public string player4Name;

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
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    #region RPC Methods

    //Method to update the player's name for all players in the network
    public void UpdatePlayerName(int actorNumber, string name)
    {
        view.RPC("UpdateNameRPC", RpcTarget.All, actorNumber, name);
    }

    //RPC Method invoked on all clients to update each player's name
    [PunRPC]
    private void UpdateNameRPC(int actorNumber, string name)
    {
        if (playerNames.ContainsKey(actorNumber))
        {
            playerNames[actorNumber] = name;
        }
        else
        {
            playerNames.Add(actorNumber, name);
        }
    }

    #endregion

    #region Public Methods
    public void DestroyThisOnLoad()
    {
        destroyOnNextLoad = true;
    }

    public string GetPlayerName(int playerNumber)
    {
        if (NamesManager.instance.playerNames.ContainsKey(playerNumber))
        {
            return NamesManager.instance.playerNames[playerNumber];
        }
        else
        {
            return "null";
        }
    }
    #endregion
}
