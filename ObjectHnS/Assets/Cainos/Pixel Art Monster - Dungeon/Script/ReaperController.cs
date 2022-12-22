using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;

public class ReaperController : MonsterFlyingController
{

    public float time;
    public float coolTime = 3f;

    void Update()
    {
        base.Update();
        time += Time.deltaTime;
    }
    protected override void Attack(bool inputAttack)
    {
        if(time >= coolTime && inputAttack)
        {
            this.pm.Attack();
            time = 0;
        }
    }
}
