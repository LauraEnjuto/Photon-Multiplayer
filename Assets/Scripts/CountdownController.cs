using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    [Header("Countdown Settings")]
    public TMP_Text countdownTMP;
    public int countdownTime = 3;

    private void Start()
    {
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.sfxSource.volume = 0.5f;
        SoundManager.instance.PlaySFX("Countdown");
        StartCoroutine(CountdownToStart());        
    }

    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
        {
            countdownTMP.text = countdownTime.ToString();            
            SoundManager.instance.PlaySFX("Countdown");

            yield return new WaitForSeconds(1f);
            
            countdownTime--;
        }

        SoundManager.instance.PlaySFX("GameStart");

        countdownTMP.text = "GO!";

        GameController.instance.BeginGame();

        SoundManager.instance.sfxSource.volume = 1f;

        yield return new WaitForSeconds(1f);

        countdownTMP.gameObject.SetActive(false);
    }
}
