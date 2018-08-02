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

    private void Start()
    {
        if( myState == null)
        {
            myState = this.gameObject.GetComponent<PlayerStateController>();
        }

        CurrentMoveController = DefaultController;
    }

    private void DefaultController( PlayerController P ) {
        
        var x = Input.GetMouseButton(0) ? Time.deltaTime * 3.0f : 0;

        P.transform.Translate(0, 0, x);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        GameObject camera = GameObject.Find("Camera Holder");

        camera.transform.position = transform.position;
        transform.rotation = Camera.main.transform.rotation;
        camera.transform.Translate(new Vector3(0f, .6f, 0));

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
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
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
        GetComponent<Renderer>().material.color = Color.blue;
        Debug.Log("LocalPlayer started");
        CmdAddPlayer();
    }
}