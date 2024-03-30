using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCoin : MonoBehaviour
{
    [SerializeField] private float coinSpeed = 250f;

    void Update()
    {
        transform.Rotate(0f, coinSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
