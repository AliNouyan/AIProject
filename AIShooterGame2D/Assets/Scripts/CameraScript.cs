using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject Player;
    Vector3 Offset;

	// Use this for initialization
	void Start () {
        Offset = transform.position - Player.transform.position;
        Player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        //move camera
        transform.position = Player.transform.position + Offset;
    }
}
