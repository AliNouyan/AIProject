using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFour<T> : State<T>
{
    public DashFour(T stateName, AIFourController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Player;
    GameObject Self;
    Transform PlayerTransform;
    Transform SelfTransform;
    Vector3 LookDir;
    AIFourController Brain;

    public override void OnEnter()
    {
        base.OnEnter();
        Player = GameObject.FindGameObjectWithTag("Player");
        Self = this.controller.gameObject;
        PlayerTransform = Player.transform;
        SelfTransform = Self.transform;
        Brain = Self.GetComponent<AIFourController>();

        LookDir = (SelfTransform.position - PlayerTransform.position);
        //Debug.Log(LookDir);

        Brain.CheckCol = true;
    }

    public override void Act()
    {
        base.Act();
        //dash towards player at 1.5 speed
        controller.transform.Translate(-LookDir * 1.5f * Time.deltaTime);
    }

    public override void OnLeave()
    {
        base.OnLeave();
        //increase attack count by 1 and make bool false
        Brain.AttackCount++;
        Brain.Attack = false;
    }
}