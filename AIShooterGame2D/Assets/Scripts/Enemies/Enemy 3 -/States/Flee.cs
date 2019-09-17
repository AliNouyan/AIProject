using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee<T> : State<T>
{
    public Flee(T stateName, AIThreeController controller, float minDuration) : base(stateName, controller, minDuration) { }

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
    }

    public override void Act()
    {
        base.Act();
        //move in opposite direction of player at 0.5f speed
        LookDir = (SelfTransform.position - PlayerTransform.position);
        controller.transform.Translate(LookDir * 0.5f * Time.deltaTime);
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}