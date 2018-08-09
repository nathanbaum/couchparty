using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BalloonPopScene : PseudoScene
{

    private NextPseudoScene Next;
    private List<PlayerStateController> Players;
    public GameObject BalloonPrefab;
    private int BalloonsPopped;
    public Transform BalloonSpawnCenter;
    bool Active;
    public GameObject GameInstructions;


    // Use this for initialization
    void Start()
    {
        Active = false;
        BalloonsPopped = 0;
    }


    public override void Run(List<PlayerStateController> players, NextPseudoScene next)
    {
        Players = players;
        Next = next;

        Debug.Log("Balloon Pop Scene Started!");

        SetUp();
    }

    public override void MoveController(PlayerController P)
    {
        Rigidbody rb = P.gameObject.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Transform head = P.gameObject.transform.Find("Body Holder/HEAD");

        if (Input.GetMouseButtonDown(0) && P.isGrounded)
        {
            Debug.Log("trying to fire a carrot!");
            P.CmdFire(head.position, head.up, head.rotation, P.gameObject.name);
        }

    }

    [Command]
    public void CmdSpawnBalloon() {
        if( !Active ) {
            return;
        }
        GameObject balloon = Instantiate(
            BalloonPrefab,
            BalloonSpawnCenter.position,
            BalloonSpawnCenter.rotation
        );

        Vector3 delta = Quaternion.Euler(0, Random.value*360, 0) * new Vector3(Random.value * 4, 0, 0);
        balloon.transform.Translate(delta);

        balloon.GetComponent<Rigidbody>().velocity = balloon.transform.up * 4;

        NetworkServer.Spawn(balloon);

        Destroy(balloon, 7.0f);
    }

    [Command]
    public void CmdAddBalloonPopped() {
        BalloonsPopped++;
    }

    void SetUp()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].gameObject.GetComponent<PlayerController>().RpcSetMoveControls("BalloonPopScene");
        }

        Debug.Log("Inside BalloonPopScene Setup");
        Debug.Log("IsServer " + isServer);
        Active = true;
        StartCoroutine(InstructionTime());
        InvokeRepeating("CmdSpawnBalloon", 10f, 3f);
    }



    [ClientRpc]
    public void RpcActivate(string name)
    {
        Debug.Log(name);
        GameObject obj = GameObject.Find(name);
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
        RpcActivate("Instructions/Timer/BalloonPop");
        yield return new WaitForSeconds(5f);
        RpcDeactivate("Instructions/Timer/BalloonPop");
        yield return new WaitForSeconds(.5f);
        RpcActivate("Instructions/Timer/three");
        yield return new WaitForSeconds(1f);
        RpcDeactivate("Instructions/Timer/three");
        RpcActivate("Instructions/Timer/two");
        yield return new WaitForSeconds(1f);
        RpcDeactivate("Instructions/Timer/two");
        RpcActivate("Instructions/Timer/one");
        yield return new WaitForSeconds(1f);
        RpcDeactivate("Instructions/Timer/one");
        yield return null;
    }


    void TearDown()
    {
        Active = false;
        Next(Players);
    }

    void Update()
    {
        if (!isServer || !Active)
        {
            return;
        }
        if (BalloonsPopped == 8)
        {
            Debug.Log("GameOver");
            TearDown();
        }
    }
}
