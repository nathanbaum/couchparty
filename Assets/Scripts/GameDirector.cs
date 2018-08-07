using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameDirector : NetworkBehaviour {
    
    public List<PseudoScene> scenes;
    private int currentScene;
    public List<PlayerStateController> Players { get; private set; }
    private bool gameStarted;

    void Start() {
        if (!isServer)
        {
            return;
        }
        Players = new List<PlayerStateController>();
        gameStarted = false;
        currentScene = -1;

    }

    void Next( List<PlayerStateController> players ) {
        if (!isServer)
        {
            return;
        }
        Players = players;
        currentScene++;
        scenes[currentScene].Run(Players, Next);
    }

    public void AddPlayer( PlayerStateController player ) {
        if (!isServer)
        {
            return;
        }
        Players.Add(player);
        Debug.Log("Added player");
        Debug.Log("There are now " + Players.Count + "players in the game.");
    }

    private void Update()
    {
        if(!isServer) {
            return;
        }

        if( !gameStarted && Players.Count == 3 ) {
            gameStarted = true;
            Next(Players);
        }
    }
}
