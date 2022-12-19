using Cainos.PixelArtMonster_Dungeon;
using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterInputJoystick : MonsterInputMouseAndKeyboard
{
    public FloatingJoystick joystick;


    private IEnumerator co_Attack()
    {
        inputAttack = true;
        yield return new WaitForEndOfFrame();
        inputAttack = false;
        yield return new WaitForSeconds(2.0f);
    }

    public void Attack()
    {
        StartCoroutine(co_Attack());
    }

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(controllerFlying) controllerFlying.inputAttack = inputAttack;
        if (controller) controller.inputMove = joystick.Direction;
        if (controllerFlying) controllerFlying.inputMove = joystick.Direction;
    }
}
