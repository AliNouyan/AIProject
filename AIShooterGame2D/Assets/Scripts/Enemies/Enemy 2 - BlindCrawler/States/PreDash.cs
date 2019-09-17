using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreDash<T> : State<T>
{
    public PreDash(T stateName, AITwoController controller, float minDuration) : base(stateName, controller, minDuration) { }


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

    }
}