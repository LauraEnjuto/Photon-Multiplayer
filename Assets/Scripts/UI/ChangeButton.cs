using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButton : MonoBehaviour
{
    public Sprite[] buttonSprites;
    public Image targetButton;

    public void SpriteChange()
    {
        if (targetButton.sprite == buttonSprites[0])
        {
            targetButton.sprite = buttonSprites[1];
            return;
        }

        targetButton.sprite = buttonSprites[0];
    }
}
