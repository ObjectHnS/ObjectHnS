using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : Manager<GameManager>
{
    public GameObject reaper;
    public GameObject ghost;
    private bool isCreated = false;

    public GameObject player;

    public bool IsPlayerCreated
    {
        get 
        {
            return isCreated;
        }
    }
    private int brokenKeyCount = 0;
    public int BrokenKeyCount
    {
        get 
        { 
            return brokenKeyCount; 
        }
        set
        {
            brokenKeyCount = value;
        }
    }

    Hashtable playerProperty = new Hashtable { { "isReaper", false } };
    private void SpawnPlayers()
    {
        if (UIManager.Instance.IsStarted && !isCreated)
        {
            if (PhotonNetwork.IsMasterClient) playerProperty["isReaper"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperty);

            GameObject player = null;
            if ((bool)playerProperty["isReaper"])
            {
                player = reaper;
            }
            else
            {
                player = ghost;
            }

            this.player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", player.name), Vector3.zero, Quaternion.identity);
            isCreated = true;
        }
    }
    private void Start()
    {
    }
    private void Update()
    {
        SpawnPlayers();
    }
}
