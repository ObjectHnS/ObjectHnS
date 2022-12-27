using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Manager<UIManager>
{
    public Button[] reaperButton; 

    public bool useCountdown = false;
    public float countdownTime = 5f;

    private bool isStarted;
    public bool IsStarted
    {
        get
        {
            return isStarted;
        }
    }

    public Text timer;
    public GameObject ghostUI;
    public GameObject reaperUI;
    public Sprite completeKey;

    private float curtime = 0f;

    private Hashtable customProperties;
    private string playerKind = "";

    private void Start()
    {
        isStarted = false;
        countdownTime++;
    }

    private bool isSetUI = false;
    private void Update()
    {
            Countdown(useCountdown);
            SetPlayerUI();
            UpdateCount();
            BindButton();
    }

    void Countdown(bool isUse)
    {
        if (isUse)
        {
            if (curtime < countdownTime - 1)
            {
                curtime += Time.deltaTime;
                timer.text = ((int)(countdownTime - curtime)).ToString();
            }
            else if (!isStarted)
            {
                timer.gameObject.SetActive(false);
                isStarted = true;
            }
        }
        else
        {
            if(!isStarted) isStarted = true;
        }
    }

    void SetPlayerUI()
    {
        if (GameManager.Instance.IsPlayerCreated && !isSetUI)
        {
            if(PhotonNetwork.LocalPlayer.CustomProperties["isReaper"] != null)
            {
                if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["isReaper"])
                {
                    reaperUI.SetActive(true);
                    playerKind = "Reaper";
                }
                else
                {
                    ghostUI.SetActive(true);
                    playerKind = "Ghost";
                }
                isSetUI = true;
            }
        }
    }

    private bool isCompleteKey = false;
    void UpdateCount()
    {
        Text countUI = null;
        if(isSetUI)
        {
            if(playerKind == "Ghost")
            {
                if (countUI == null) countUI = ghostUI.transform.Find("KeyCount").Find("Count").GetComponent<Text>();
                int count = GameManager.Instance.BrokenKeyCount;
                countUI.text = count.ToString() + " / 4";
                if (count == 4 && !isCompleteKey)
                {
                    ghostUI.transform.Find("KeyCount").Find("Image").GetComponent<Image>().sprite = completeKey;
                    isCompleteKey = true;
                }
            }
            else
            {
                if (countUI == null) countUI = reaperUI.transform.Find("SoulStack").Find("Count").GetComponent<Text>();
                int count = GameManager.Instance.Player.GetComponent<ReaperSkill_Blue>().skillCount;
                countUI.text = count.ToString() + " / 4";
            }
        }
    }
    private bool isBinded = false;
    void BindButton()
    {
        if(playerKind == "Reaper" && !isBinded && GameManager.Instance.Player != null)
        {
            reaperButton[0].onClick.AddListener(GameManager.Instance.Player.GetComponent<ReaperButtonInput>().Attack);
            reaperButton[1].onClick.AddListener(GameManager.Instance.Player.GetComponent<ReaperButtonInput>().Skill);

            isBinded = true;
        }
    }
}
