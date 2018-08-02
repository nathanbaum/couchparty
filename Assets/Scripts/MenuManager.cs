using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour {

    public List<ITrigger> triggers;
    public Canvas menu;
    public Button basicButton;
    public bool menuOpen = false;
    Canvas menuInstance;


    // Use this for initialization
    void Start() {
        Debug.Log("finished setup!");
        if(menuOpen)
        {
            menuOpen = false;
            toggleMenu();
        }
    }

    public callback setToggle(callback toggle)
    {
        EventTrigger evt = GetComponent<EventTrigger>();
        EventTrigger.Entry ent = new EventTrigger.Entry();
        ent.eventID = EventTriggerType.PointerClick;
        ent.callback.AddListener((data) => { toggle(); });
        evt.triggers.Add(ent);
        return () => { evt.triggers.Remove(ent); };
    }

    public void closeMenu()
    {
        if( menuOpen )
        {
            menuOpen = false;
            Destroy(menuInstance);
        }
    }

    public void toggleMenu()
    {
        //Debug.Log("Hit my parent!");
        if (!menuOpen)
        {
            //Debug.Log("Inside menu instantiation");
            menuOpen = true;
            Canvas newMenu = Instantiate(menu, transform);
            newMenu.transform.position = Camera.main.transform.position + Vector3.forward*4 + Vector3.down*1;
            //Debug.Log("instantiated new menu: " + newMenu);
            for (int i = 0; i < triggers.Count; i++)
            {
                triggers[i].setCallBacks(closeMenu, setToggle);
                Button newButt = Instantiate(basicButton, newMenu.transform);
                newButt.onClick.AddListener(triggers[i].Toggle);
                newButt.GetComponentInChildren<Text>().text = triggers[i].GetName();
            }
            menuInstance = newMenu;
        }
    }

    // Update is called once per frame
    void Update() {
        /*if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ptr = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ptr, out hit))
            {
                if (hit.collider.gameObject == parent)
                {
                    Debug.Log("Hit my parent!");
                    if (!menuOpen)
                    {
                        menuOpen = true;
                        Canvas newMenu = Instantiate(menu, parent.transform);
                        for (int i = 0; i < triggerInstances.Count; i++)
                        {
                            Button newButt = Instantiate(basicButton, newMenu.transform);
                            newButt.onClick.AddListener(triggerInstances[i].Toggle);
                            newButt.GetComponentInChildren<Text>().text = triggerInstances[i].GetName();
                        }
                        menuInstance = newMenu;
                    }
                }
            }

        }*/
    }
}
