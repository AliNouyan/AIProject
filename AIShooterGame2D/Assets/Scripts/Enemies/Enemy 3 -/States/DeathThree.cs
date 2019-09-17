using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathThree<T> : State<T>
{
    public DeathThree(T stateName, AIThreeController controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void Act()
    {
        base.Act();
        this.controller.GetComponent<AIThreeController>().Death();
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}