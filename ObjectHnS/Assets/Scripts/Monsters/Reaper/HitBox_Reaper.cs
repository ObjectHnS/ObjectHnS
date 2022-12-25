using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Reaper : MonoBehaviour
{
    public float damage { private get; set; }

    void Start()
    {
        StartCoroutine(Timer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fugitive")
        {
            
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
