using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    static private T _instance = null;

    static public T Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    static public bool Exists { get { return Instance != null; } }

    public bool IsActiveInstance { get { return this == Instance; } }
}
