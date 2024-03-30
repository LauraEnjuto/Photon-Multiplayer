using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string nextSceneName;

    private void Start()
    {
        SoundManager.instance.PlayMusic("TitleTheme");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Invoke("LoadNextScene", 0.2f);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
