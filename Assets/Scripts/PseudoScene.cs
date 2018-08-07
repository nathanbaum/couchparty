using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void NextPseudoScene(List<PlayerStateController> players );

public abstract class PseudoScene : NetworkBehaviour {

    public abstract void Run(List<PlayerStateController> players, NextPseudoScene next);
    public abstract void MoveController(PlayerController P);
}
