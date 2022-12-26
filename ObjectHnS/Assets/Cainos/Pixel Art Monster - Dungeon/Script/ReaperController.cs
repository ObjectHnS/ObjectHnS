using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;
using Photon.Realtime;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ReaperController : MonsterFlyingController
{
    public ReaperButtonInput input;

    private PhotonView pv;

    public float time;
    public float coolTime = 3f;

    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<ReaperButtonInput>();
        pv = GetComponent<PhotonView>();
    }
    protected override void Update()
    {
        base.Update();
        time += Time.deltaTime;
    }
    protected override void Attack(bool inputAttack)
    {
        inputAttack = input.inputAttack;
        if (time >= coolTime && inputAttack)
        {
            this.pm.Attack();
            if (pv.IsMine)
            {
                Vector3 cur = transform.position;
                GameObject a = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "HitBox_Reaper"), new Vector3(GetComponent<PixelMonster>().Facing + cur.x, cur.y + 1f), Quaternion.identity);
                a.transform.parent = gameObject.transform;
            }
            time = 0;
        }
    }
}
