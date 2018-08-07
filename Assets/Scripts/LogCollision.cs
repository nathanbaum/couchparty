using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCollision : MonoBehaviour
{
    public Material deathColor;

    // Use this for initialization
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION: " + collision.rigidbody.gameObject);
        var player = collision.rigidbody.gameObject;
        player.GetComponent<Renderer>().material = deathColor;
    }

    // Update is called once per frame
    void Start()
    {
        Debug.Log("Collision script loaded");
    }
}