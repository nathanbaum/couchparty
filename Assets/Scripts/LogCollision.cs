using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCollision : MonoBehaviour
{
    public Material deathColor;

    // Use this for initialization
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLISION: " + other.gameObject);
        GameObject player = other.gameObject;
        player.GetComponent<PlayerStateController>().CmdTriggerDeath( 3 - GameObject.Find("Scenes").GetComponent<LogRollScene>().PlayersAlive() );
        player.GetComponent<ChangeMaterial>().UpdateMat(deathColor);

    }

    // Update is called once per frame
    void Start()
    {
        Debug.Log("Collision script loaded");
    }
}
