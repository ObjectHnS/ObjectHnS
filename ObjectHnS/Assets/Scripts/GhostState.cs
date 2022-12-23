using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostState : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
