using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class ExitKey : MonoBehaviourPun
{
    private void Awake()
    {
        photonView.OwnershipTransfer = OwnershipOption.Takeover;
    }

    [PunRPC] void GetBrokenKey()
    {
        GameManager.Instance.BrokenKeyCount++;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Ghost")
        {
            if (photonView.IsMine)
            {
                photonView.RPC("GetBrokenKey", RpcTarget.All);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
