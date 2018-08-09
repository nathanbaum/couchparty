using UnityEngine;
using UnityEngine.Networking;

public delegate void MoveController(PlayerController P);

public class PlayerController : NetworkBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float fireClickLength = 1 / 10;
    private float buttonDownTime;
    public PlayerStateController myState = null;
    public MoveController CurrentMoveController;
    public Material Ghost;
    public bool isGrounded = false;
    public Material StartingColor;
    public GameObject CarrotPrefab;
    public float speed = 15f;
    [SyncVar]
    public string MyName;

    private void Start()
    {
        if( myState == null)
        {
            myState = this.gameObject.GetComponent<PlayerStateController>();
        }
        CurrentMoveController = DefaultController;
    }

    private void DefaultController( PlayerController P ) {
        
        var x = Input.GetMouseButton(0) ? Time.deltaTime * 2f : 0;

        P.transform.Translate(Camera.main.transform.forward.x*x, 0, Camera.main.transform.forward.z*x);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Shroom")
        {
            isGrounded = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Shroom")
        {
            isGrounded = false;
        }
    }

    [Command]
    public void CmdFire(Vector3 pos, Vector3 fore, Quaternion rot, string owner)
    {
        GameObject carrot = Instantiate(
            CarrotPrefab,
            pos + fore,
            rot
        );

        carrot.transform.Rotate(new Vector3(-90, 0, 0));
        carrot.GetComponent<Rigidbody>().velocity = carrot.transform.forward * speed;
        carrot.GetComponent<Carrot>().Owner = GameObject.Find(owner);

        NetworkServer.Spawn(carrot);

        Destroy(carrot, 3.0f);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        GameObject camera = GameObject.Find("Camera Holder");
        Transform head = transform.Find("Body Holder/HEAD");

        camera.transform.position = transform.position;
        head.rotation = Camera.main.transform.rotation;
        head.Rotate(new Vector3(-90, 180, 0));
        camera.transform.Translate(new Vector3(0f, .6f, 0f));
        camera.transform.Translate(Camera.main.transform.forward * .8f);

        CurrentMoveController(this);

    }

    [ClientRpc]
    public void RpcSetMoveControls(string sceneName)
    {
        PseudoScene newScene = GameObject.Find("Scenes").GetComponent(sceneName) as PseudoScene;
        CurrentMoveController = newScene.MoveController;
    }

    [ClientRpc]
    public void RpcSnapTo(Vector3 pos)
    {
        transform.position = pos;
    }

    [Command]
    void CmdAddPlayer() {
        GameDirector gd = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        Debug.Log("My state:");
        Debug.Log(myState);
        gd.AddPlayer(myState);
    }

    public override void OnStartLocalPlayer()
    {
        //GetComponent<Renderer>().material.color = Color.blue;
        Debug.Log("LocalPlayer started");
        CmdAddPlayer();
    }
}