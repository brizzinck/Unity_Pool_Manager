using System.Collections.Generic;
using UnityEngine;

namespace Optimization
{
    public class PoolMono<T> where T : MonoBehaviour
    {
        public Transform Container { get; }
        public List<T> Pool => _pool;
        private T _prefab;
        private List<T> _freeElements = new List<T>();
        private bool _autoExpand;
        private List<T> _pool;

        public PoolMono(T prefab, int countInPool, bool autoExpand, Transform container = null)
        {
            _prefab = prefab;
            Container = container;
            _autoExpand = autoExpand;
            CreatePool(countInPool);
        }
        public bool HasFreeElement(out T element, bool activeted)
        {
            foreach (var mono in _pool)
                if (!mono.gameObject.activeInHierarchy)
                    _freeElements.Add(mono);

            if (_freeElements.Count == 0)
            {
                element = null;
                return false;
            }
            element = _freeElements[Random.Range(0, _freeElements.Count)];
            element.gameObject.SetActive(activeted);
            _freeElements.Clear();
            return true;

        }
        public T GetFreeElement(bool activeted = false)
        {
            if (HasFreeElement(out var element, activeted))
                return element;
            if (_autoExpand)
                return CreateObject(true);
            throw new System.Exception($"There is no elements in pool of type {typeof(T)}");
        }

        private void CreatePool(int countInPool)
        {
            _pool = new List<T>();

            for (int i = 0; i < countInPool; i++) 
                CreateObject();
        }
        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObj = Object.Instantiate(this._prefab, this.Container);
            createdObj.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObj);
            return _pool[^1];
        }
    }
}
