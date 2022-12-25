using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private void Awake()
    {
        if(photonView.IsMine)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PF_BrokenKey"), new Vector3(2, 2, 0), Quaternion.identity);
        }
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
