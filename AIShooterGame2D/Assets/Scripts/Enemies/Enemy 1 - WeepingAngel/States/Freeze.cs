using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze<T> : State<T>
{
    public Freeze(T stateName, AIOneController controller, float minDuration) : base(stateName, controller, minDuration) { }


    public override void OnEnter()
    {
        base.OnEnter();
        GameObject Self = this.controller.gameObject;
        var MyScript = Self.GetComponent<AIOneController>();
        controller.StartCoroutine(MyScript.DashTimer());
        //Call timer, if still frozen by the time the timer goes off ai will dash
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}