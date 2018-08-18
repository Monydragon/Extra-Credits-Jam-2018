using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    //Delegates
    public delegate void D_Void();
    public delegate void D_Generic<T>(T obj);
    public delegate void D_Generic<T1,T2>(T1 obj, T2 obj2);
    public delegate void D_Generic<T1,T2,T3>(T1 obj, T2 obj2, T3 obj3);
    public delegate void D_Generic<T1,T2,T3,T4>(T1 obj, T2 obj2, T3 obj3, T4 obj4);
    public delegate void D_Generic<T1,T2,T3,T4,T5>(T1 obj, T2 obj2, T3 obj3, T4 obj4, T5 obj5);

    //Events
    public static event D_Generic<GameObject,GameObject> OnInteracted;

    public static event D_Generic<bool> OnSetPlayerControl;

    public static event D_Generic<string,string> OnSendMessageText; 


    //Handlers

    /// <summary>
    /// <para>Used to call upon the <see cref="D_Generic{T1,T2}"/>. Source represents the gameobject that invokes the action. and Target represents the target gameobject</para>
    /// <para>EXAMPLE: ObjectInteract(this.gameObject,othergameobject); This gameobject is the source and the othergameobjct is the object you are invoking the actions of. </para>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public static void ObjectInteract(GameObject source, GameObject target) => OnInteracted?.Invoke(source, target);

    /// <summary>
    /// Used to set the player control/input when the value is true movement/input is enabled when false it is disabled.
    /// </summary>
    /// <param name="value"></param>
    public static void SetPlayerControl(bool value) => OnSetPlayerControl?.Invoke(value);

    public static void SendMessage(string message, string name = "") => OnSendMessageText?.Invoke(message, name);

}

