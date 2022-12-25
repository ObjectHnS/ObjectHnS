using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NetworkManager : Manager<NetworkManager>
{
    private PhotonView pv;
    public PhotonView phtonView
    {
        get
        {
            return pv;
        }
    }
    protected override void Awake()
    {
        base.Awake();

        pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //public override void OnConnectedToMaster()
    //{
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    //}
    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.Instantiate("PF_BrokenKey", new Vector3(-3.7f, 1.2f, 0), Quaternion.identity);
    //}
}
