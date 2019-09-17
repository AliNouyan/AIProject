using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFourController : MonoBehaviour
{
    public GameObject Rock;
    Transform PlayerTransform;
    Vector3 StoredPos;
    public Vector3 ClickPos;
    public int AttackCount;
    public int ClickCount;
    public bool ThirdClick = false;
    public bool Moving = false;
    public bool GoToPlayerBool = false;
    public bool DashFourBool = false;
    public bool RockandDashBool = false;
    public bool DeathBool = false;
    public bool CheckCol = false;
    public bool Attack = false;
    public bool Dash = false;
    bool timer = false;
    public float Speed;
    float StartSpeed = 1f;
    float SpeedMultiply = 1.3f;
    float Time = 3f;

    // Use this for initialization
    void Start()
    {
        setStates();

        Speed = StartSpeed;
        AttackCount = 0;
        PlayerTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //If pressed left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            //increase click count till it reaches 3
            ClickCount++;
            if (ClickCount == 3)
            {
                Debug.Log("ClickCount is " + ClickCount);
                ClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ClickPos.z = transform.position.z;
                ClickCount = 0;
                ThirdClick = true;
                //If third click, turn bool to true and reset click count
            }
        }

        //Check if player is moving
        if (StoredPos != PlayerTransform.position)
        {
            Moving = true;
        }
        else
        {
            Moving = false;
        }
        StoredPos = PlayerTransform.position;

        if (ThirdClick == true && Moving == true)
        {
            Speed = Speed * SpeedMultiply; //increase speed
        }

        if(Moving == false)
        {
            Speed = StartSpeed; //reset speed when player stop moving
        }

        if(timer == false)
        {
            StartCoroutine(Timer()); // start timer, is timer is not on
        }

        AIFourStateMachine.Check();

        AIFourStateMachine.CurrentState.Act();
    }

    IEnumerator Timer()
    {
        if(Moving == false)
        {
            timer = true;
            yield return new WaitForSeconds(Time);
            timer = false;
            //if moving is false start timer, if moving is still false after timer finished
            //then
            if(Moving == false)
            {
                //Attack
                Attack = true;
                Debug.Log("AttackIsNowTrue" + AttackCount);
                //Chance time the Ai has to wait for after each attack
                if (Time == 4f)
                {
                    Time = Time + 1f;
                }
                else
                {
                    Time = Time + 0.5f;
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if(CheckCol == true)
        {
            if(col.gameObject.tag == "Walls")
            {
                //Check for collision on walls
                if (DashFourBool == false)
                {
                    DashFourBool = true;
                    CheckCol = false;
                }

                else if (DashFourBool == true && RockandDashBool == false)
                {
                    RockandDashBool = true;
                    CheckCol = false;
                }

                else if (RockandDashBool == true)
                {
                    DeathBool = true;
                }
            }
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    //State Machine
    public enum AIFourStates { CheckClick, GoToClickOne, GoToClickTwo, GoToPlayer, DashFour, RockandDash, DeathFour};

    [HideInInspector]
    public FSM<AIFourStates> AIFourStateMachine;

    public void setStates()
    {
        //Creating state machine
        AIFourStateMachine = new FSM<AIFourStates>();

        AIFourStateMachine.AddState(new CheckClick<AIFourStates>(AIFourStates.CheckClick, this, 0f));
        AIFourStateMachine.AddState(new GoToClickOne<AIFourStates>(AIFourStates.GoToClickOne, this, 0f));
        AIFourStateMachine.AddState(new GoToClickTwo<AIFourStates>(AIFourStates.GoToClickTwo, this, 0f));
        AIFourStateMachine.AddState(new GoToPlayer<AIFourStates>(AIFourStates.GoToPlayer, this, 0f));
        AIFourStateMachine.AddState(new DashFour<AIFourStates>(AIFourStates.DashFour, this, 0f));
        AIFourStateMachine.AddState(new RockandDash<AIFourStates>(AIFourStates.RockandDash, this, 0f));
        AIFourStateMachine.AddState(new DeathFour<AIFourStates>(AIFourStates.DeathFour, this, 0f));

        //Legal transitions
        AIFourStateMachine.AddTransition(AIFourStates.CheckClick, AIFourStates.GoToClickOne);
        AIFourStateMachine.AddTransition(AIFourStates.GoToClickOne, AIFourStates.GoToClickTwo);
        AIFourStateMachine.AddTransition(AIFourStates.GoToClickTwo, AIFourStates.GoToClickOne);
        AIFourStateMachine.AddTransition(AIFourStates.GoToClickOne, AIFourStates.CheckClick);
        AIFourStateMachine.AddTransition(AIFourStates.GoToClickTwo, AIFourStates.CheckClick);
        AIFourStateMachine.AddTransition(AIFourStates.CheckClick, AIFourStates.GoToPlayer);
        AIFourStateMachine.AddTransition(AIFourStates.GoToPlayer, AIFourStates.CheckClick);
        AIFourStateMachine.AddTransition(AIFourStates.CheckClick, AIFourStates.DashFour);
        AIFourStateMachine.AddTransition(AIFourStates.DashFour, AIFourStates.CheckClick);
        AIFourStateMachine.AddTransition(AIFourStates.CheckClick, AIFourStates.RockandDash);
        AIFourStateMachine.AddTransition(AIFourStates.RockandDash, AIFourStates.CheckClick);
        AIFourStateMachine.AddTransition(AIFourStates.CheckClick, AIFourStates.DeathFour);

        AIFourStateMachine.SetInitialState(AIFourStates.CheckClick);
    }
    public bool GuardCheckClickToGoToClickOne(State<AIFourStates> currentState)
    {
        return (ThirdClick == true && Moving == false);
    }

    public bool GuardGoToClickOneToGoToClickTwo(State<AIFourStates> currentState)
    {
        return (ThirdClick == true && Moving == true);
    }

    public bool GuardGoToClickTwoToGoToClickOne(State<AIFourStates> currentState)
    {
        return (ThirdClick == true && Moving == true);
    }

    public bool GuardGoToClickOneToCheckClick(State<AIFourStates> currentState)
    {
        return (Moving == false);
    }

    public bool GuardGoToClickTwoToCheckClick(State<AIFourStates> currentState)
    {
        return (Moving == false);
    }

    public bool GuardCheckClickToGoToPlayer(State<AIFourStates> currentState)
    {
        return (Moving == false && AttackCount == 0 && Attack == true);
    }

    public bool GuardGoToPlayerToCheckClick(State<AIFourStates> currentState)
    {
        return (GoToPlayerBool == true);
    }

    public bool GuardCheckClickToDashFour(State<AIFourStates> currentState)
    {
        return (Moving == false && AttackCount == 1 && Attack == true);
    }

    public bool GuardDashFourToCheckClick(State<AIFourStates> currentState)
    {
        return (DashFourBool == true);
    }

    public bool GuardCheckClickToRockandDash(State<AIFourStates> currentState)
    {
        return (Moving == false && AttackCount == 2 && Attack == true);
    }

    public bool GuardRockandDashToCheckClick(State<AIFourStates> currentState)
    {
        return (RockandDashBool == true);
    }

    public bool GuardCheckClickToDeathFour(State<AIFourStates> currentState)
    {
        return (Moving == false && AttackCount == 3 && Attack == true);
    }
}