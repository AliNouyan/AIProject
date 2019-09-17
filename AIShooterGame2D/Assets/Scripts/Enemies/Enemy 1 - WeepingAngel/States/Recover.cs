using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recover<T> : State<T>
{
    public Recover(T stateName, AIOneController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Self;

    public override void OnEnter()
    {
        base.OnEnter();
        Self = this.controller.gameObject;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        var SelfController = Self.GetComponent<AIOneController>();
        SelfController.Recover = false;
    }
}