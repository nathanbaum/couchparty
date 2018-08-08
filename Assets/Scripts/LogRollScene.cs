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
    public List<GameObject> Timer;
 

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
        StartCoroutine(InstructionTime());
        Debug.Log("IsServer " + isServer);
        Debug.Log(GameScene);
        Active = true;
    }
    IEnumerator InstructionTime()
    {
        yield return new WaitForSeconds(5f);
        RpcCountdownStart();
        yield return null;
    }


    [ClientRpc]
    public void RpcShowInstructions() {
        Debug.Log("Inside LogRollScene RpcShowInstructions");
        Debug.Log("IsClient " + isServer);
        GameInstructions.SetActive(true);
    }

    [ClientRpc]
    public void RpcCountdownStart()
    {
        Debug.Log("Starting countdown");
        Destroy(GameInstructions);
        for (int i = 0; i < Timer.Count; i++)
        {
            Debug.Log("Countdown: " + i);
            Timer[i].SetActive(true);
            StartCoroutine(CountDown(Timer[i]));
        }
        StartCoroutine(CreateGame());
    }


    [ClientRpc]
    public void RpcCountdown(GameObject num)
    {
        num.SetActive(false);
    }

    IEnumerator CountDown(GameObject num)
    {
        yield return new WaitForSeconds(1f);
        RpcCountdown(num);
        yield return null;
    }

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

	// Update is called once per frame
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
