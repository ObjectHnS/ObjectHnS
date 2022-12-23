using Cainos.PixelArtMonster_Dungeon;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterInputJoystick : MonsterInputMouseAndKeyboard
{
    private FloatingJoystick joystick;
    private PhotonView pv;

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
        joystick = GameObject.Find("Joystick").GetComponent<FloatingJoystick>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(pv.IsMine)
        {
            if (controllerFlying) controllerFlying.inputMove = joystick.Direction;
            if (controllerFlying) controllerFlying.inputAttack = inputAttack;
            if (controller) controller.inputMove = joystick.Direction;
        }
    }
}
