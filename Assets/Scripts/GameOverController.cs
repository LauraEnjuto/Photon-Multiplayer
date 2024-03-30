using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviourPunCallbacks
{
    #region Variables

    [Header("Button Settings")]
    public Button titleButton;
    public Button replayButton;
    public Button quitButton;

    [Header("Score Settings")]
    public TMP_Text p1ScoreTMP;
    public TMP_Text p2ScoreTMP;
    public TMP_Text p3ScoreTMP;
    public TMP_Text p4ScoreTMP;

    [Header("Player Name Settings")]
    public TMP_Text p1NameTMP;
    public TMP_Text p2NameTMP;
    public TMP_Text p3NameTMP;
    public TMP_Text p4NameTMP;

    private PhotonView view;

    private ScoreManager scoreManager;
    private NamesManager namesManager;
    private ChooseAName chooseAName;
    private SoundManager soundManager;

    #endregion

    #region Unity Methods
    void Start()
    {
        view = GetComponent<PhotonView>();
                
        if (scoreManager == null)
        {
            scoreManager = ScoreManager.instance;
            if (scoreManager == null)
            {
                Debug.LogError("ScoreManager instance not found!");
                return;
            }
        }

        if (namesManager == null)
        {
            namesManager = NamesManager.instance;
            if (namesManager == null)
            {
                Debug.LogError("NamesManager instance not found!");
                return;
            }
        }

        if (chooseAName == null)
        {
            chooseAName = ChooseAName.instance;
            if (chooseAName == null)
            {
                Debug.LogError("ChooseAName instance not found!");
                return;
            }
        }

        if (soundManager == null)
        {
            soundManager = SoundManager.instance;
            if (soundManager == null)
            {
                Debug.LogError("ChooseAName instance not found!");
                return;
            }
        }

        DisplayScores();
        DisplayNames();

        soundManager.PlayMusic("TitleTheme");
        titleButton.onClick.AddListener(ReturnToTitleAfterSound);
        replayButton.onClick.AddListener(ReplayAfterSound);
        quitButton.onClick.AddListener(QuitAfterSound);        
    }
    #endregion

    #region Public Methods
    //Method to display all player scores in the scoreboard
    public void DisplayScores()
    {
        foreach (var player in scoreManager.playerScores)
        {
            switch (player.Key)
            {
                case 1:
                    p1ScoreTMP.text = player.Value.ToString();
                    break;
                case 2:
                    p2ScoreTMP.text = player.Value.ToString();
                    break;
                case 3:
                    p3ScoreTMP.text = player.Value.ToString();
                    break;
                case 4:
                    p4ScoreTMP.text = player.Value.ToString();
                    break;
                default:
                    Debug.LogWarning("Invalid player key: " + player.Key);
                    break;
            }
        }
    }

    //Method to display all player names in the scoreboard
    public void DisplayNames()
    {
        foreach (var player in namesManager.playerNames)
        {
            switch (player.Key)
            {
                case 1:
                    p1NameTMP.text = player.Value;
                    break;
                case 2:
                    p2NameTMP.text = player.Value;
                    break;
                case 3:
                    p3NameTMP.text = player.Value;
                    break;
                case 4:
                    p4NameTMP.text = player.Value;
                    break;
                default:
                    Debug.LogWarning("Invalid player key: " + player.Key);
                    break;
            }
        }
    }

    //Method to return to title screen
    public void ReturnToTitle()
    {       
        scoreManager.DestroyThisOnLoad();
        chooseAName.DestroyThisOnLoad();
        namesManager.DestroyThisOnLoad();
        soundManager.DestroyThisOnLoad();

        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("TitleScreen");
    }

    //Method to return to loading screen
    public void Replay()
    {       
        scoreManager.DestroyThisOnLoad();
        chooseAName.DestroyThisOnLoad();
        namesManager.DestroyThisOnLoad();

        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("LoadingScene");
    }

    //Method to close and quit the application
    public void QuitApplication()
    {       
        Application.Quit();
    }

    public void QuitAfterSound()
    {
        soundManager.PlaySFX("Button");
        Invoke("QuitApplication", 0.1f);
    }

    public void ReturnToTitleAfterSound()
    {
        soundManager.PlaySFX("Button");
        Invoke("ReturnToTitle", 0.1f);
    }

    public void ReplayAfterSound()
    {
        soundManager.PlaySFX("Button");
        Invoke("Replay", 0.1f);
    }
    #endregion


}
