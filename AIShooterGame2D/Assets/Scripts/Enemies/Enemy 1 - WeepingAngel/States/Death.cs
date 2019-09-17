using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death<T> : State<T>
{
    public Death(T stateName, AIOneController controller, float minDuration) : base(stateName, controller, minDuration) { }


    public override void OnEnter()
    {
        base.OnEnter();
        this.controller.GetComponent<AIOneController>().Death(); //Call death function
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}