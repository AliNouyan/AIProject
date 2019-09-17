using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listen<T> : State<T>
{
    public Listen(T stateName, AITwoController controller, float minDuration) : base(stateName, controller, minDuration) { }


    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}