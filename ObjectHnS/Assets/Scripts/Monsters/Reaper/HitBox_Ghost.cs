    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Ghost : MonoBehaviour
{
    public float damage { private get; set; }

    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.CompareTag(transform.tag) && collision.gameObject.GetComponent<>() != null)
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
