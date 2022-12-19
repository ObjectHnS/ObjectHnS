using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Cainos.PixelArtMonster_Dungeon
{
    //to feed the MonsterController input parameters using mouse and keyboard input
    public class MonsterInputMouseAndKeyboard : MonoBehaviour
    {
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;

        public KeyCode skillKey;
        public KeyCode interactionKey;
        public KeyCode moveModifierKey;

        public KeyCode attackKey = KeyCode.Mouse0;

        protected MonsterFlyingController controllerFlying;

        private Vector2 inputMove;
        private bool inputMoveModifier;
        private bool inputInteraction;
        protected bool inputAttack;

        public virtual void Awake()
        {
            controllerFlying = GetComponent<MonsterFlyingController>();
        }

        public virtual void Update()
        {
            bool pointerOverUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();
            if (!pointerOverUI)
            {
                inputMoveModifier = Input.GetKey(moveModifierKey);
                inputAttack = Input.GetKeyDown(attackKey);
                inputInteraction = Input.GetKeyDown(interactionKey);

                if (controllerFlying)
                {
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

            if (controllerFlying) controllerFlying.inputMove = inputMove;
        }
    }
}
