using Cainos.PixelArtMonster_Dungeon;
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

    float uptime = 5;
    float useTime = 0;
    [SerializeField] Sprite[] transObj;

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
        photonView.OwnershipTransfer = OwnershipOption.Fixed;
    }

    private int randomObj;

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
        if (photonView.IsMine)
        {
            GetComponent<PixelMonster>().InjuredBack();
            hp -= value;
            if (hp <= 0)
            {
                GetComponent<PixelMonster>().OnDieFx();
                this.Invoke(() => { PhotonNetwork.Destroy(gameObject); }, 1.8f);
            }
        }
    }

    private void Update()
    {
        uptime += Time.deltaTime;
        if (uptime >= float.MaxValue-1) uptime = 0;
    }

    IEnumerator co_Transform(int item)
    {
        yield return null;
        transform.Find("Animator").gameObject.SetActive(false);
        transform.Find("Transform").gameObject.SetActive(true);
        transform.Find("Transform").GetComponent<SpriteRenderer>().sprite
            = transObj[item];
        yield return new WaitForSeconds(15);
        transform.Find("Animator").gameObject.SetActive(true);
        transform.Find("Transform").gameObject.SetActive(false);
    }

    [PunRPC]
    public void TransformRPC(int item)
    {
        if ((uptime - useTime >= 25) || (useTime == 0))
        {
            StartCoroutine(co_Transform(item));
            useTime = uptime;
        }
    }

    public void Transformation()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("TransformRPC", RpcTarget.All, Random.Range(0, transObj.Length));
        }
    }
    
    public void Heist()
    {
        if (photonView.IsMine)
        {
            GetComponent<MonsterFlyingController>().speedMax = 5;
        }
    }
}
