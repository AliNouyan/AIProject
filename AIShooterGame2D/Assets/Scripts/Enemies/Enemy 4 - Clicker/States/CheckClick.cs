using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClick<T> : State<T>
{
    public CheckClick(T stateName, AIFourController controller, float minDuration) : base(stateName, controller, minDuration) { }


    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}