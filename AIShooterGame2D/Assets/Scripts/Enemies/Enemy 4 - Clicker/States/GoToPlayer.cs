using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer<T> : State<T>
{
    public GoToPlayer(T stateName, AIFourController controller, float minDuration) : base(stateName, controller, minDuration) { }

    GameObject Player;
    GameObject Self;
    AIFourController Brain;
    Vector3 PlayerPos;

    public override void OnEnter()
    {
        base.OnEnter();
        Self = this.controller.gameObject;
        Brain = Self.GetComponent<AIFourController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerPos = Player.transform.position;
        Brain.Attack = false;
    }

    public override void Act()
    {
        base.Act();
        //go to players position
        Self.transform.position = Vector3.MoveTowards(Self.transform.position, PlayerPos, 1.5f * Time.deltaTime);

        //if this pos, is the same as the players stored position then change bool/state
        if(Self.transform.position == PlayerPos)
        {
            Brain.GoToPlayerBool = true;
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
        Brain.AttackCount++;
        Brain.Attack = false;
        Debug.Log(Brain.Moving);
        Debug.Log(Brain.Attack);
    }
}