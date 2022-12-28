using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviourPun
{
    [PunRPC]
    void NoticeGameEnd()
    {
        PhotonNetwork.LoadLevel("Ending");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ghost"))
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values.ToArray())
            {
                player.CustomProperties.Add("isWin", true);
            }
            PhotonNetwork.MasterClient.CustomProperties["isWin"] = false;
            photonView.RPC("NoticeGameEnd", RpcTarget.MasterClient);
        }
    }
}
