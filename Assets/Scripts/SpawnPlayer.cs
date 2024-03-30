using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    #region Variables
    public GameObject playerPrefabPlayer1;
    public GameObject playerPrefabPlayer2;
    public float minX, maxX, minY, maxY;
    #endregion

    #region Public Methods
    public void SpawnPlayerStart()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0.6f, Random.Range(minY, maxY));

        if (PhotonNetwork.IsMasterClient) 
        {
            PhotonNetwork.Instantiate(playerPrefabPlayer1.name, randomPosition, Quaternion.identity);
        }

        else
        {
            PhotonNetwork.Instantiate(playerPrefabPlayer2.name, randomPosition, Quaternion.identity);
        }
    }
    #endregion
}
