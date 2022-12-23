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
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    //public override void OnConnectedToMaster()
    //{
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    //}
    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.Instantiate("PF_Ghost_Blue", Vector3.zero, Quaternion.identity);
    //}
}
