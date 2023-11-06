using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW.ECS
{
    public class GOPool : MonoBehaviour
    {
        [SerializeField]
        private List<PoolObject> _objects;

        private Dictionary<string, PoolObject> _templates;
        private Dictionary<string, Queue<Transform>> _pools;

        private void Awake()
        {
            _templates = new Dictionary<string, PoolObject>();
            _pools = new Dictionary<string, Queue<Transform>>();

            foreach (var obj in _objects)
            {
                _templates[obj.id] = obj;
                _pools[obj.id] = new Queue<Transform>();
            }
        }

        public T Get<T>(string id)
        {
            _pools.TryGetValue(id, out Queue<Transform> pool);
            if (pool == null)
                return default(T);

            if (pool.Count > 0)
            {
                var obj = pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj.GetComponent<T>();
            }
            else
            {
                var obj = GameObject.Instantiate(_templates[id]);
                return obj.GetComponent<T>();
            }
        }

        public void Return<T>(T component) where T : Component
        {
            Return(component.transform);
        }

        public void Return(Transform transform)
        {
            var poolObject = transform.GetComponent<PoolObject>();
            if (poolObject != null)
            {
                transform.gameObject.SetActive(false);
                _pools[poolObject.id].Enqueue(transform);
            }
        }

        public void Return(GameObject obj)
        {
            Return(obj.transform);
        }
    }
}
