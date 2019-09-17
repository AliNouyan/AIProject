using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRock : MonoBehaviour {

    GameObject Player;
    PlayerController playerController;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (playerController.RockCount <= 3)
            {
                playerController.PickUpRock();
                Destroy(this.gameObject);
                //Pickup rock function and then destory it
            }
        }
    }
}
