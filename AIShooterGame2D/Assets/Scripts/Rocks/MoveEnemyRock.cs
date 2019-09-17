using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyRock : MonoBehaviour {
    
    GameObject Self;
    GameObject Player;
    Vector3 TargetPos;

	// Use this for initialization
	void Start () {
        Self = GameObject.FindGameObjectWithTag("Enemy4");
        Player = GameObject.FindGameObjectWithTag("Player");
        TargetPos = Player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPos, 3f * Time.deltaTime);

        if(this.transform.position == TargetPos)
        {
            var AiFourBrain = Self.GetComponent<AIFourController>();
            AiFourBrain.Dash = true;
            Destroy(this.gameObject);
        }
        //get and store players position and then move the rock to the position
    }
}
