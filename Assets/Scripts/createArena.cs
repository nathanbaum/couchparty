using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateArena : NetworkBehaviour {

    public int numPlayers = 4;                           //number of points on radius to place prefabs
    public Vector3 centerPos = new Vector3(0, 0, 32);    //center of circle/elipsoid

    public GameObject pointPrefab;                       //generic prefab to place on each point
    public float radius;                                //radii for each x,y axes, respectively

    Vector3 pointPos;                                //position to place each prefab along the given circle/eliptoid
                                                     //*is set during each iteration of the loop

    // Use this for initialization
    public override void OnStartServer()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            //multiply 'i' by '1.0f' to ensure the result is a fraction
            float pointNum = (i * 1.0f) / numPlayers;

            //angle along the unit circle for placing points
            float angle = pointNum * Mathf.PI * 2;

            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            //position for the point prefab

            pointPos = new Vector3(x, 0, y) + centerPos;


            //place the prefab at given position
            Instantiate(pointPrefab, pointPos, Quaternion.identity);
        }

    }
}
