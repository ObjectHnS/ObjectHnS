using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : MonoBehaviour
{
    public int RepaperHp;
    private CapsuleCollider2D capsulecollider;
    void Awake()
    {
        capsulecollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if(RepaperHp < 0)
        {
            Destroy(gameObject);
        }
    }

    
}