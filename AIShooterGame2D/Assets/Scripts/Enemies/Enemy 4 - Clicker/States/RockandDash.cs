using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockandDash<T> : State<T>
{
    public RockandDash(T stateName, AIFourController controller, float minDuration) : base(stateName, controller, minDuration) { }
    
    GameObject Player;
    GameObject Self;
    Transform PlayerTransform;
    Transform SelfTransform;
    Vector3 LookDir;
    AIFourController Brain;

    bool GetDashDir = true;

    public override void OnEnter()
    {
        base.OnEnter();
        Self = this.controller.gameObject;
        SelfTransform = Self.transform;
        Brain = Self.GetComponent<AIFourController>();
        Brain.CheckCol = true;

        //instante Rock, which has its own moving script
        MonoBehaviour.Instantiate(Brain.Rock, SelfTransform.position, SelfTransform.rotation);
    }

    public override void Act()
    {
        base.Act();
        //Dash becomes true once the rock has reached its location
        if(Brain.Dash == true)
        {
            if(GetDashDir == true)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
                PlayerTransform = Player.transform;
                LookDir = (SelfTransform.position - PlayerTransform.position);
                GetDashDir = false;
            }
            //after getting direction (if required) then dash at player
            controller.transform.Translate(-LookDir * 1.5f * Time.deltaTime);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
        Brain.AttackCount++;
        Brain.Attack = false;
        Brain.Dash = false;

    }
}