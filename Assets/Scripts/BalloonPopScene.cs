using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BalloonPopScene : PseudoScene
{

    private NextPseudoScene Next;
    private List<PlayerStateController> Players;
    public GameObject CarrotPrefab;
    public GameObject BalloonPrefab;
    private int BalloonsPopped;
    bool Active;


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
            CmdFire(head.position, head.up, head.rotation, P.name);
        }

    }

    [Command]
    public void CmdFire(Vector3 pos, Vector3 fore, Quaternion rot, string owner ) {
        GameObject carrot = Instantiate(
            CarrotPrefab,
            pos + fore,
            rot
        );

        carrot.transform.Rotate(new Vector3(-90, 0, 0));
        carrot.GetComponent<Rigidbody>().velocity = carrot.transform.forward * 8;
        carrot.GetComponent<Carrot>().Owner = GameObject.Find(owner);

        NetworkServer.Spawn(carrot);

        Destroy(carrot, 3.0f);
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
            Players[i].GetComponent<ChangeMaterial>().UpdateMat(Players[i].GetComponent<PlayerController>().StartingColor);
        }

        Debug.Log("Inside BalloonPopScene Setup");
        Debug.Log("IsServer " + isServer);
        Active = true;
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
        if (BalloonsPopped == 5)
        {
            Debug.Log("GameOver");
            TearDown();
        }
    }
}
