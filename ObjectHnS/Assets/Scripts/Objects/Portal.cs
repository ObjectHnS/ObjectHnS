using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    Reaper reaper;
    GhostState ghost;


    void Start()
    {
        reaper = GetComponent<Reaper>();
        ghost = GetComponent<GhostState>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            GameManager.Instance.isExit = true;
            GameManager.Instance.OverCount++;
        }
    }
}
