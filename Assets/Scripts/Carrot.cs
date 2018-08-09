using UnityEngine;
using System.Collections;


public class Carrot : MonoBehaviour
{
    public GameObject Owner;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit something!");
        if( collision.gameObject.tag != "Balloon" ) {
            return;
        }
        Debug.Log("It was a balloon!");

        Owner.GetComponent<PlayerStateController>().CmdAddScore(1);
        GameObject.Find("Scenes").GetComponent<BalloonPopScene>().CmdAddBalloonPopped();
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}