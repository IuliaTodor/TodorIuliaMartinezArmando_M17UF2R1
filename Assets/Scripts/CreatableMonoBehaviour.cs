using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatableMonoBehaviour : MonoBehaviour
{
    protected static T Create<T>(GameObject gameObject = null) where T : MonoBehaviour 
    {
        GameObject obj = gameObject;
        if (obj == null) obj = new GameObject(typeof(T).ToString());

        return obj.AddComponent<T>();
    }
}
