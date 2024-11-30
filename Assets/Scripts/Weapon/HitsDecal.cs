using UnityEngine;
using System.Collections.Generic;

public class HitsDecal : MonoBehaviour
{
    [SerializeField] private float _decalLifetime = 10f;
    private ObjectPool _pool;

    public void Initialize(Vector3 position, Vector3 normal, ObjectPool pool, Transform parent)
    {
        transform.parent = parent;
        transform.position = position + normal * 0.01f;
        transform.rotation = Quaternion.LookRotation(normal);
        _pool = pool;
        Invoke(nameof(ReturnToPool), _decalLifetime);
    }

    private void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.ReturnObject(gameObject);
        }
    }
}
