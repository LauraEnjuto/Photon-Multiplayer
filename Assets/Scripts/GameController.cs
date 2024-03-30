using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class GameController : MonoBehaviour
{
    #region Variables

    [Header("Spawn Settings")]
    public GameObject smallCoinPrefab;
    public GameObject bigCoinPrefab;
    public float spawnChance = 4f;
    
    SoundManager soundManager;

    //Game Settings
    public bool gamePlaying { get; private set; }
    public static GameController instance;
    public SpawnPlayer spawnPlayer;

    [HideInInspector] public int actorNumber;

    [Header("Raycast Settings")]
    public float distanceBetweenChecks = 3f;
    public float heightOfCheck = 10f, rangeOfCheck = 30f;
    public LayerMask layerMask;
    public Vector2 positivePosition, negativePosition;

    [Header("Round Settings")]
    public int singleRoundDuration = 30;
    public int numberOfRounds = 6;
    [SerializeField] private TMP_Text timerTMP, roundTMP, gameResultTMP;
    private int totalRoundsDuration, currentRound = 1;
    private float currentTimer;
    private float startTime;
    private float timeToPlaySound;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (soundManager == null)
        {
            soundManager = SoundManager.instance;
            if (soundManager == null)
            {
                Debug.LogError("Sound Manager instance not found!");
                return;
            }
        }

        totalRoundsDuration = singleRoundDuration * numberOfRounds;
        currentTimer = totalRoundsDuration;
        timeToPlaySound = Mathf.FloorToInt(numberOfRounds / 2);
        soundManager.sfxSource2.pitch = 1f;

        gamePlaying = false;
    }

    private void Update()
    {
        if (gamePlaying)
        {
            Timer();
        }        
    }

    #endregion

    #region Game Methods

    //Method to begin the game
    public void BeginGame()
    {
        gamePlaying = true;

        soundManager.PlayMusic("Theme");

        spawnPlayer.SpawnPlayerStart();
        SpawnResources();
        StartCoroutine(Rounds());        
    }

    //Method to end the game
    private void EndGame()
    {
        gamePlaying = false;

        soundManager.musicSource.Stop();
        soundManager.sfxSource2.pitch = 1f;
        soundManager.PlaySFX("GameEnd");

        DetermineGameResult();
        Invoke("ShowGameOverScreen", 5f);
    }

    //Method to sow the game over screen
    private void ShowGameOverScreen()
    {
        SceneManager.LoadScene(sceneName: "GameOverScene");
    }

    //Method to handle the in-game timer
    private void Timer()
    {    
        roundTMP.text = "Round" + " " + currentRound.ToString();

        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
        }

        else if (currentTimer < 0)
        {
            currentTimer = 0;
            StopCoroutine(Rounds()); 
            DestroyCoins();
            timerTMP.color = Color.red;
            EndGame();
        }

        // Calculate the remaining minutes and seconds
        int minutes = Mathf.FloorToInt(currentTimer / 60);
        int seconds = Mathf.FloorToInt(currentTimer % 60);

        // Update the timer text with the format MM:SS
        timerTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //Method to spawn resources, in this case we are doing small and big coins
    private void SpawnResources()
    {
        // Iterate through each position within the range in both x and z directions (we use y because we are using a Vector2, but it corresponds to z axis in editor)
        for (float x  = negativePosition.x; x < positivePosition.x; x += distanceBetweenChecks)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenChecks)
            {
                RaycastHit hit;

                //Cast a ray to check that the next spawn position is correct
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask))
                {
                    // If the random spawn chance is successful, spawn a resource
                    if (spawnChance > UnityEngine.Random.Range(0, 101))
                    {
                        //Determine if we are spawning a big coin or a small coin based on a random number
                        float random = UnityEngine.Random.Range(0, 360);

                        if(random < 300)
                        {          
                            //Use PhotonNetwork.InstantiateRoomObject to make sure there is only 1 coin for all players and no duplicates
                            PhotonNetwork.InstantiateRoomObject(smallCoinPrefab.name, new Vector3(hit.point.x, 0.1f, hit.point.z), Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));
                        }

                        else
                        {                        
                            PhotonNetwork.InstantiateRoomObject(bigCoinPrefab.name, new Vector3(hit.point.x, 0.1f, hit.point.z), Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));
                        }
                        
                    }
                }
            }
        }
    }

    //Method to destroy the coins
    private void DestroyCoins()
    {
        GameObject[] gos_1 = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] gos_2 = GameObject.FindGameObjectsWithTag("BigCoin");
        GameObject[] gos = gos_1.Concat(gos_2).ToArray();

        foreach (GameObject go in gos)
        {
            Destroy(go);
        }
    }

    //Method to determine game result 
    private void DetermineGameResult()
    {
        gameResultTMP.gameObject.SetActive(true);

        int player1Score = ScoreManager.instance.GetPlayerScore(1);
        int player2Score = ScoreManager.instance.GetPlayerScore(2);
        int player3Score = ScoreManager.instance.GetPlayerScore(3);
        int player4Score = ScoreManager.instance.GetPlayerScore(4);

        string player1Name = NamesManager.instance.GetPlayerName(1);
        string player2Name = NamesManager.instance.GetPlayerName(2);
        string player3Name = NamesManager.instance.GetPlayerName(3);
        string player4Name = NamesManager.instance.GetPlayerName(4);

        if (player1Score > player2Score && player1Score > player3Score && player1Score > player4Score)
        {
            gameResultTMP.text = player1Name + " " + "wins!";
        }
        else if (player2Score > player1Score && player2Score > player3Score && player2Score > player4Score)
        {
            gameResultTMP.text = player2Name + " " + "wins!";
        }
        else if (player3Score > player1Score && player3Score > player2Score && player3Score > player4Score)
        {
            gameResultTMP.text = player3Name + " " + "wins!";
        }
        else if (player4Score > player1Score && player4Score > player2Score && player4Score > player3Score)
        {
            gameResultTMP.text = player4Name + " " + "wins!";
        }
        else
        {
            gameResultTMP.text = "It's a tie!";
        }
    }
    
    //Coroutine for managing rounds
    IEnumerator Rounds()
    {
        while(currentRound < numberOfRounds)
        {
            if (currentRound == timeToPlaySound)
            {
                soundManager.PlaySFX2("HavingFun");
            }

            if (currentRound == (timeToPlaySound + 1))
            {
                //soundManager.sfxSource2.pitch = 1.2f;
                //soundManager.PlaySFX2("Laugh");
            }          

            yield return new WaitForSeconds(singleRoundDuration);

            DestroyCoins();

            yield return new WaitForSeconds(0.5f);
            currentRound += 1;
            SpawnResources();
        }
    }

    #endregion

}
