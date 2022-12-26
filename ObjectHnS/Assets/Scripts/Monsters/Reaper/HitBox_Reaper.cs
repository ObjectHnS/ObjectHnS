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
        if (collision.tag == "Ghost")
        {
            Debug.Log(gameObject.GetComponentInParent<Reaper>());
            collision.GetComponent<GhostState>().photonView.RPC("Damaged", RpcTarget.All, gameObject.GetComponentInParent<Reaper>().reaperAttack);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
