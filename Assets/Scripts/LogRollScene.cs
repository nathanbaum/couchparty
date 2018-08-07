using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogRollScene : PseudoScene {

    private NextPseudoScene Next;
    private List<PlayerStateController> Players;
    public List<Transform> PlayerDropPoints;
    private bool cloudsAreDescending;
    public Transform CloudStopPoint;
    public GameObject GameScene;
    public GameObject GameInstructions;


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

    public override void MoveController(PlayerController P)
    {
        Destroy(GameObject.Find("Floating Platform"));
        Rigidbody rb = P.gameObject.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if( Input.GetMouseButtonDown(0) ) {
            rb.AddForce(new Vector3(0, 700, 0));
        }

    }

    private void SetUp() {
        for (int i = 0; i < Players.Count; i++ ) {
            Players[i].gameObject.GetComponent<PlayerController>().RpcSetMoveControls("LogRollScene");
            Players[i].gameObject.GetComponent<PlayerController>().RpcSnapTo(PlayerDropPoints[i].position);
        }
        GameInstructions.SetActive(true);
        StartCoroutine(CreateGame());


        
    }
    IEnumerator CreateGame()
    {
        yield return new WaitForSeconds(5f);
        Destroy(GameInstructions);
        GameScene.SetActive(true);
        StartCoroutine(StartGame());
        yield return null;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);
        GameScene.GetComponent<LogRotate>().enabled = true;
        yield return null;
    }

	// Update is called once per frame
    /*private bool PlayersAlive(){
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].IsDead == false)
            {
                return true;
            }
        }
        return false;
    }*/

    void Update () {
        //if(PlayersAlive()){
        //    Debug.Log("Alive");
        //}
        //else{
        //    Debug.Log("GameOver");
        //}
	}
}
