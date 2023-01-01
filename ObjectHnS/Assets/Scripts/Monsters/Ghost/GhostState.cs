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
    float transformTime = 0;
    float heistTime = 0;
    [SerializeField] Sprite[] transObj;

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
    public void AddDeadCount()
    {
        GameManager.Instance.OverCount++;
    }

    [PunRPC]
    public void Damaged(int value)
    {
        if (photonView.IsMine)
        {
            GetComponent<PixelMonster>().InjuredBack();
            hp -= value;
            if(hp <= 0)
            {
                photonView.RPC("AddDeadCount", RpcTarget.All);
                this.Invoke(() => 
                {
                    var ghosts = GameObject.FindGameObjectsWithTag("Ghost");
                    transform.Find("Camera").parent = ghosts[Random.Range(0, ghosts.Length)].transform;

                    UIManager.Instance.ghostUI.SetActive(false);
                    PhotonNetwork.Destroy(this.gameObject);
                }, 0.5f);
            }
        }
    }


    private bool isDead = false;
    private void Update()
    {
        uptime += Time.deltaTime;
        if (uptime >= float.MaxValue-1) uptime = 0;

        if (hp <= 0 && isDead == false)
        {
            isDead = true;
            var ghosts = FindObjectsOfType<GhostState>();
            transform.Find("Camera").parent = ghosts[0].gameObject.transform.root;

            GetComponentInChildren<Animator>().SetBool("IsDead", isDead);
            GetComponent<MonsterInputJoystick>().enabled = false;
            UIManager.Instance.ghostUI.gameObject.SetActive(false);
        }
    }

    IEnumerator co_Transform(int item)
    {
        yield return null;
        transform.Find("Animator").gameObject.SetActive(false);
        transform.Find("Transform").gameObject.SetActive(true);
        transform.Find("Transform").GetComponent<SpriteRenderer>()
            .sprite = transObj[item];
        yield return new WaitForSeconds(15);
        transform.Find("Animator").gameObject.SetActive(true);
        transform.Find("Transform").gameObject.SetActive(false);
    }

    [PunRPC]
    public void TransformRPC(int item)
    {
        if ((uptime - transformTime >= 25) || (transformTime == 0))
        {
            StartCoroutine(co_Transform(item));
            transformTime = uptime;
        }
    }

    public void Transformation()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("TransformRPC", RpcTarget.All, Random.Range(0, transObj.Length));
        }
    }
    
    IEnumerator co_Heist()
    {
        yield return null;
        GetComponent<MonsterFlyingController>().speedMax = 4;
        yield return new WaitForSeconds(3f);
        GetComponent<MonsterFlyingController>().speedMax = 2.25f;
    }

    public void Heist()
    {
        if ((uptime - heistTime >= 15) || (heistTime == 0))
        {
            if (photonView.IsMine)
            {
                Vector3 cur = transform.position;
                GameObject a = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "GhostSkill"), new Vector3(cur.x, cur.y, -5), Quaternion.identity);
                a.transform.parent = gameObject.transform;
            }
            StartCoroutine(co_Heist());
            heistTime = uptime;
        }
    }
}
