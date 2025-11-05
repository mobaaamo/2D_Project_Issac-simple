using UnityEngine;

public static class GloberManager
{
    private static GameObject _root;

    private static PoolManager _pool;

    private static void Init()
    {
        if (_root == null)
        {
            _root = new GameObject("@Managers");
            Object.DontDestroyOnLoad(_root);
        }

    }
    private static void CreatManager<T>(ref T manager, string name) where T : Component
    {
        if (manager == null)
        {
            Init(); 

            GameObject obj = new GameObject(name);

            manager = obj.AddComponent<T>();

            Object.DontDestroyOnLoad(obj);

            obj.transform.SetParent(_root.transform);
        }
    }

    public static PoolManager Pool
    {
        get
        {
            CreatManager(ref _pool, "PoolManager");
            return _pool;
        }
    }
}