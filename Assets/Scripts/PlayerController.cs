using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float fireClickLength = 1 / 10;
    private float buttonDownTime;
    private PlayerStateController myState;

    private void Start()
    {
        myState = new PlayerStateController();
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
        

        var x = Input.GetMouseButton(0) ? Time.deltaTime * 3.0f : 0;

        transform.Translate(0, 0, x);

        if (Input.GetMouseButtonDown(0))
        {
            buttonDownTime = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - buttonDownTime <= fireClickLength)
            {
                CmdFire();
            }
        }
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
        gd.AddPlayer(myState);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        CmdAddPlayer();
    }
}