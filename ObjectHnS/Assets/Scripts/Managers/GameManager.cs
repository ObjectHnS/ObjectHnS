using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    public GameObject playerObject;
    private bool isCreated = false;

    private void Update()
    {
        if(UIManager.Instance.isStartedGame && !isCreated)
        {
            PhotonNetwork.Instantiate(playerObject.name, Vector3.zero, Quaternion.identity);
            isCreated = true;
        }
    }
}
