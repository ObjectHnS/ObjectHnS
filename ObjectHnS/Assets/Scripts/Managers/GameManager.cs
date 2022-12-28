using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

static class ExtensionsClass
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


public class GameManager : Manager<GameManager>
{
    public bool isExit = false;
    public int OverCount = 0;

    public GameObject reaper;
    public GameObject ghost;
    private bool isCreated = false;

    private GameObject player;
    public GameObject Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
        }
    }
    public GameObject potal;

    public bool IsPlayerCreated
    {
        get
        {
            return isCreated;
        }
    }

    [SerializeField]
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
                this.player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", player.name), new Vector3(-69, -0.1f, -1), Quaternion.identity);

            }
            else
            {
                player = ghost;
                this.player = PhotonNetwork.Instantiate(Path.Combine("Prefabs", player.name), new Vector3(-24.5f + Random.Range(-3f, 3f), 3.5f + Random.Range(-3f, 3f), -1), Quaternion.identity);
            }

            GameObject camera = GameObject.Find("Main Camera");
            camera.transform.position = new Vector3(this.player.transform.position.x, this.player.transform.position.y, -10);
            camera.transform.parent = this.player.transform;

            isCreated = true;
        }
    }

    private bool isKeyGen = false;
    private void GenBrokenKey()
    {
        if (UIManager.Instance.IsStarted && !isKeyGen)
        {
            List<bool> keyList = new List<bool>();
            for (int i = 0; i < keyNumber; i++)
            {
                keyList.Add(true);
            }
            for (int i = 0; i < KeyPoints.Length - keyNumber; i++)
            {
                keyList.Add(false);
            }
            keyList.Shuffle();
            isKeyGen = true;
            for (int i = 0; i < KeyPoints.Length; i++)
            {
                if (keyList[i] && photonView.IsMine)
                {
                    PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PF_BrokenKey"), KeyPoints[i].position, Quaternion.identity);
                }
            }
        }

    }

    private bool isPotalCreated = false;
    private void Update()
    {
        //EndGame();
        SpawnPlayers();
        GenBrokenKey();
        if (BrokenKeyCount == 4 && !isPotalCreated)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs", potal.name), potal.transform.position, Quaternion.identity);
                isPotalCreated = true;
            }
        }
    }

    //private void EndGame()
    //{
    //    if (OverCount == PhotonNetwork.CurrentRoom.PlayerCount - 1 && SceneManager.GetActiveScene().name == "GameScene")
    //    {
    //        if (PhotonNetwork.LocalPlayer.CustomProperties["isReaper"] != null)
    //        {
    //            if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["isReaper"])
    //            {
    //                if (isExit)
    //                {
    //                    SceneManager.LoadScene("ReaperDScene");
    //                }
    //                else
    //                {
    //                    SceneManager.LoadScene("ReaperVScene");
    //                }
    //            }

    //            else
    //            {
    //                if (isExit)
    //                {
    //                    SceneManager.LoadScene("GhostVScene");
    //                }
    //                else
    //                {
    //                    SceneManager.LoadScene("GhostDScene");
    //                }
    //            }
    //        }
    //    }
    //}
}
