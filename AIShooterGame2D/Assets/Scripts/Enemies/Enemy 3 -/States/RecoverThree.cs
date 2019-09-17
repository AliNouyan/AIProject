using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverThree<T> : State<T>
{
    public RecoverThree(T stateName, AIThreeController controller, float minDuration) : base(stateName, controller, minDuration) { }


    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void Act()
    {
        base.Act();
        
    }

    public override void OnLeave()
    {
        base.OnLeave();
        this.controller.GetComponent<AIThreeController>().Recover = false;
    }
}