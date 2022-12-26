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

    public int keyNumber;
    public Transform[] KeyPoints;

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

            this.player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", player.name), new Vector3(0, 0, -1), Quaternion.identity);
            GameObject camera = GameObject.Find("Main Camera");
            camera.transform.parent = this.player.transform;

            isCreated = true;
        }
    }
    private void Start()
    {
        
    }
    private bool isKeyGen = false;

    private void Update()
    {
        SpawnPlayers();
        if(UIManager.Instance.IsStarted && !isKeyGen)
        {
            for (int i = 0; i < KeyPoints.Length; i++)
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PF_BrokenKey"), KeyPoints[i].position, Quaternion.identity);
                }
            }
            isKeyGen = true;
        }
        
    }
}
