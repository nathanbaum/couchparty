using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PodiumScene : PseudoScene
{

    private NextPseudoScene Next;
    private List<PlayerStateController> Players;
    public List<Transform> PlayerDropPoints;
    bool Active;


    // Use this for initialization
    void Start()
    {
        Active = false;


    }


    public override void Run(List<PlayerStateController> players, NextPseudoScene next)
    {
        Players = players;
        Next = next;

        Debug.Log("Podium Scene Started!");

        SetUp();
    }

    public override void MoveController(PlayerController P)
    {
        //Destroy(GameObject.Find("Floating Platform"));
        Rigidbody rb = P.gameObject.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;


    }

    int SortByScore(PlayerStateController p1, PlayerStateController p2)
    {
        return p2.Score.CompareTo(p1.Score);
    }

    void SetUp()
    {

        Players.Sort(SortByScore);
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].gameObject.GetComponent<PlayerController>().RpcSetMoveControls("PodiumScene");
            Players[i].gameObject.GetComponent<PlayerController>().RpcSnapTo(PlayerDropPoints[i].position);
        }
        RpcStartParticles();

        //RpcShowInstructions();
        //StartCoroutine(CreateGame());
        //Debug.Log("IsServer " + isServer);
        //Debug.Log(GameScene);
        Active = true;
    }

    [ClientRpc]
    public void RpcStartParticles(){
        GameObject parts = GameObject.Find("PodiumParticles");
        parts.SetActive(true);
    }

    //[ClientRpc]
    //public void RpcShowInstructions()
    //{
    //    Debug.Log("IsClient " + isServer);
    //    GameInstructions.SetActive(true);
    //}

    //[ClientRpc]
    //public void RpcCreateGame()
    //{
    //    Debug.Log(GameScene);
    //    Debug.Log(GameInstructions);
    //    Destroy(GameInstructions);
    //    GameScene.SetActive(true);
    //}

    //IEnumerator CreateGame()
    //{
    //    yield return new WaitForSeconds(5f);
    //    RpcCreateGame();
    //    StartCoroutine(StartGame());
    //    yield return null;
    //}

    //[ClientRpc]
    //public void RpcStartGame()
    //{
    //    GameScene.GetComponent<LogRotate>().enabled = true;
    //}

    //IEnumerator StartGame()
    //{
    //    yield return new WaitForSeconds(3f);
    //    RpcStartGame();
    //    yield return null;
    //}

    // Update is called once per frame
    public int PlayersAlive()
    {
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

    void TearDown()
    {
        Active = false;
        Next(Players);
    }


}
