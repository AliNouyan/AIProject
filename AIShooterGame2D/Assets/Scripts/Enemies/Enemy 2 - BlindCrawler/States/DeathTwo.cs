using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTwo<T> : State<T>
{
    public DeathTwo(T stateName, AITwoController controller, float minDuration) : base(stateName, controller, minDuration) { }


    public override void OnEnter()
    {
        base.OnEnter();
        this.controller.GetComponent<AITwoController>().Death();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}