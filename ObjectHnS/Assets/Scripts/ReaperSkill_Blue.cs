using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;
using Photon.Realtime;
using Photon.Pun;

public class ReaperSkill_Blue : MonoBehaviour
{
    public float skillTime;
    public float skillCoolTime = 5f;
    public GameObject bat;
    public float passiveTime;
    public float passiveCoolTime = 5f;

    MonsterSkill monsterSkill;

    void Start()
    {
        monsterSkill = GetComponent<MonsterSkill>();
    }

    void Update()
    {
        skillTime += Time.deltaTime;
        passiveTime += Time.deltaTime;

        if(passiveCoolTime <= passiveTime)
        {
            Passive();
        }
    }
    public void Passive()
    {
        passiveTime = 0;
        for(int i = 0; i < /*PhotonNetwork.CurrentRoom.PlayerCount - 1*/ 7; i++)
        {
            //GameObject a = PhotonNetwork.Instantiate("PF_Bat",GetComponent<Reaper>().gameObject.transform.position,Quaternion.identity);
            Instantiate(bat);
        }
    }

    public void Skill()
    {
        if (skillTime >= skillCoolTime)
        {
            skillTime = 0;
            monsterSkill.CharacterSkill();
            
        }
    }
}
