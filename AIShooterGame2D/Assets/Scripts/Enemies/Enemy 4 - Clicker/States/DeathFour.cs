using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFour<T> : State<T>
{
    public DeathFour(T stateName, AIFourController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Player;
    GameObject Self;
    Transform PlayerTransform;
    Transform SelfTransform;
    Vector3 LookDir;
    AIFourController Brain;

    bool GetDashDir = true;

    public override void OnEnter()
    {
        //First part of script is taken from the RockAndDash Script as i thought it would
        //Be easier to just do this than to change the FSM around to make it do RockandDash twice
        //then on the second one to go to death
        base.OnEnter();
        Self = this.controller.gameObject;
        SelfTransform = Self.transform;
        Brain = Self.GetComponent<AIFourController>();
        Brain.CheckCol = true;

        MonoBehaviour.Instantiate(Brain.Rock, SelfTransform.position, SelfTransform.rotation);
    }

    public override void Act()
    {
        base.Act();
        if (Brain.Dash == true)
        {
            if (GetDashDir == true)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
                PlayerTransform = Player.transform;
                LookDir = (SelfTransform.position - PlayerTransform.position);
                GetDashDir = false;
            }

            controller.transform.Translate(-LookDir * 1.5f * Time.deltaTime);
        }

        if(Brain.DeathBool == true)
        {
            this.controller.GetComponent<AIFourController>().Death();
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}