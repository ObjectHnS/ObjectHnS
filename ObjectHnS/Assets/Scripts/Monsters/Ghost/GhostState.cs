using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GhostState : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private int hp = 150;
    public int Hp
    {
        get
        {
            return hp;
        }
    }

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
        photonView.OwnershipTransfer = OwnershipOption.Fixed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
        }
        else
        {
            hp = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void Damaged(int value)
    {
        if(photonView.IsMine)
        {
            hp -= value;
            if(hp <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
