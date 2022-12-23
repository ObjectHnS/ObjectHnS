using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RoomButton : MonoBehaviour
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

    private Button button;

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
        else if(PhotonNetwork.CurrentRoom != null)
        {
            transform.Find("JoinCount").GetComponent<Text>().text = PhotonNetwork.CurrentRoom.PlayerCount + " / 8";
        }
    }

    public void JoinRoom()
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            PhotonNetwork.JoinRoom(Name);
            button = GetComponent<Button>();
        }
        else
        {
            var roomCanvas = GameObject.Find("Canvas").transform.Find("RoomPage").gameObject;
            if (roomCanvas)
            {
                roomCanvas.SetActive(true);
            }
        }
    }
}
