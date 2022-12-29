using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}
