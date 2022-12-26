using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class HitBox_Reaper : MonoBehaviour
{
    private PhotonView photonView;
    public float damage { private get; set; }

    void OnEnable()
    {
        photonView = GetComponentInParent<PhotonView>();
        StartCoroutine(Timer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReaperSkill_Blue skill = null;
        if (collision.tag == "Ghost")
        {
            if(skill == null)
            {
                skill = gameObject.GetComponentInParent<ReaperSkill_Blue>();
            }
            if(skill.skillCount < 4)
            {
                skill.skillCount++;
            }
            collision.GetComponent<GhostState>().photonView.RPC("Damaged", RpcTarget.All, gameObject.GetComponentInParent<Reaper>().reaperAttack);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
