using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    public bool inputSkill = false;
    private static float coolTime = 3f;
    private static float time;

    public Animator animator;

    void Update()
    {
        CharacterSkill();
        time += Time.deltaTime;
    }

    void CharacterSkill()
    {
        if (inputSkill && time >= coolTime)
        {
            animator.SetTrigger("canSkill");
            time = 0f;
        }
    }
}
