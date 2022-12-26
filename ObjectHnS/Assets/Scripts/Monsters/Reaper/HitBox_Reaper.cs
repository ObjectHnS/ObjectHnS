using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Reaper : MonoBehaviour
{
    public Reaper reaper;
    public float damage { private get; set; }

    void Start()
    {
        reaper = GetComponent<Reaper>();
        StartCoroutine(Timer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ghost")
        {
            //collision.GetComponent<>()
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
