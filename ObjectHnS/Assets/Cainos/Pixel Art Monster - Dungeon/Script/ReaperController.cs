using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;

public class ReaperController : MonsterFlyingController
{
    public ReaperButtonInput input;

    public float time;
    public float coolTime = 3f;

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<ReaperButtonInput>();
    }
    protected override void Update()
    {
        base.Update();
        time += Time.deltaTime;
    }
    protected override void Attack(bool inputAttack)
    {
        inputAttack = input.inputAttack;
        if(time >= coolTime && inputAttack)
        {
            this.pm.Attack();
            time = 0;
        }
    }
}
