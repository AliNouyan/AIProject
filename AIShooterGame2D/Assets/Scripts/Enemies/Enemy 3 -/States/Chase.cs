using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase<T> : State<T>
{
    public Chase(T stateName, AIThreeController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject self;
    GameObject Player;

    float Speed = 2f;

    public override void OnEnter()
    {
        base.OnEnter();
        self = this.controller.gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Act()
    {
        base.Act();
        //Chase the player
        self.transform.position = Vector3.MoveTowards(self.transform.position, Player.transform.position, Speed * Time.deltaTime);
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}