using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseAName : MonoBehaviour
{
    #region Singleton
    public static ChooseAName instance;
    #endregion

    #region Variables
    public TMP_InputField nameImput;
    public string playerName;

    private bool destroyOnNextLoad = false;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        nameImput.onEndEdit.AddListener(OnInputEndEdit);        
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

    #region Public Methods

    //Set the flag to destroy this object on next scene load
    public void DestroyThisOnLoad()
    {
        destroyOnNextLoad = true;
    }
    #endregion

    #region Private Methods

    private void OnInputEndEdit(string text)
    {
        // If the return key is pressed, in this case Intro
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Set the player name to the text entered in the imput field
            playerName = nameImput.text;

            //Load the next scene
            SceneManager.LoadScene("Lobby");
        }
    }

    #endregion

}
