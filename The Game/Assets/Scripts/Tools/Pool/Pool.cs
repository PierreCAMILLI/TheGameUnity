using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool<T> where T : PoolElement<T>
{
    [SerializeField]
    private T _prefab;

    private HashSet<T> _instantiated;
    private Stack<T> _available;

    public Pool()
    {
        _instantiated = new HashSet<T>();
        _available = new Stack<T>();
    }

    private T Create()
    {
        return GameObject.Instantiate(_prefab);
    }

    private T PickFromPool()
    {
        return _available.Pop();
    }

    private T PickInstance()
    {
        return (_available.Count > 0) ? PickFromPool() : Create();
    }

    private T InstantiateWithWorldCoordinates(Vector3 position, Quaternion rotation, Transform parent)
    {
        T gameObject = PickInstance();
        gameObject.gameObject.SetActive(true);
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.Pool = this;
        _instantiated.Add(gameObject);
        gameObject.transform.SetAsLastSibling();
        gameObject.OnInstantiate();
        return gameObject;
    }
    
    private T InstantiateWithLocalCoordinates(Vector3 position, Quaternion rotation, Transform parent)
    {
        T gameObject = PickInstance();
        gameObject.gameObject.SetActive(true);
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;
        gameObject.Pool = this;
        _instantiated.Add(gameObject);
        gameObject.transform.SetAsLastSibling();
        gameObject.OnInstantiate();
        return gameObject;
    }

    public T Instantiate(Vector3 position, Quaternion rotation, Transform parent, bool worldPosition)
    {
        if (worldPosition)
        {
            return InstantiateWithWorldCoordinates(position, rotation, parent);
        } else
        {
            return InstantiateWithLocalCoordinates(position, rotation, parent);
        }
    }

    public T Instantiate(Vector3 position, Quaternion rotation, Transform parent)
    {
        return InstantiateWithWorldCoordinates(position, rotation, parent);
    }

    public T Instantiate()
    {
        return InstantiateWithWorldCoordinates(_prefab.transform.position, _prefab.transform.rotation, null);
    }

    public void Release(T gameObject)
    {
        if (_instantiated.Remove(gameObject))
        {
            _available.Push(gameObject);
            gameObject.OnRelease();
            gameObject.gameObject.SetActive(false);
        }
    }

    public void Free()
    {
        foreach (T gameObject in _available)
        {
            GameObject.Destroy(gameObject.gameObject);
        }
        _available.Clear();
    }
}

public abstract class PoolElement<T> : MonoBehaviour where T : PoolElement<T>
{
    public Pool<T> Pool
    {
        get; set;
    }

    public void Release()
    {
        this.Pool.Release((T) this);
    }

    public virtual void OnInstantiate() { }
    public virtual void OnRelease() { }
}
