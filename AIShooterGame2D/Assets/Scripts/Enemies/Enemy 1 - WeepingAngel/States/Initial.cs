using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initial<T> : State<T>
{
    public Initial(T stateName, AIOneController controller, float minDuration) : base(stateName, controller, minDuration) { }


    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}