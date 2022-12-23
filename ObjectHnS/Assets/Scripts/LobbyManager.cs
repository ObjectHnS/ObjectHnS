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

public class LobbyManager : Manager<LobbyManager>
{
    #region Properties
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
    #endregion

    private Dictionary<string, GameObject> myList = new Dictionary<string, GameObject>(); // RoomName / RoomButton Object
    private Dictionary<Player, GameObject> userDict = new Dictionary<Player, GameObject>(); // Player / Player GameObject
    private List<RoomInfo> roomList = new List<RoomInfo>(); // List of rooms
    private bool isEnter = false;

    private GameObject player;

    #region ����
    protected override void Awake()
    {
        base.Awake();

        if (Popups) Popups.SetActive(false);
        if (LoginCanvas) LoginCanvas.SetActive(false);
        if (LobbyCanvas) LobbyCanvas.SetActive(false);
        if (RoomCanvas) RoomCanvas.SetActive(false);

        if (State) State.SetActive(true);
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
                foreach(GameObject buttons in myList.Values.ToArray())
                {
                    buttons.GetComponent<Button>().interactable = false;
                }
                GameObject obj = null;
                if(myList.TryGetValue(PhotonNetwork.CurrentRoom.Name, out obj))
                {
                    if(obj) obj.GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        if (State) State.SetActive(false);
        if (LoginCanvas) LoginCanvas.SetActive(true);
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
                    stateText = "������";
                }
                else if (stateText == "ConnectingToMasterServer")
                {
                    stateText = "������ ������ �����ϴ���";
                }
                else if (stateText == "ConnectedToMasterServer")
                {
                    stateText = "������ ������ �����";
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

    private void showQuit(string msg)
    {
        Popups.SetActive(true);
        var obj = Popups.transform.Find("Quit").gameObject;
        obj.SetActive(true);
        obj.transform.Find("Title").GetComponent<Text>().text = msg;
        this.Invoke(() => { Application.Quit(); }, 1.5f);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        showQuit(cause.ToString());
    }

    #endregion

    #region �� ����
    private List<Player> playerList = new List<Player>();

    // �� ���� ���н�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    // �� ���� ���н�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
    }

    // ������ ���� ���н�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ShowWarningPopupMessage("���� �����ϴ�!\n ���� ������ּ���!");
        Debug.Log(returnCode + " : " + message);
    }

    // �濡 ������ �÷��̾� ����Ʈ ����
    private void UpdateUserList()
    {
        if (isEnter) // ���� �� ��ư�� ���� ���Դٸ�
        {
            playerList = new List<Player>(PhotonNetwork.CurrentRoom.Players.Values);

            if(!PhotonNetwork.IsMasterClient)
            {
                RoomCanvas.transform.Find("StartButton").GetComponent<Button>().interactable = false;
            }
            else RoomCanvas.transform.Find("StartButton").GetComponent<Button>().interactable = true;

            foreach (Player p in playerList.ToArray()) // ���� ���� ��� üũ�ϰ� ī�� ����
            {
                if (!userDict.Keys.Contains(p)) // ���� �÷��̾� ����Ʈ �߰� / ���� ��ųʸ��� �÷��̾ �������� �ʴ´ٸ�
                {    
                    // ���ο� ���� ī�� ����
                    var card = Instantiate(UserCard, RoomCanvas.transform.Find("Userboard").Find("Grid"));
                    card.transform.Find("UserName").GetComponent<Text>().text = p.NickName; // ���� �̸�
                    card.name = p.NickName; // ���ӿ�����Ʈ
                    if (p.NickName == PhotonNetwork.MasterClient.NickName) // ���� �����̶��
                    {
                        card.transform.Find("UserName").GetComponent<Text>().color = Color.magenta;
                    }
                    userDict.Add(p, card);
                }
                else
                {
                    GameObject card = null;
                    if(userDict.TryGetValue(PhotonNetwork.MasterClient, out card))
                    {
                        if(card) card.transform.Find("UserName").GetComponent<Text>().color = Color.magenta;
                    }
                }
            }
            foreach (var p in userDict.Keys.ToArray()) //���� ��� üũ
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

    // '������'��ư�� ������ �� ȣ��Ǵ� �Լ�
    public void LeaveRoom()
    {
        // ���� ī�� ����
        foreach (var p in PhotonNetwork.CurrentRoom.Players.Values.ToArray())
        {
            GameObject t = null;
            userDict.TryGetValue(p, out t);
            Destroy(t);
        }
        FetchRoomList(); // �� ����Ʈ ���ΰ�ħ
        if(PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom(); // �� ������
    }

    // ���� ������ �� ȣ��Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        RoomCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
        isEnter = false;
    }

    // ���ο� �� ��ư�� ����� �Լ�
    private GameObject CreateRoomButton(string roomName)
    {
        roomButton.GetComponent<RoomButton>().Name = roomName;
        var obj = Instantiate(roomButton, roomGrid.transform); 
        obj.name = roomName;
        return obj;
    }

    // ���ο� ���� ����� onclick�Լ�
    public void CreateRoom()
    {
        InputField roomName = roomCreation.transform.Find("RoomName").gameObject.GetComponent<InputField>();
        if (roomName.text == "")
        {
            ShowWarningPopupMessage("�� �̸��� ����ֽ��ϴ�!");
            return;
        }
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 8 });

        HidePopups();

        myList.Add(roomName.text, CreateRoomButton(roomName.text));
    }

    // ���� �濡 �������� �� ȣ��Ǵ� �Լ�
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // �濡 �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        if (RoomCanvas) RoomCanvas.SetActive(true);
        userDict = new Dictionary<Player, GameObject>();
        isEnter = true;
    }

    // ���� ��ư�� ������ �� ȣ��Ǵ� �Լ�
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
            isEnter = false;
        }
    }

    // �� ����Ʈ�� ������Ʈ �Ǿ��� �� ȣ��Ǵ� �Լ�
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

    #region �˾�â

    // �� �� ����� �˾�â ����
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

    // ��� �޼��� �˾�â ����
    private void ShowWarningPopupMessage(string message)
    {
        Popups.SetActive(true);
        if(!roomCreation) roomCreation = Popups.transform.Find("RoomCreation").gameObject;
        roomCreation.SetActive(false);

        if (!warning) warning = Popups.transform.Find("Warning").gameObject;
        warning.transform.Find("Message").GetComponent<Text>().text = message;
        warning.SetActive(true);
    }

    // ��� �˾�â �ݱ�
    private void HidePopups()
    {
        Popups.SetActive(false);
    }

    // ��� ��ư ������ �� ȣ��Ǵ� onclick�Լ�
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