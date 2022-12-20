using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Manager<NetworkManager>
{
    [Header("Pages")]
    public GameObject LoginCanvas;
    public GameObject State;
    public GameObject LobbyCanvas;
    public GameObject RoomCanvas;
    public GameObject Popups;

    [Header("Room Objects")]
    public GameObject roomGrid;
    public GameObject roomButton;
    public GameObject UserCard;

    [Header("etc..")]
    public PhotonView pv;
    private GameObject roomCreation;
    private GameObject warning;

    private Dictionary<string, GameObject> myList = new Dictionary<string, GameObject>();
    private Dictionary<Player, GameObject> userDict = new Dictionary<Player, GameObject>();
    private List<Player> players = new List<Player>();

    private bool isEnter = false;

    private GameObject player;

    #region 포톤
    protected override void Awake()
    {
        base.Awake();
        Screen.SetResolution(1920, 1080, false);

        if (Popups) Popups.SetActive(false);
        if (LoginCanvas) LoginCanvas.SetActive(false);
        if (LobbyCanvas) LobbyCanvas.SetActive(false);
        if (RoomCanvas) RoomCanvas.SetActive(false);

        if (State) State.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        UpateState();
        UpdateJoinCount();
        UpdateUserList();
        MobileInputSystem();
    }

    private void MobileInputSystem()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (Popups.activeSelf)
            {
                Popups.SetActive(false);
            }
            else if(RoomCanvas.activeSelf)
            {
                RoomCanvas.SetActive(false);
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        if (State) State.SetActive(false);
        if (LoginCanvas) LoginCanvas.SetActive(true);

        PhotonNetwork.JoinLobby();
    }

    private void UpateState()
    {
        if (State)
        {
            if (State.activeSelf)
            {
                string stateText = PhotonNetwork.NetworkClientState.ToString();
                if (stateText == "Authenticating")
                {
                    stateText = "인증중";
                }
                else if (stateText == "ConnectingToMasterServer")
                {
                    stateText = "마스터 서버에 연결하는중";
                }
                else if (stateText == "ConnectedToMasterServer")
                {
                    stateText = "마스터 서버에 연결됨";
                }
                State.GetComponent<Text>().text = stateText;
            }
        }
    }

    private void UpdateJoinCount()
    {
        if (isEnter)
        {
            RoomCanvas.transform.Find("JoinCount").GetComponent<Text>().text = "(" + PhotonNetwork.CurrentRoom.PlayerCount + " / 8)";
            RoomCanvas.transform.Find("RoomName").GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
        }
    }

    public void Connect()
    {
        if (LoginCanvas)
        {
            PhotonNetwork.LocalPlayer.NickName = LoginCanvas.transform.GetChild(2).GetComponent<InputField>().text;
            Debug.Log(PhotonNetwork.LocalPlayer.NickName);

            LoginCanvas.SetActive(false);
            if (LobbyCanvas) LobbyCanvas.SetActive(true);
        }
        else PhotonNetwork.LocalPlayer.NickName = "Player" + UnityEngine.Random.Range(1, 100);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Popups.SetActive(true);
        Popups.transform.Find("Timeout").gameObject.SetActive(true);
        PhotonNetwork.Reconnect();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    #endregion

    #region 방 관련
    public void ShowCreateRoomPopup()
    {
        if (Popups)
        {
            Popups.SetActive(true);
            if (!roomCreation)
            {
                roomCreation = Popups.transform.Find("RoomCreation").gameObject;
            }
        }
        roomCreation.SetActive(true);
    }

    private static List<Player> playerList = new List<Player>();
    
    private void UpdateUserList()
    {
        
        if (isEnter)
        {
            playerList = new List<Player>(PhotonNetwork.CurrentRoom.Players.Values);
            foreach(Player p in playerList)
            {
                if (!players.Contains(p))
                {
                    var card = Instantiate(UserCard, RoomCanvas.transform.Find("Userboard").Find("Grid"));
                    card.transform.Find("UserName").GetComponent<Text>().text = p.NickName;

                    userDict.Add(p, card);
                    players.Add(p);
                }
            }
            foreach (var p in players.ToArray())
            {
                GameObject obj = null;
                if(!playerList.Contains(p))
                {
                    userDict.TryGetValue(p, out obj);
                    if (obj) Destroy(obj);

                    userDict.Remove(p);
                    players.Remove(p);
                }
            }
            
        }
    }

    public void LeaveRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            Destroy(GameObject.Find(PhotonNetwork.CurrentRoom.Name));
        }
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        RoomCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
        isEnter = false;
    }

    private GameObject CreateRoomButton(string roomName)
    {
        roomButton.GetComponent<RoomButton>().Name = roomName;
        var obj = Instantiate(roomButton, roomGrid.transform); 
        obj.name = roomName;
        return obj;
    }

    public void CreateRoom()
    {
        InputField roomName = roomCreation.transform.Find("RoomName").gameObject.GetComponent<InputField>();
        if (roomName.text == "")
        {
            ShowWarningPopupMessage("방 이름이 비어있습니다!");
            return;
        }
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 8 });

        HidePopups();

        CreateRoomButton(roomName.text);
    }

    private void ShowWarningPopupMessage(string message)
    {
        roomCreation.SetActive(false);
        if (warning == null)
        {
            warning = Popups.transform.Find("Warning").gameObject;
        }
        warning.transform.Find("Message").GetComponent<Text>().text = message;
        warning.SetActive(true);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        if (RoomCanvas) RoomCanvas.SetActive(true);
        isEnter = true;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Scenes/Fugitive");
        this.Invoke(() =>
        {
            player = PhotonNetwork.Instantiate("PF_Ghost_Blue", Vector3.zero, Quaternion.identity);
            player.GetComponent<MonsterInputJoystick>().joystick = (FloatingJoystick)FindObjectOfType(typeof(FloatingJoystick));
        }, 0.5f);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tmp = null;
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                myList.TryGetValue(room.Name, out tmp);
                Destroy(tmp);
            }
            else
            {
                if (!myList.ContainsKey(room.Name))
                {
                    GameObject newRoom = CreateRoomButton(room.Name);
                    newRoom.GetComponent<RoomButton>().Roominfo = room;
                    newRoom.name = room.Name;
                    myList.Add(room.Name, newRoom);
                }
                else
                {
                    myList.TryGetValue(room.Name, out tmp);
                    tmp.GetComponent<RoomButton>().Roominfo = room;
                }
            }
        }
    }
    #endregion

    #region 팝업창
    private void HidePopups()
    {
        Popups.SetActive(false);
    }
    public void CancelPopup(GameObject popup)
    {
        Popups.SetActive(false);
        popup.SetActive(false);
    }
    #endregion
}

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}