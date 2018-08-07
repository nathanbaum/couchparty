using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogRollScene : PseudoScene {

    private NextPseudoScene Next;
    private List<PlayerStateController> Players;
    public List<Transform> PlayerDropPoints;
    private bool cloudsAreDescending;
    public Transform CloudStopPoint;
    public GameObject GameScene;
    public GameObject GameInstructions;
    public bool isJumping = false;


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


        if( Input.GetMouseButtonDown(0) && P.isGrounded) {
            rb.AddForce(new Vector3(0, 700, 0));
        }

    }



    void SetUp() {
        for (int i = 0; i < Players.Count; i++ ) {
            Players[i].gameObject.GetComponent<PlayerController>().RpcSetMoveControls("LogRollScene");
            Players[i].gameObject.GetComponent<PlayerController>().RpcSnapTo(PlayerDropPoints[i].position);
        }

        Debug.Log("Inside LogRollScene Setup");
        RpcShowInstructions();
        StartCoroutine(CreateGame());
        Debug.Log("IsServer " + isServer);
        Debug.Log(GameScene);
    }

    [ClientRpc]
    public void RpcShowInstructions() {
        Debug.Log("Inside LogRollScene RpcShowInstructions");
        Debug.Log("IsClient " + isServer);
        GameInstructions.SetActive(true);
    }

    [ClientRpc]
    public void RpcCreateGame()
    {
        Debug.Log(GameScene);
        Debug.Log(GameInstructions);
        Destroy(GameInstructions);
        GameScene.SetActive(true);
    }

    IEnumerator CreateGame()
    {
        yield return new WaitForSeconds(5f);
        RpcCreateGame();
        StartCoroutine(StartGame());
        yield return null;
    }

    [ClientRpc]
    public void RpcStartGame() {
        GameScene.GetComponent<LogRotate>().enabled = true;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        RpcStartGame();
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
