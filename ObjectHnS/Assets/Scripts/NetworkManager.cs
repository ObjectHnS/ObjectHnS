using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    private Dictionary<string, GameObject> myList = new Dictionary<string, GameObject>(); // RoomName / RoomButton Object
    private Dictionary<Player, GameObject> userDict = new Dictionary<Player, GameObject>(); // Player / Player GameObject
    private List<RoomInfo> roomList = new List<RoomInfo>(); // List of rooms

    private bool isEnter = false;

    private GameObject player;
    private Player master = null;

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

    #endregion

    #region 방 관련
    private List<Player> playerList = new List<Player>();

    // 방 생성 실패시
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    // 방 입장 실패시
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    // 랜덤방 입장 실패시
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ShowWarningPopupMessage("방이 없습니다!\n 방을 만들어주세요!");
        Debug.Log(returnCode + " : " + message);
    }

    // 방에 들어갔을때 플레이어 리스트 갱신
    private void UpdateUserList()
    {
        if (isEnter) // 만약 방 버튼을 눌러 들어왔다면
        {
            playerList = new List<Player>(PhotonNetwork.CurrentRoom.Players.Values);

            if(!PhotonNetwork.IsMasterClient)
            {
                RoomCanvas.transform.Find("StartButton").GetComponent<Button>().interactable = false;
            }

            foreach (Player p in playerList.ToArray()) // 새로 들어온 사람 체크하고 카드 생성
            {
                if (!userDict.Keys.Contains(p)) // 방의 플레이어 리스트 추가 / 만약 딕셔너리에 플레이어가 존재하지 않는다면
                {    
                    // 새로운 유저 카드 생성
                    var card = Instantiate(UserCard, RoomCanvas.transform.Find("Userboard").Find("Grid"));
                    card.transform.Find("UserName").GetComponent<Text>().text = p.NickName; // 유저 이름
                    card.name = p.NickName; // 게임오브젝트
                    if (p.NickName == master.NickName) // 만약 방장이라면
                    {
                        card.transform.Find("UserName").GetComponent<Text>().color = Color.magenta;
                    }
                    userDict.Add(p, card);
                }
            }
            foreach (var p in userDict.Keys.ToArray()) //나간 사람 체크
            {
                GameObject obj = null;
                if(!playerList.Contains(p))
                {
                    userDict.TryGetValue(p, out obj);
                    if (obj) Destroy(obj);

                    userDict.Remove(p);
                }
            }
        }
    }

    private void FetchRoomList()
    {
        foreach (var room in myList.Values.ToArray())
        {
            Destroy(room);
        }
        myList.Clear();
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            myList.Remove(PhotonNetwork.CurrentRoom.Name);
        }
        foreach (var room in roomList.ToArray())
        {
            var obj = CreateRoomButton(room.Name);
            obj.GetComponent<RoomButton>().Roominfo = room;
            myList.Add(room.Name, obj);
        }
    }

    // '나가기'버튼을 눌렀을 때 호출되는 함수
    public void LeaveRoom()
    {
        // 유저 카드 삭제
        foreach (var p in PhotonNetwork.CurrentRoom.Players.Values.ToArray())
        {
            GameObject t = null;
            userDict.TryGetValue(p, out t);
            Destroy(t);
        }
        FetchRoomList(); // 방 리스트 새로고침
        if(PhotonNetwork.LocalPlayer == master && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            if (pv.IsMine)
            {
                pv.RPC("SetMaster", RpcTarget.AllBuffered, userDict.Keys.ToList()[1]);
            }
        }
        if(PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom(); // 방 떠나기
    }

    // 방을 나갔을 때 호출되는 함수
    public override void OnLeftRoom()
    {
        RoomCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
        isEnter = false;
    }

    // 새로운 방 버튼을 만드는 함수
    private GameObject CreateRoomButton(string roomName)
    {
        roomButton.GetComponent<RoomButton>().Name = roomName;
        var obj = Instantiate(roomButton, roomGrid.transform); 
        obj.name = roomName;
        return obj;
    }

    // 새로운 방을 만드는 onclick함수
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

        myList.Add(roomName.text, CreateRoomButton(roomName.text));
    }

    // 랜덤 방에 접속했을 때 호출되는 함수
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // 방에 접속했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            master = PhotonNetwork.LocalPlayer;
            if (pv.IsMine) pv.RPC("SetMaster", RpcTarget.AllBuffered, master);
        }
        if (RoomCanvas) RoomCanvas.SetActive(true);
        userDict = new Dictionary<Player, GameObject>();
        isEnter = true;
    }
    [PunRPC]
    private void SetMaster(Player m)
    {
        master = m;
    }

    // 시작 버튼을 눌렀을 때 호출되는 함수
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Scenes/Fugitive");
        this.Invoke(() =>
        {
            player = PhotonNetwork.Instantiate("PF_Ghost_Blue", Vector3.zero, Quaternion.identity);
            player.GetComponent<MonsterInputJoystick>().joystick = (FloatingJoystick)FindObjectOfType(typeof(FloatingJoystick));
        }, 0.5f);
    }

    // 방 리스트가 업데이트 되었을 때 호출되는 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tmp = null;
        this.roomList = new List<RoomInfo>(roomList);
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                myList.TryGetValue(room.Name, out tmp);
                Destroy(tmp);
                myList.Remove(room.Name);
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
                    if(tmp) tmp.GetComponent<RoomButton>().Roominfo = room;
                }
            }
        }
    }
    #endregion

    #region 팝업창

    // 새 방 만들기 팝업창 띄우기
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

    // 경고 메세지 팝업창 띄우기
    private void ShowWarningPopupMessage(string message)
    {
        Popups.SetActive(true);
        if(!roomCreation) roomCreation = Popups.transform.Find("RoomCreation").gameObject;
        roomCreation.SetActive(false);

        if (!warning) warning = Popups.transform.Find("Warning").gameObject;
        warning.transform.Find("Message").GetComponent<Text>().text = message;
        warning.SetActive(true);
    }

    // 모든 팝업창 닫기
    private void HidePopups()
    {
        Popups.SetActive(false);
    }

    // 취소 버튼 눌렀을 때 호출되는 onclick함수
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