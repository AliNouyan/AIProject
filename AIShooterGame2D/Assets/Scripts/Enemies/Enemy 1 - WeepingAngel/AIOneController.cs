using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOneController : MonoBehaviour {

    bool colCheck = false;
    public bool Dash = false;
    public bool Recover = false;
    float DashTime = 3;
    public int DashCount = 0;
    bool stopTimer = false;

	// Use this for initialization
	void Start () {
        setStates();
    }
	
	// Update is called once per frame
	void Update () {
        if(colCheck == true)
        {
            StartCoroutine(DashTimer());
        }

        AIOneStateMachine.Check();

        AIOneStateMachine.CurrentState.Act();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "FlashLight")
        {
            colCheck = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "FlashLight")
        {
            colCheck = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Walls")
        {
            Recover = true;
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator DashTimer()
    {
        if(stopTimer == false)
        {
            stopTimer = true; //This is so the Dash timer doesnt get called multiple times
            yield return new WaitForSeconds(DashTime);

            if (colCheck == true)
            {
                Dash = true;
                DashCount = DashCount + 1;
                DashTime = DashTime + 1;
                Debug.Log("Dash Count: " + DashCount);
                Debug.Log("Dash Time: " + DashTime);
            }
            stopTimer = false;
        }
    }

    //State Machine
    public enum AIOneStates { Initial, Move, Freeze, Dash, Recover, Death };

    [HideInInspector] public FSM<AIOneStates> AIOneStateMachine;

    public void setStates()
    {
        //Creating state machine
        AIOneStateMachine = new FSM<AIOneStates>();

        AIOneStateMachine.AddState(new Initial<AIOneStates>(AIOneStates.Initial, this, 1f));
        AIOneStateMachine.AddState(new Move<AIOneStates>(AIOneStates.Move, this, 0f));
        AIOneStateMachine.AddState(new Freeze<AIOneStates>(AIOneStates.Freeze, this, 0f));
        AIOneStateMachine.AddState(new Dash<AIOneStates>(AIOneStates.Dash, this, 0f));
        AIOneStateMachine.AddState(new Recover<AIOneStates>(AIOneStates.Recover, this, 3f));
        AIOneStateMachine.AddState(new Death<AIOneStates>(AIOneStates.Death, this, 0f));

        //Legal transitions
        AIOneStateMachine.AddTransition(AIOneStates.Initial, AIOneStates.Move);
        AIOneStateMachine.AddTransition(AIOneStates.Move, AIOneStates.Freeze);
        AIOneStateMachine.AddTransition(AIOneStates.Freeze, AIOneStates.Move);
        AIOneStateMachine.AddTransition(AIOneStates.Freeze, AIOneStates.Dash);
        AIOneStateMachine.AddTransition(AIOneStates.Dash, AIOneStates.Recover);
        AIOneStateMachine.AddTransition(AIOneStates.Recover, AIOneStates.Move);
        AIOneStateMachine.AddTransition(AIOneStates.Recover, AIOneStates.Death);

        AIOneStateMachine.SetInitialState(AIOneStates.Initial);
    }

    public bool GuardInitialToMove(State<AIOneStates> currentState)
    {
        return (true);
    }

    public bool GuardMoveToFreeze(State<AIOneStates> currentState)
    {
        return (colCheck == true);
    }

    public bool GuardFreezeToMove(State<AIOneStates> currentState)
    {
        return (colCheck == false);
    }

    public bool GuardFreezeToDash(State<AIOneStates> currentState)
    {
        return (Dash == true);
    }

    public bool GuardDashToRecover(State<AIOneStates> currentState)
    {
        return (Recover == true);
    }

    public bool GuardRecoverToMove(State<AIOneStates> currentState)
    {
        return (DashCount < 3);
    }

    public bool GuardRecoverToDeath(State<AIOneStates> currentState)
    {
        return (DashCount >= 3);
    }
}
