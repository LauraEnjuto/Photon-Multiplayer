using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScaleTextAnimation : MonoBehaviour
{
    #region Variables
    public TMP_Text text;
    public float scaleFactor = 1.1f; 
    public float duration = 0.4f;
    #endregion

    #region Unity Methods
    void Start()
    {
        ScaleTextYoyoAnimation();
    }
    #endregion

    #region Private Methods
    private void ScaleTextYoyoAnimation()
    {        
        text.transform.DOScale(scaleFactor, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    #endregion
}
