using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void callback();
public delegate callback setter(callback cb);

public abstract class ITrigger : MonoBehaviour
{
    public abstract string GetName(); //returns the string name of this trigger
    public abstract void setCallBacks(callback close, setter set);
    public abstract void Toggle(); //toggles the trigger on/off
}