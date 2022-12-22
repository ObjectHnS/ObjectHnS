using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;

public class Reaper : MonoBehaviour
{
    public int ReaperHp = 1500;
    private bool isDead = false;

    public PixelMonster pm;
    private CapsuleCollider2D capsulecollider;
    private MonsterInputMouseAndKeyboard inputMouseButton;
    protected ReaperSkill_Blue RsBlue;
    protected ReaperSkill_White RsWhite;
    void Awake()
    {
        capsulecollider = GetComponent<CapsuleCollider2D>();
        inputMouseButton = GetComponent<MonsterInputMouseAndKeyboard>();
        RsBlue = GetComponent<ReaperSkill_Blue>();
        RsWhite = GetComponent<ReaperSkill_White>();
    }
    

    void Update()
    {

        if(ReaperHp <= 0 && isDead == false)
        {
            pm.OnDieFx();
            isDead = true;
        }

        if (isDead)
        {
            Destroy(gameObject);
        }
    }
}