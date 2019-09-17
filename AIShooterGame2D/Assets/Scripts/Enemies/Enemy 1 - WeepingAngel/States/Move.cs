using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move<T> : State<T>
{
    public Move(T stateName, AIOneController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject self;
    GameObject Player;

    float Speed;
    float StartSpeed = 0.5f;
    float MaxSpeed = 2.5f;
    float Acceleration = 0.005f;

    public override void OnEnter()
    {
        base.OnEnter();
        self = this.controller.gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");

        Speed = StartSpeed;
    }

    public override void Act()
    {
        base.Act();
        //Move ai towards players position
        self.transform.position = Vector3.MoveTowards(self.transform.position, Player.transform.position, Speed * Time.deltaTime);

        //Increase speed
        if(Speed <= MaxSpeed)
        {
            Speed = Speed + Acceleration;
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();

    }
}
