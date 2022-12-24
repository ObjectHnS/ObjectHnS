using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : Manager<GameManager>
{
    public GameObject reaper;
    public GameObject ghost;
    private bool isCreated = false;
    public bool IsPlayerCreated
    {
        get 
        {
            return isCreated;
        }
    }

    Hashtable playerProperty = new Hashtable { { "isReaper", false } };
    private void Update()
    {
        if(UIManager.Instance.IsStarted && !isCreated)
        {
            if (PhotonNetwork.IsMasterClient) playerProperty["isReaper"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperty);

            GameObject player = null;
            if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["isReaper"])
            {
                player = reaper;
            }
            else player = ghost;

            PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity);
            isCreated = true;
        }
    }
}
