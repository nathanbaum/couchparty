using UnityEngine;
using System.Collections;


public class Carrot : MonoBehaviour
{
    public GameObject Owner;

    void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.tag != "Balloon" ) {
            return;
        }

        Owner.GetComponent<PlayerStateController>().CmdAddScore(1);
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}