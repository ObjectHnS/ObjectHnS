using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostState : MonoBehaviourPun, IPunObservable
{

    private int hp = 150;
    public int Hp
    {
        get 
        {
            return hp;
        }
    }

    public CircleCollider2D collider;
    private PhotonView pv;

    private void Awake()
    {
        pv = PhotonView.Get(this);
    }
    private void Update()
    {
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(hp);
        }
        else
        {
            hp = (int)stream.ReceiveNext();
        }
    }
}
