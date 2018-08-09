using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogRotate : NetworkBehaviour
{
    public static float curSpeed = 0;
    public float maxSpeed = 200;
    public float accelTime = 3;
    public GameObject log;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (curSpeed < maxSpeed)
        {
            curSpeed = curSpeed + accelTime * Time.deltaTime;
        }
        log.transform.Rotate(-transform.up, 5f * curSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * curSpeed * Time.deltaTime);
    }

}