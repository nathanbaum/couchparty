using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour {

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

    public void TriggerDeath() {
        IsDead = true;
        Debug.Log("TriggerDeath called");
    }

}
