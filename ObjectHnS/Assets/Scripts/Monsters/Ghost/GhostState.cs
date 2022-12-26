using Photon.Pun;
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

    [PunRPC]
    private void Damaged(int value)
    {
        if(hp > value)
        {
            hp -= value;
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
