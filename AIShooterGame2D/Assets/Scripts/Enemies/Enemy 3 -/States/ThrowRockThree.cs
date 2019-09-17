using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRockThree<T> : State<T>
{
    public ThrowRockThree(T stateName, AIThreeController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Player;
    GameObject Self;
    Transform PlayerTransform;
    Transform SelfTransform;
    Vector3 LookDir;
    AIThreeController Brain;

    public override void OnEnter()
    {
        base.OnEnter();
        Self = this.controller.gameObject;
        SelfTransform = Self.transform;
        Brain = Self.GetComponent<AIThreeController>();

        MonoBehaviour.Instantiate(Brain.Rock, SelfTransform.position, SelfTransform.rotation);
    }

    public override void Act()
    {
        base.Act();
    }

    public override void OnLeave()
    {
        base.OnLeave();
        //Decrease anger
        Brain.Anger = Brain.Anger - 10;
    }
}