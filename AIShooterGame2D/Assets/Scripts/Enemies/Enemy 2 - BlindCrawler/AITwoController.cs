using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITwoController : MonoBehaviour {

    GameObject Player;
    public GameObject Rock;
    public GameObject Boulder;
    Transform PlayerTransform;
    Vector3 StoredPos;
    bool Movement = false;
    public bool StopDash = false;
    public bool RockBool = false;
    public bool HitBoulder = false;
    public int BoulderCount = 0;
    public float Dist;

	// Use this for initialization
	void Start () {
        setStates();

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.transform;

        StoredPos = PlayerTransform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //Check if the player is moving and change bool accordingly
        if(StoredPos != PlayerTransform.position)
        {
            Debug.Log("Player Is Moving");
            Movement = true;
        }
        else
        {
            Movement = false;
        }
        StoredPos = PlayerTransform.position;

        Dist = Vector3.Distance(this.transform.position, StoredPos);
        Debug.Log("AI 2 Dist: " + Dist);

        AITwoStateMachine.Check();

        AITwoStateMachine.CurrentState.Act();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Check for boulder collision 
        if (col.gameObject.tag == "Boulder")
        {
            HitBoulder = true;
            Boulder = col.gameObject;
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Walls")
        {
            StopDash = true;
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    //State Machine
    public enum AITwoStates { Listen, FollowPlayer, FollowRock, PreDash, DashTwo, RecoverTwo, DeathTwo };

    [HideInInspector]
    public FSM<AITwoStates> AITwoStateMachine;

    public void setStates()
    {
        //Creating state machine
        AITwoStateMachine = new FSM<AITwoStates>();

        AITwoStateMachine.AddState(new Listen<AITwoStates>(AITwoStates.Listen, this, 1f));
        AITwoStateMachine.AddState(new FollowPlayer<AITwoStates>(AITwoStates.FollowPlayer, this, 0f));
        AITwoStateMachine.AddState(new FollowRock<AITwoStates>(AITwoStates.FollowRock, this, 0f));
        AITwoStateMachine.AddState(new PreDash<AITwoStates>(AITwoStates.PreDash, this, 1f));
        AITwoStateMachine.AddState(new DashTwo<AITwoStates>(AITwoStates.DashTwo, this, 0f));
        AITwoStateMachine.AddState(new RecoverTwo<AITwoStates>(AITwoStates.RecoverTwo, this, 3f));
        AITwoStateMachine.AddState(new DeathTwo<AITwoStates>(AITwoStates.DeathTwo, this, 0f));

        //Legal transitions
        AITwoStateMachine.AddTransition(AITwoStates.FollowPlayer, AITwoStates.FollowRock);
        AITwoStateMachine.AddTransition(AITwoStates.Listen, AITwoStates.FollowRock);
        AITwoStateMachine.AddTransition(AITwoStates.Listen, AITwoStates.FollowPlayer);
        AITwoStateMachine.AddTransition(AITwoStates.FollowRock, AITwoStates.Listen);
        AITwoStateMachine.AddTransition(AITwoStates.FollowPlayer, AITwoStates.Listen);
        AITwoStateMachine.AddTransition(AITwoStates.FollowRock, AITwoStates.RecoverTwo);
        AITwoStateMachine.AddTransition(AITwoStates.RecoverTwo, AITwoStates.Listen);
        AITwoStateMachine.AddTransition(AITwoStates.FollowPlayer, AITwoStates.PreDash);
        AITwoStateMachine.AddTransition(AITwoStates.PreDash, AITwoStates.DashTwo);
        AITwoStateMachine.AddTransition(AITwoStates.DashTwo, AITwoStates.Listen);
        AITwoStateMachine.AddTransition(AITwoStates.RecoverTwo, AITwoStates.DeathTwo);

        AITwoStateMachine.SetInitialState(AITwoStates.Listen);
    }
    public bool GuardFollowPlayerToFollowRock(State<AITwoStates> currentState)
    {
        return (RockBool == true);
    }

    public bool GuardListenToFollowRock(State<AITwoStates> currentState)
    {
        return (RockBool == true);
    }

    public bool GuardListenToFollowPlayer(State<AITwoStates> currentState)
    {
        return (Movement == true);
    }

    public bool GuardFollowRockToListen(State<AITwoStates> currentState)
    {
        return (RockBool == false);
    }

    public bool GuardFollowPlayerToListen(State<AITwoStates> currentState)
    {
        return (Movement == false);
    }

    public bool GuardFollowRockToRecoverTwo(State<AITwoStates> currentState)
    {
        return (HitBoulder == true);
    }

    public bool GuardRecoverTwoToDeathTwo(State<AITwoStates> currentState)
    {
        return (BoulderCount >= 3);
    }

    public bool GuardRecoverTwoToListen(State<AITwoStates> currentState)
    {
        return (BoulderCount < 3);
    }

    public bool GuardFollowPlayerToPreDash(State<AITwoStates> currentState)
    {
        return (Dist < 1.75f);
    }

    public bool GuardPreDashToDashTwo(State<AITwoStates> currentState)
    {
        return (true);
    }

    public bool GuardDashTwoToListen(State<AITwoStates> currentState)
    {
        return (StopDash == true);
    }
}
