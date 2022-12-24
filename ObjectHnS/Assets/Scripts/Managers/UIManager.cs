using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public class UIManager : Manager<UIManager>
{
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

    public Text text;
    public GameObject ghostUI;
    public GameObject reaperUI;

    private float curtime = 0f;

    private void Start()
    {
        isStarted = false;
        countdownTime++;
    }

    private void Update()
    {
        Countdown(useCountdown);
        if(GameManager.Instance.IsPlayerCreated)
        {
            Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            if ((bool)playerProperties["isReaper"])
            {
                reaperUI.SetActive(true);
            }
            else
            {
                ghostUI.SetActive(true);
            }
        }
    }

    void Countdown(bool isUse)
    {
        if (isUse)
        {
            if (curtime < countdownTime - 1)
            {
                curtime += Time.deltaTime;
                text.text = ((int)(countdownTime - curtime)).ToString();
            }
            else if (!isStarted)
            {
                text.gameObject.SetActive(false);
                isStarted = true;
            }
        }
        else
        {
            if(!isStarted) isStarted = true;
        }
    }
}
