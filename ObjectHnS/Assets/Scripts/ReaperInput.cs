using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperInput : MonoBehaviour
{
    public bool inputSkill;
    public bool inputAttack;
    public bool inputInteraction;

    void Skill()
    {
        inputSkill = true;
    }

    void Attack()
    {
        inputAttack = true;
    }

    void Interaction()
    {
        inputInteraction = true;
    }
}
