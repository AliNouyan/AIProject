using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThree<T> : State<T>
{
    public DashThree(T stateName, AIThreeController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Player;
    GameObject Self;
    Transform PlayerTransform;
    Transform SelfTransform;
    Vector3 LookDir;
    AIThreeController Brain;

    public override void OnEnter()
    {
        base.OnEnter();
        Player = GameObject.FindGameObjectWithTag("Player");
        Self = this.controller.gameObject;
        PlayerTransform = Player.transform;
        SelfTransform = Self.transform;
        Brain = Self.GetComponent<AIThreeController>();

        LookDir = (SelfTransform.position - PlayerTransform.position);
        //Debug.Log(LookDir);
    }

    public override void Act()
    {
        base.Act();
        //dash at player with speed of 1.3
        controller.transform.Translate(-LookDir * 1.3f * Time.deltaTime);
    }

    public override void OnLeave()
    {
        base.OnLeave();
        //Reset Anger and lower click count
        Brain.Anger = 0;
        Brain.ClickCount = Brain.ClickCount - 20;
    }
}