using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;
using Photon.Realtime;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ReaperSkill_Blue : MonoBehaviourPun
{
    public bool skillRetention = false;
    public float skillTime;
    public float skillCoolTime = 20f;
    public float skillRetentionTime = 5f;
    public int skillCount = 0;

    public Animator animator;
    ReaperController reaper;
    ReaperButtonInput Input;
    Reaper reaperStat;

    void Start()
    {
        reaper = GetComponent<ReaperController>();
        Input = GetComponent<ReaperButtonInput>();
        reaperStat = GetComponent<Reaper>();
    }

    void Update()
    {
        skillTime += Time.deltaTime;

        if (Input.inputSkill && skillCoolTime <= skillTime && skillCount > 0)
        {
            Skill();
        }

        if(skillTime >= skillRetentionTime && skillRetention)
        {
            reaperStat.reaperAttack = 50;
            skillTime = 0;
            reaper.speedMax = 2;
            reaper.acc = 8;
            reaper.brakeAcc = 8;
            skillRetention = false;
        }
    }
    public void Skill()
    {
        skillCount -= 1;
        animator.SetTrigger("canSkill");
        skillTime = 0;
        reaper.speedMax = 10;
        reaper.acc = 10;
        reaper.brakeAcc = 10;
        reaperStat.reaperAttack = 75;
        skillRetention = true;
    }
}
