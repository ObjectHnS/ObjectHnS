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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ghost")
        {
            if (photonView.IsMine)
            {
                photonView.RPC("GetBrokenKey", RpcTarget.All);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }
    [PunRPC] void GetBrokenKey()
    {
        GameManager.Instance.BrokenKeyCount++;
    }
}
