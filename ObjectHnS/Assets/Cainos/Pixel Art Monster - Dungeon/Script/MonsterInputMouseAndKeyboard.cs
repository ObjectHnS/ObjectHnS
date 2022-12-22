using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Cainos.PixelArtMonster_Dungeon
{
    //to feed the MonsterController input parameters using mouse and keyboard input
    public class MonsterInputMouseAndKeyboard : MonoBehaviour
    {
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;

        public KeyCode interactionKey = KeyCode.E;
        public KeyCode skillKey = KeyCode.F;
        public KeyCode moveModifierKey = KeyCode.LeftShift;

        public KeyCode attackKey = KeyCode.Mouse0;

        protected MonsterController controller;
        protected MonsterFlyingController controllerFlying;
        protected MonsterSkill monsterSkill;


        private Vector2 inputMove;
        private bool inputMoveModifier;
        
        public bool inputSkill;
        public bool inputInteraction;
        protected bool inputAttack;

        public virtual void Awake()
        {
            controller = GetComponent<MonsterController>();
            controllerFlying = GetComponent<MonsterFlyingController>();
            monsterSkill = GetComponent<MonsterSkill>();
        }

        public virtual void Update()
        {
            bool pointerOverUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();
            if (!pointerOverUI)
            {
                inputInteraction = Input.GetKey(interactionKey);
                inputMoveModifier = Input.GetKey(moveModifierKey);
                inputSkill = Input.GetKeyDown(skillKey);
                inputAttack = Input.GetKeyDown(attackKey);

                if (controller)
                {
                    controller.inputMoveModifier = inputMoveModifier;
                    monsterSkill.inputSkill = inputSkill;
                    controller.inputAttack = inputAttack;
                }
                if (controllerFlying)
                {
                    monsterSkill.inputSkill = inputSkill;
                    controllerFlying.inputAttack = inputAttack;
                }
            }

            //move horizontal
            if (Input.GetKey(leftKey))
            {
                inputMove.x = -1.0f;
            }
            else if (Input.GetKey(rightKey))
            {
                inputMove.x = 1.0f;
            }
            else
            {
                inputMove.x = 0.0f;
            }

            //move vertical
            if (Input.GetKey(downKey))
            {
                inputMove.y = -1.0f;
            }
            else if (Input.GetKey(upKey))
            {
                inputMove.y = 1.0f;
            }
            else
            {
                inputMove.y = 0.0f;
            }

            if (controller) controller.inputMove = inputMove;
            if (controllerFlying) controllerFlying.inputMove = inputMove;
        }
    }
}
