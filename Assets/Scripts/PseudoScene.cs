using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NextPseudoScene(List<PlayerStateController> players );

public abstract class PseudoScene : MonoBehaviour {

    public abstract void Run(List<PlayerStateController> players, NextPseudoScene next);
    public abstract void MoveController(PlayerController P);
}
