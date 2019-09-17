using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRock<T> : State<T>
{
    public FollowRock(T stateName, AITwoController controller, float minDuration) : base(stateName, controller, minDuration) { }

    public GameObject Rock;
    AITwoController Controller;

    public override void OnEnter()
    {
        base.OnEnter();
        Controller = this.controller.GetComponent<AITwoController>();
        Rock = Controller.Rock;
    }

    public override void Act()
    {
        base.Act();
        if (Rock == null)
        {
            Controller.RockBool = false;
        }

        this.controller.transform.position = Vector3.MoveTowards(this.controller.transform.position, Rock.transform.position, 2.5f * Time.deltaTime);

        if(this.controller.transform.position == Rock.transform.position)
        {
            Controller.RockBool = false;
        }

        //Chase the rock and go to its position and then when the rocks pos and this Ai's pos is the same, change the bool/state
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}