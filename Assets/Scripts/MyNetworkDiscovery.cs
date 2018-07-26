using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace nb2255
{
    public class MyNetworkDiscovery : NetworkDiscovery
    {
        public NetworkManager nm = null;

        // Use this for initialization
        void Start()
        {
            if (nm == null)
            {
                nm = FindObjectOfType<NetworkManager>();
            }
        }

        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            Debug.Log("got broadcast!");
            Debug.Log(fromAddress);
            Debug.Log("port: " + data);
            nm.networkAddress = fromAddress;
            nm.networkPort = int.Parse(data);
            nm.StartClient();
        }
    }
}