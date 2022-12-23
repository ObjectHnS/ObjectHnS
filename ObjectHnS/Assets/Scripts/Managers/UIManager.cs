using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class UIManager : Manager<UIManager>
{
    public float TimerSec = 5f;
    public bool isStartedGame;

    public Text text;
    public GameObject ghostUI;
    public GameObject reaperUI;

    private float curtime = 0f;

    private void Start()
    {
        isStartedGame = false;
    }

    private void Update()
    {
        RunTimer();
    }

    void RunTimer()
    {
        if (curtime <= TimerSec)
        {
            curtime += Time.deltaTime;
            text.text = ((int)(TimerSec - curtime)).ToString();
        }
        else if(!isStartedGame)
        {
            text.gameObject.SetActive(false);
            isStartedGame = true;
        }
    }
}
