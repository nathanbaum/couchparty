using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace nb2255
{
    public class Wander : NetworkBehaviour
    {

        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public float moveSpeed = 1.5f;
        public float shootProb = 200;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isServer) return;

            //var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            float x = 5f * (Mathf.PerlinNoise(transform.position.x, transform.position.z) - .5f);

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, Time.deltaTime * moveSpeed);

            float rand = Random.Range(1, shootProb);
            if (Mathf.Abs(rand - 5f) < 1)
            {
                CmdFire();
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
    }
}