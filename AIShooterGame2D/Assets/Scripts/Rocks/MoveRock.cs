using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRock : MonoBehaviour {
    //originaly move rock script, moves towards where the player clicks
    GameObject Player;
    GameObject enemyTwo;
    Vector3 TargetPos;

    // Use this for initialization
    void Start () {
        enemyTwo = GameObject.FindGameObjectWithTag("Enemy2");
        enemyTwo.GetComponent<AITwoController>().RockBool = true;
        enemyTwo.GetComponent<AITwoController>().Rock = this.gameObject;

        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerController>().cooldown = true;

        TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetPos.z = transform.position.z;
        //get position
    }
	
	// Update is called once per frame
	void Update () {
        //move rock
        this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPos, 3f * Time.deltaTime);
    }
}
