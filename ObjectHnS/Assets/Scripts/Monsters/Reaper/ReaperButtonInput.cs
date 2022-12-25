using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperButtonInput : MonoBehaviour
{
    public bool inputSkill;
    public bool inputAttack;
    public bool inputInteraction;

    public void Skill()
    {
        StartCoroutine(SkillCorutine());
    }

    public void Attack()
    {
        StartCoroutine(AttackCorutine());
    }

    public void Interaction()
    {
        StartCoroutine(InteractionCorutine());
    }

    IEnumerator SkillCorutine()
    {
        inputSkill = true;
        yield return new WaitForEndOfFrame();

        inputSkill = false;
        yield return new WaitForSeconds(3);
    }

    IEnumerator AttackCorutine()
    {
        inputAttack = true;
        yield return new WaitForEndOfFrame();
        inputAttack = false;
        yield return new WaitForSeconds(3);
    }

    IEnumerator InteractionCorutine()
    {
        inputInteraction = true;
        yield return new WaitForEndOfFrame();
        inputInteraction = false;
        yield return new WaitForSeconds(3);
    }
}
