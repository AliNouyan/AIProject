using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThreeController : MonoBehaviour {

    public GameObject Rock;

    public GameObject ally1;
    public GameObject ally2;
    public GameObject ally3;
    List<GameObject> allies = new List<GameObject>();

    GameObject Player;

    bool flashlight;
    public bool Recover = false;
    public int Anger = 0;
    public int DeadAllies;
    public int ClickCount;
    public float PlayerDistance;
    public int FlashlightInt = 0;
    private int FuzzyAnger;
    private int FuzzyClickCount;
    private int FuzzyPlayerDistance;
    private int FuzzyFlashlightInt;

    public int fuzzyActionValue;
    public int degreeFlee;
    public int degreeChasePlayer;
    public int degreeThrowRock;
    public int degreeDash;

    // Use this for initialization
    void Start () {
        setStates();

        Player = GameObject.FindGameObjectWithTag("Player");

        Anger = 0;
        ClickCount = 0;
        ally1 = GameObject.FindGameObjectWithTag("Enemy1");
        ally2 = GameObject.FindGameObjectWithTag("Enemy2");
        ally3 = GameObject.FindGameObjectWithTag("Enemy4");
        allies.Add(ally1);
        allies.Add(ally2);
        allies.Add(ally3);

        StartCoroutine(IncreaseAnger());
        StartCoroutine(FlashLight());
        StartCoroutine(Think());
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //Log the ammount of player clicks
            ClickCount++;
        }

        //Originally i was going to use the allies alive/dead as part of the fuzzy logic 
        //But in the end i decided not to but kept the code here anyway since im using dead allies
        //to kill this AI, if all other AI is dead this AI dies after his dash
        //I couldve used another/shorter method to calculate the dead enemies but I already had this code
        if (ally1 == null)
        {
            allies.Remove(ally1);
        }
        if (ally2 == null)
        {
            allies.Remove(ally2);
        }
        if (ally3 == null)
        {
            allies.Remove(ally3);
        }

        if (allies.Count == 3)
        {
            DeadAllies = 0;
        }
        if (allies.Count == 2)
        {
            DeadAllies = 1;
        }
        if (allies.Count == 1)
        {
            DeadAllies = 2;
        }
        if (allies.Count == 0)
        {
            DeadAllies = 3;
        }

        //calculate distance
        PlayerDistance = Vector3.Distance(this.transform.position, Player.transform.position);

        AIThreeStateMachine.Check();

        AIThreeStateMachine.CurrentState.Act();
    }

    IEnumerator IncreaseAnger()
    {
        yield return new WaitForSeconds(1f);
        if(Anger < 30)
        {
            //Loop increasing anger every second
            Anger++;
        }
        StartCoroutine(IncreaseAnger());
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "FlashLight")
        {
            flashlight = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "FlashLight")
        {
            flashlight = false;
        }
    }

    IEnumerator FlashLight()
    {
        if (flashlight == true && FlashlightInt < 10)
        {
            //If flashlight on AI, increase the flastlight int by 1 every second
            yield return new WaitForSeconds(1f);
            FlashlightInt = FlashlightInt + 1;
            StartCoroutine(FlashLight());
        }
        else if (flashlight == false)
        {
            //If flashlight not on AI, decrease the flastlight int by 1 every 2 seconds
            yield return new WaitForSeconds(2f);
            if (FlashlightInt > 0)
            {
                FlashlightInt = FlashlightInt - 1;
            }
            StartCoroutine(FlashLight());
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

    //Fuzzy Think
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.4f);

        FuzzyAnger = Fuzzy.Grade(Anger, 0, 30);
        FuzzyClickCount = Fuzzy.Grade(ClickCount, 3, 50);
        FuzzyPlayerDistance = Fuzzy.Grade(PlayerDistance, 1, 7);
        FuzzyFlashlightInt = Fuzzy.Grade(FlashlightInt, 1, 10);
         
        Debug.Log(FuzzyPlayerDistance);
        //Flee if FlashlightINT is high OR AI is close to the player
        degreeFlee = Fuzzy.OR(FuzzyFlashlightInt, Fuzzy.NOT(FuzzyPlayerDistance));
        //ChasePlayer if AI is far from player AND hes not angry
        degreeChasePlayer = Fuzzy.AND(FuzzyPlayerDistance, Fuzzy.NOT(FuzzyAnger));
        //Dash based on the average of ANGER and ClickCount
        degreeDash = Fuzzy.AVERAGE(FuzzyAnger, FuzzyClickCount);
        //throwRock if ClickCount is low but hes angry
        degreeThrowRock = Fuzzy.AND(FuzzyAnger, Fuzzy.NOT(FuzzyClickCount));

        fuzzyActionValue = Fuzzy.OR(Fuzzy.OR(degreeFlee, degreeChasePlayer), Fuzzy.OR(degreeDash, degreeThrowRock));
        
        StartCoroutine(Think());
    }


    //State Machine
    public enum AIThreeStates { Chase, Flee, DashThree, ThrowRockThree, RecoverThree, DeathThree };

    [HideInInspector]
    public FSM<AIThreeStates> AIThreeStateMachine;

    public void setStates()
    {
        //Creating state machine
        AIThreeStateMachine = new FSM<AIThreeStates>();

        AIThreeStateMachine.AddState(new Chase<AIThreeStates>(AIThreeStates.Chase, this, 0f));
        AIThreeStateMachine.AddState(new Flee<AIThreeStates>(AIThreeStates.Flee, this, 0f));
        AIThreeStateMachine.AddState(new DashThree<AIThreeStates>(AIThreeStates.DashThree, this, 0f));
        AIThreeStateMachine.AddState(new ThrowRockThree<AIThreeStates>(AIThreeStates.ThrowRockThree, this, 0f));
        AIThreeStateMachine.AddState(new DeathThree<AIThreeStates>(AIThreeStates.DeathThree, this, 0f));
        AIThreeStateMachine.AddState(new RecoverThree<AIThreeStates>(AIThreeStates.RecoverThree, this, 3f));

        //Legal transitions
        AIThreeStateMachine.AddTransition(AIThreeStates.Chase, AIThreeStates.Flee);
        AIThreeStateMachine.AddTransition(AIThreeStates.Flee, AIThreeStates.Chase);
        AIThreeStateMachine.AddTransition(AIThreeStates.Chase, AIThreeStates.ThrowRockThree);
        AIThreeStateMachine.AddTransition(AIThreeStates.Chase, AIThreeStates.DashThree);
        AIThreeStateMachine.AddTransition(AIThreeStates.Flee, AIThreeStates.ThrowRockThree);
        AIThreeStateMachine.AddTransition(AIThreeStates.Flee, AIThreeStates.DashThree);
        AIThreeStateMachine.AddTransition(AIThreeStates.ThrowRockThree, AIThreeStates.Chase);
        AIThreeStateMachine.AddTransition(AIThreeStates.ThrowRockThree, AIThreeStates.Flee);
        AIThreeStateMachine.AddTransition(AIThreeStates.DashThree, AIThreeStates.RecoverThree);
        AIThreeStateMachine.AddTransition(AIThreeStates.RecoverThree, AIThreeStates.Chase);
        AIThreeStateMachine.AddTransition(AIThreeStates.RecoverThree, AIThreeStates.Flee);
        AIThreeStateMachine.AddTransition(AIThreeStates.RecoverThree, AIThreeStates.DeathThree);

        AIThreeStateMachine.SetInitialState(AIThreeStates.Chase);
    }

    public bool GuardChaseToFlee(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeFlee);
    }

    public bool GuardFleeToChase(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeChasePlayer);
    }

    public bool GuardChaseToThrowRockThree(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeThrowRock);
    }

    public bool GuardChaseToDashThree(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeDash);
    }

    public bool GuardFleeToThrowRockThree(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeThrowRock);
    }

    public bool GuardFleeToDashThree(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeDash);
    }

    public bool GuardThrowRockThreeToChase(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeChasePlayer);
    }

    public bool GuardThrowRockThreeToFlee(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeFlee);
    }

    public bool GuardDashThreeToRecoverThree(State<AIThreeStates> currentState)
    {
        return (Recover == true);
    }

    public bool GuardRecoverThreeToChase(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeChasePlayer);
    }

    public bool GuardRecoverThreeToFlee(State<AIThreeStates> currentState)
    {
        return (fuzzyActionValue == degreeFlee);
    }

    public bool GuardRecoverThreeToDeathThree(State<AIThreeStates> currentState)
    {
        return (DeadAllies == 3);
    }
}
