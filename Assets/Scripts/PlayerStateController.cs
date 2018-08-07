using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStateController : NetworkBehaviour {

    public bool IsReady { get; private set; }
    public int Score { get; private set; }
    public bool IsDead { get; private set; }

	// Use this for initialization
	void Start () {
        IsReady = false;
        Score = 0;
        IsDead = false;
	}

    void Update() {
        
    }

    public void AddScore( int n ) {
        Score += n;
    }

    [Command]
    public void CmdTriggerDeath() {
        IsDead = true;
        Debug.Log("TriggerDeath called");
        Score = GameObject.Find("Scenes").GetComponent<LogRollScene>().PlayersAlive();
    }

}
