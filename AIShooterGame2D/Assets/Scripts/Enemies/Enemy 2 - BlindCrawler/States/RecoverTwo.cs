using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverTwo<T> : State<T>
{
    public RecoverTwo(T stateName, AITwoController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Boulder;
    AITwoController Controller;

    public override void OnEnter()
    {
        base.OnEnter();
        Controller = this.controller.GetComponent<AITwoController>();
        Controller.BoulderCount++;
        Debug.Log("BoulderCount");

        Boulder = Controller.Boulder;
        MonoBehaviour.Destroy(Boulder);
        //Destroy the boulder the ai collided with
    }

    public override void OnLeave()
    {
        base.OnLeave();
        Controller.RockBool = false;
        Controller.HitBoulder = false;
    }
}