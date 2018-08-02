using UnityEngine;
using UnityEngine.Networking;

public delegate Transform MoveController(Transform T);

public class PlayerController : NetworkBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float fireClickLength = 1 / 10;
    private float buttonDownTime;
    public PlayerStateController myState = null;
    private MoveController CurrentMoveController;

    private void Start()
    {
        if( myState == null)
        {
            myState = this.gameObject.GetComponent<PlayerStateController>();
        }

        setMoveControls(DefaultController);
    }

    private Transform DefaultController( Transform T ) {
        var x = Input.GetMouseButton(0) ? Time.deltaTime * 3.0f : 0;

        T.Translate(0, 0, x);

        return T;
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

        transform.position = CurrentMoveController(transform).position;

    }

    public void setMoveControls( MoveController mc ) {
        CurrentMoveController = mc;
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