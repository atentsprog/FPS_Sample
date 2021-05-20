using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class ListItemMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance;
    public static List<T> Items = new List<T>();
    protected void Awake()
    {
        Items.Add(this as T);
    }

    protected void OnDestroy()
    {
        Items.Remove(this as T);
    }
}
