using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour {
    public GameObject[] parts;
	// Use this for initialization
    public void UpdateMat(Material mat) {
     
        parts = GameObject.FindGameObjectsWithTag("Changeable");
        foreach (GameObject part in parts)
        {
            part.GetComponent<Renderer>().material = mat;
        }
	}
}
