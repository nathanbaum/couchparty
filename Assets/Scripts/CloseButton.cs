using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nb2255
{
    public class CloseButton : ITrigger
    {
        GameObject menu;
        bool triggered = false;
        callback closeMenu;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override string GetName()
        {
            return "Close";
        }

        public override void setCallBacks(callback close, setter setToggle)
        {
            closeMenu = close;
        }

        public override void Toggle()
        {
            if( !triggered )
            {
                closeMenu();
            }
        }
    }
}