using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;

public class ReaperSkill_White : MonoBehaviour
{
    MonsterSkill monsterSkill;
    void Start()
    {
        monsterSkill = GetComponent<MonsterSkill>();
    }

    public void Passive()
    {

    }

    public void Skill()
    {
        monsterSkill.CharacterSkill();
    }
}
