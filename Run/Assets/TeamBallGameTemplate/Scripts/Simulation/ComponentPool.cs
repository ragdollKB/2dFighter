using System;
using System.Collections.Generic;
using UnityEngine;


namespace TeamBallGame
{
    public class ComponentPool<T> where T : Component
    {
        static ComponentPool<T> Instance = new ComponentPool<T>();

        struct PendingReturn : IComparable<PendingReturn>
        {
            public float time;
            public T instance;
            public int CompareTo(PendingReturn other) => time.CompareTo(other.time);
        }

        Dictionary<int, Stack<T>> pools = new Dictionary<int, Stack<T>>();
        Dictionary<int, int> instances = new Dictionary<int, int>();
        HeapQueue<PendingReturn> pendingReturns = new HeapQueue<PendingReturn>();

        static public void Prewarm(T prefab, int count)
        {
            var pool = Instance.GetPool(prefab.GetInstanceID());
            for (var i = 0; i < count; i++)
                Instance.CreateInstance(prefab, pool);
        }

        static public T Take(T prefab)
        {
            var poolKey = prefab.GetInstanceID();
            var pool = Instance.GetPool(poolKey);
            var returns = Instance.pendingReturns;
            T g;
            if (pool.Count == 0)
            {
                while (returns.Count > 0 && returns.Peek().time <= Time.time)
                {
                    Return(returns.Pop().instance);
                }
            }
            if (pool.Count == 0)
            {
                g = Instance.CreateInstance(prefab, pool);
            }
            else
                g = pool.Pop();
            Instance.instances[g.GetInstanceID()] = poolKey;
            g.gameObject.SetActive(true);
            return g;
        }

        static public void Return(T instance, float when)
        {
            Instance.pendingReturns.Push(new PendingReturn() { time = Time.time + when, instance = instance });
        }

        static public void Return(T instance)
        {
            if (instance == null)
                Debug.Log("Cannot return a null instance.");
            else
            {
                int poolKey;
                var instanceKey = instance.GetInstanceID();
                if (Instance.instances.TryGetValue(instanceKey, out poolKey))
                {
                    var pool = Instance.pools[poolKey];
                    instance.gameObject.SetActive(false);
                    Instance.instances.Remove(instanceKey);
                    pool.Push(instance);
                }
                else
                {
                    Debug.LogWarning("Cannot return an instance that was not taken from a pool.", instance.gameObject);
                }
            }
        }

        T CreateInstance(T prefab, Stack<T> pool)
        {
            var g = GameObject.Instantiate(prefab);
            g.gameObject.SetActive(false);
            return g;
        }

        Stack<T> GetPool(int key)
        {
            Stack<T> pool;
            if (!pools.TryGetValue(key, out pool))
                pool = pools[key] = new Stack<T>();
            return pool;
        }
    }
}


