using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtMonster_Dungeon;

public class Reaper : MonoBehaviour
{
    public int reaperHp = 1500;
    public int reaperAttack = 50;
    private bool isDead = false;

    private PixelMonster pm;
    private CapsuleCollider2D capsulecollider;
    private MonsterInputMouseAndKeyboard inputMouseButton;
    protected ReaperSkill_Blue RsBlue;
    void Awake()
    {
        pm = GetComponent<PixelMonster>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        inputMouseButton = GetComponent<MonsterInputMouseAndKeyboard>();
        RsBlue = GetComponent<ReaperSkill_Blue>();
    }
    

    void Update()
    {

        if(reaperHp <= 0 && isDead == false)
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