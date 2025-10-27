using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{    public static PoolManager Instance { get; private set; }
    public Transform Root;

    private Dictionary<string, object> pools = new Dictionary<string, object>();
    private void Awake()
    {
        if (Instance == null)            
        {
            Instance = this;           
            DontDestroyOnLoad(gameObject);

            Root = new GameObject("@PoolRoot").transform;
            Root.SetParent(transform);
        }
        else
        {
            Destroy(gameObject);   
        }
    }
    public void CreatPool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) return; 

        string key = prefab.name; 
        if (pools.ContainsKey(key)) return;

        pools.Add(key, new ObjectPool<T>(prefab, initCount, parent));

    }
    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null) return null;


        if (!pools.TryGetValue(prefab.name, out var box))
        {
            return null;
        }


        var pool = box as ObjectPool<T>;

        if (pool != null)
        {
            return pool.Dequeue();
        }
        else
        {
            return null;
        }
    }

    public void ReturnPool<T>(T instance) where T : MonoBehaviour
    {
        string key = instance.gameObject.name;
        if (instance == null) return;

        if (!pools.TryGetValue(instance.gameObject.name, out var box))
        {
            Destroy(instance.gameObject);
            return;
        }

        var pool = box as ObjectPool<T>;

        if (pool != null)
        {
            pool.Enqueue(instance);
        }

    }
}
