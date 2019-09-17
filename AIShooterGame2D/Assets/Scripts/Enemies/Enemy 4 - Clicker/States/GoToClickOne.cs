using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToClickOne<T> : State<T>
{
    public GoToClickOne(T stateName, AIFourController controller, float minDuration) : base(stateName, controller, minDuration) { }

    //2 gotoclick scripts so it'll change straight away when theres a new click location

    GameObject self;
    AIFourController Brain;

    float speed;

    public override void OnEnter()
    {
        base.OnEnter();
        self = this.controller.gameObject;
        Brain = self.GetComponent<AIFourController>();
        Brain.ThirdClick = false;
        speed = Brain.Speed;
    }

    public override void Act()
    {
        base.Act();
        //go to clicks position
        self.transform.position = Vector3.MoveTowards(self.transform.position, Brain.ClickPos, speed * Time.deltaTime);
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}