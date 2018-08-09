using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameDirector : NetworkBehaviour {
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public List<PseudoScene> scenes;
    private int currentScene;
    public List<PlayerStateController> Players { get; private set; }
    private bool gameStarted;
    private int playercounter;

    void Start() {
        if (!isServer)
        {
            return;
        }
        Players = new List<PlayerStateController>();
        gameStarted = false;
        currentScene = -1;
        playercounter = 0;
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
        player.gameObject.name = "player" + Players.Count.ToString();
        player.RpcUpdateName(player.gameObject.name);
        switch (Players.Count)
        {
            case 1:
                player.GetComponent<ChangeMaterial>().UpdateMat(mat1);
                player.GetComponent<PlayerController>().StartingColor = mat1;
                break;
            case 2:
                player.GetComponent<ChangeMaterial>().UpdateMat(mat2);
                player.GetComponent<PlayerController>().StartingColor = mat2;
                break;
            case 3:
                player.GetComponent<ChangeMaterial>().UpdateMat(mat3);
                player.GetComponent<PlayerController>().StartingColor = mat3;
                break;
        }
    }

    private void Update()
    {
        if(!isServer) {
            return;
        }

        if( !gameStarted && Players.Count == 2 ) {
            gameStarted = true;
            Next(Players);
        }
    }
}
