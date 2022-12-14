using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
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

    [Header("etc..")]
    public PhotonView pv;

    private GameObject roomCreation;
    private GameObject warning;

    private Dictionary<string, GameObject> myList = new Dictionary<string, GameObject>();

    #region 포톤
    private void Awake()
    {
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
    }

    public override void OnConnectedToMaster()
    {
        if (State) State.SetActive(false);
        if (LoginCanvas) LoginCanvas.SetActive(true);

        PhotonNetwork.JoinLobby();
    }

    private void UpateState()
    {
        if(State)
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
                    
    public void Connect()
    {
        if (LoginCanvas)
        {
            PhotonNetwork.LocalPlayer.NickName = LoginCanvas.transform.GetChild(2).GetComponent<InputField>().text;
            Debug.Log(PhotonNetwork.LocalPlayer.NickName);

            LoginCanvas.SetActive(false);
            if (LobbyCanvas) LobbyCanvas.SetActive(true);
        }
        else PhotonNetwork.LocalPlayer.NickName = "Player" + Random.Range(1, 100);
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
    }


    private GameObject CreateRoomButton(string roomName)
    {
        roomButton.GetComponent<RoomButton>().Name = roomName;
        return Instantiate(roomButton, roomGrid.transform);
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
        Popups.SetActive(true);
        if(warning == null)
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
        if(RoomCanvas) RoomCanvas.SetActive(true);
        print("joined " + PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.Instantiate("FP_Ghost_Blue", Vector3.zero, Quaternion.identity);
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
