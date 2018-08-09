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
    bool Active;


	// Use this for initialization
	void Start () {
        cloudsAreDescending = false;
        Active = false;
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
        GameInstructions.SetActive(false);
        StartCoroutine(InstructionTime());

        Debug.Log("IsServer " + isServer);
        Debug.Log(GameScene);
        Active = true;
    }

    [ClientRpc]
    public void RpcActivate(string name)
    {
        Debug.Log(name);
        GameObject obj = GameObject.Find(name);
        Debug.Log("OBJ: " + obj.name);
        obj.SetActive(true);
    }

    [ClientRpc]
    public void RpcDeactivate(string name)
    {
        GameObject obj = GameObject.Find(name);
  
        obj.SetActive(false);
    }


    IEnumerator InstructionTime()
    {
        yield return new WaitForSeconds(1f);
        RpcActivate("Instructions/Timer/three");
        yield return new WaitForSeconds(1f);
        RpcDeactivate("Instructions/Timer/three");
        RpcActivate("Instructions/Timer/two");
        yield return new WaitForSeconds(1f);
        RpcDeactivate("Instructions/Timer/two");
        RpcActivate("Instructions/Timer/one");
        yield return new WaitForSeconds(1f);
        RpcDeactivate("Instructions/Timer/one");
        StartCoroutine(CreateGame());
        yield return null;
    }







    //IEnumerator InstructionTime()
    //{
    //    yield return new WaitForSeconds(2f);
    //    RpcCountdownStart();
    //    yield return null;
    //}


    [ClientRpc]
    public void RpcShowInstructions() {
        Debug.Log("Inside LogRollScene RpcShowInstructions");
        Debug.Log("IsClient " + isServer);
        GameInstructions.SetActive(true);
    }

    //[ClientRpc]
    //public void RpcCountdownStart()
    //{
    //    Debug.Log("Starting countdown");
    //    Destroy(GameInstructions);
    //    for (int i = 0; i < Timer.Count; i++)
    //    {
    //        Debug.Log("Countdown: " + i);
    //        Timer[i].SetActive(true);
    //        StartCoroutine(CountDown(Timer[i]));
    //    }
    //    StartCoroutine(CreateGame());
    //}



    [ClientRpc]
    public void RpcCreateGame()
    {
        Debug.Log(GameScene);
        Debug.Log(GameInstructions);
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

    public int PlayersAlive(){
        int num = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (!Players[i].IsDead)
            {
                num++;
            }
        }
        return num;
    }

    void TearDown() {
        GameScene.SetActive(false);
        Active = false;
        Next(Players);
    }

    void Update () {
        if( !isServer || !Active ){
            return;
        }
        if(PlayersAlive() == 0){
            Debug.Log("GameOver");
            TearDown();
        }
	}
}
