using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogRollScene : PseudoScene {

    private NextPseudoScene Next;
    private List<PlayerStateController> Players;
    public List<Transform> PlayerDropPoints;
    private bool cloudsAreDescending;
    public Transform CloudStopPoint;

	// Use this for initialization
	void Start () {
        cloudsAreDescending = false;
	}


    public override void Run ( List<PlayerStateController> players, NextPseudoScene next ) {
        Players = players;
        Next = next;

        Debug.Log("Log Roll Scene Started!");

        SetUp();
    }

    private Transform MoveController( Transform T ){
        Rigidbody rb = T.gameObject.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if( Input.GetMouseButtonDown(0) ) {
            rb.AddForce(new Vector3(0, 1, 0));
        }

        return T;
    }

    private void SetUp() {
        for (int i = 0; i < Players.Count; i++ ) {
            Players[i].gameObject.GetComponent<PlayerController>().setMoveControls(MoveController);
            Players[i].gameObject.transform.position = PlayerDropPoints[i].position;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
