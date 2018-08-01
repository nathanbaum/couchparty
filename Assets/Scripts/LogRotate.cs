using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogRotate : NetworkBehaviour
{
    public float speed = 10f;
    public GameObject log;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {

            return;
        }
        //var qTo = new Vector3(0f, 0f, Random.Range(-88f, -92f));
        //log.transform.localEulerAngles = Vector3.Slerp(log.transform.localEulerAngles, qTo, speed * Time.deltaTime);
        log.transform.Rotate(-transform.up, 5f * speed * Time.deltaTime);
        //log.transform.localEulerAngles = new Vector3(0, 0, 90 + 50 * (Mathf.PerlinNoise(speed * .5f * Time.deltaTime, 0) - .5f));
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}