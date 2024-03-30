using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreCounter : MonoBehaviourPunCallbacks
{
    #region Variables

    [Header("Score Settings")]
    private TMP_Text scoreCount;

    [HideInInspector] public int currentScore;
    public static ScoreCounter instance;
    public ChooseAName chooseAName;
    public string myName;

    SoundManager soundManager;

    [Header("Player Settings")]
    private TMP_Text playerNumber;

    public int playerId;
    private int score;

    public PhotonView view;
    public ScoreManager sm;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        chooseAName = GameObject.FindGameObjectWithTag("Name").GetComponent<ChooseAName>();
        myName = chooseAName.playerName;

        instance = this;

        scoreCount = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
        sm = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        playerNumber = GameObject.FindGameObjectWithTag("PlayerNum").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();

        if (soundManager == null)
        {
            soundManager = SoundManager.instance;
            if (soundManager == null)
            {
                Debug.LogError("Sound Manager instance not found!");
                return;
            }
        }

        currentScore = 0;

        if (view.IsMine)
        {
            playerId = view.Owner.ActorNumber;
            //playerNumber.text = "Player" + " " + playerId.ToString();
            playerNumber.text = chooseAName.playerName;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            ScoreManager.instance.UpdatePlayerScore(playerId, currentScore);
            NamesManager.instance.UpdatePlayerName(playerId, myName);
        }
    }

    #endregion

    #region Private Methods
    //Method that updates the player score and the destruction of coins
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            //Update current player score
            if (view.IsMine)
            {
                //Audio
                soundManager.PlaySFX("Coin");

                //Score
                currentScore += 1;
                scoreCount.text = currentScore.ToString();
            }

            //Destroy the coin for all clients 
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(other.gameObject);

        }

        else if (other.gameObject.CompareTag("BigCoin"))
        {
            //Update current player score
            if (view.IsMine)
            {
                //Audio
                soundManager.PlaySFX("Coin");

                //Score
                currentScore += 5;
                scoreCount.text = currentScore.ToString();
            }

            //Destroy the big coin for all clients 
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(other.gameObject);
        }           
    }
    #endregion
}
