using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Invector.CharacterController
{
    public class vThirdPersonAttack : vThirdPersonMotor
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("atack");
        }

        private void NormalAttack()
        {
            if(Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("AttackTrigger");
                animator.SetInteger("Action", 4);
            }
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetTrigger("AttackTrigger");
                animator.SetInteger("Action", 5);
            }
        }
    }
}

