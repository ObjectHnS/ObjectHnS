using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RoomButton : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public string Name 
    {
        get
        {
            return transform.Find("RoomName").GetComponent<Text>().text;
        }
        set
        {
            transform.Find("RoomName").GetComponent<Text>().text = value;
        }
    }

    private RoomInfo info;

    public RoomInfo Roominfo
    {
        get
        {
            return info;
        }
        set
        {
            info = value;
        }
    }

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void Update()
    {
        if(Roominfo != null)
        {
            transform.Find("JoinCount").GetComponent<Text>().text = info.PlayerCount + " / 8";
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(Name);
        GetComponent<Button>().interactable = false;
    }
}
