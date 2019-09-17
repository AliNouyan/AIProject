using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float speed = 2f;
    public int RockCount = 1;
    public bool cooldown = false;
    Vector3 TargetPos;
    GameObject Rock;
    public GameObject RockPrefab;
    public GameObject RockSpawn;
    Transform storedTransform;

	// Use this for initialization
	void Start () {
        TargetPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //Move Player
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse Down " + Input.mousePosition);
            TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetPos.z = transform.position.z;
        }
        //Debug.Log("hi");
        this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPos, speed * Time.deltaTime);
        //Move Player

        //Rotation
        Vector3 MouseScreen = Input.mousePosition;
        Vector3 Mouse = Camera.main.ScreenToWorldPoint(MouseScreen);

        var Rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Mouse.y - transform.position.y, Mouse.x - transform.position.x) * Mathf.Rad2Deg - 90);
        transform.rotation = Rotation;
        //Rotation 

        //Pickup/Throw Rocks
        if (cooldown == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Mouse Down " + Input.mousePosition);
                //Debug.Log("Space Pressed");
                if (RockCount > 0)
                {
                    //instantiate rock and lower rock count
                    Instantiate(RockPrefab, RockSpawn.transform.position, RockSpawn.transform.rotation);
                    RockCount--;
                    StartCoroutine(CooldownTimer());
                }
            }
        }
        //Pickup/Throw Rocks
    }

    public void PickUpRock()
    {
        RockCount = RockCount + 1;
        Debug.Log("Rocks: " + RockCount);
    }

    IEnumerator CooldownTimer()
    {
        //timer between rock throws
        yield return new WaitForSeconds(3f);
        cooldown = false;
    }
}

