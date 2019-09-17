﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTwo<T> : State<T>
{
    public DashTwo(T stateName, AITwoController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Player;
    GameObject Self;
    Transform PlayerTransform;
    Transform SelfTransform;
    Vector3 LookDir;

    public override void OnEnter()
    {
        base.OnEnter();
        Player = GameObject.FindGameObjectWithTag("Player");
        Self = this.controller.gameObject;
        PlayerTransform = Player.transform;
        SelfTransform = Self.transform;

        LookDir = (SelfTransform.position - PlayerTransform.position);
    }

    public override void Act()
    {
        base.Act();
        //dash towards player
        controller.transform.Translate(-LookDir * 1.5f * Time.deltaTime);
    }

    public override void OnLeave()
    {
        base.OnLeave();
        var SelfController = Self.GetComponent<AITwoController>();
        SelfController.StopDash = false;
    }
}