using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    static public T Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Instance == (T)this)
        {
            OnStart();
        }
    }

    virtual protected void OnAwake()
    {
    }

    virtual protected void OnStart()
    {
    }

}

