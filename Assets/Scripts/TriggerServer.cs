using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

namespace nb2255
{
    public class TriggerServer : ITrigger
    {
        private callback closeMenu;

        void Start()
        {
            NetworkManager.singleton.StartMatchMaker();
        }

        public override string GetName()
        {
            return "Start Game";
        }

        public override void Toggle()
        {
            NetworkManager.singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);

            closeMenu();
        }

        public override void setCallBacks(callback close, setter set)
        {
            closeMenu = close;
        }

        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            if (success)
            {
                if (matches.Count != 0)
                {
                    //Debug.Log("A list of matches was returned");

                    //join the last server (just in case there are two...)
                    NetworkManager.singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinMatch);
                }
                else
                {
                    Debug.Log("No matches in requested room!");
                    //so let's make a match
                    NetworkManager.singleton.matchMaker.CreateMatch("", 4, true, "", "", "", 0, 0, OnMatchCreate);
                }
            }
            else
            {
                Debug.LogError("Couldn't connect to match maker");
            }
        }

        public void OnJoinMatch(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (success)
            {
                //Debug.Log("Able to join a match");

                MatchInfo hostInfo = matchInfo;
                NetworkManager.singleton.StartClient(hostInfo);
            }
            else
            {
                Debug.LogError("Join match failed");
            }
        }

        public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (success)
            {
                //Debug.Log("Create match succeeded");

                MatchInfo hostInfo = matchInfo;
                NetworkServer.Listen(hostInfo, 9000);

                NetworkManager.singleton.StartHost(hostInfo);
            }
            else
            {
                Debug.LogError("Create match failed");
            }
        }
    }
}