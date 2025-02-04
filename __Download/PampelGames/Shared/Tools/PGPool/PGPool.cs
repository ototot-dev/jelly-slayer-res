// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Pool class that can preload objects.
    /// </summary>
    public static class PGPool
    {
        private static readonly Dictionary<int, PoolDescr> poolDictionary = new();

        private class PoolDescr
        {
            public PoolDescr(GameObject prefab, ObjectPool<GameObject> pool)
            {
                this.pool = pool;
                poolDictionary.Add(prefab.GetInstanceID(), this);
            }
            
            public ObjectPool<GameObject> pool;
        }
        
#if UNITY_EDITOR

        private static PlayModeStateChange currentPlayMode;
        [InitializeOnLoadMethod]
        private static void EditorInit()
        {
            EditorApplication.playModeStateChanged -= Change; 
            EditorApplication.playModeStateChanged += Change;  
        }
    
        private static void Change(PlayModeStateChange state)
        {
            currentPlayMode = state;
            if (state != PlayModeStateChange.EnteredEditMode) return;
            poolDictionary.Clear();
        }
#endif

        /********************************************************************************************************************************/
        /* Public ***********************************************************************************************************************/
        
        /// <summary>
        ///     Checks for an existing pool of a prefab.
        /// </summary>
        /// <returns>Returns existing pool or null.</returns>
        public static ObjectPool<GameObject> TryGetExistingPool(GameObject prefab)
        {
            return poolDictionary.TryGetValue(prefab.GetInstanceID(), out var poolsStruct) ? poolsStruct.pool : null;
        }
        

        /// <summary>
        ///     Preloads objects into the scene using Pool.Get() and Pool.Release().
        /// </summary>
        /// <param name="prefab">Prefab used for dictionary look up.</param>
        /// <param name="_pool">Existing pool.</param>
        /// <param name="loadAmount">If pool exists, only loads until pooled count is equal to the loadAmount.</param>
        /// <returns>Newly created objects.</returns>
        public static GameObject[] Preload(GameObject prefab, ObjectPool<GameObject> _pool, int loadAmount, bool limited = false)
        {
            if (!poolDictionary.TryGetValue(prefab.GetInstanceID(), out var poolDescr))
            {
                poolDescr = new PoolDescr(prefab, _pool);
            }

            if (loadAmount <= poolDescr.pool.CountAll) return Array.Empty<GameObject>();

            var newLoadedObjects = new GameObject[loadAmount - poolDescr.pool.CountAll];
            for (var i = 0; i < newLoadedObjects.Length; i++)
            {
                newLoadedObjects[i] = poolDescr.pool.Get();

                if (!newLoadedObjects[i].TryGetComponent<PGPoolable>(out var pgPoolable))
                    newLoadedObjects[i].AddComponent<PGPoolable>();
                
                var pgPoolables = newLoadedObjects[i].GetComponentsInChildren<PGPoolable>();
                foreach (var poolable in pgPoolables) poolable.prefab = prefab;
            }

            for (var i = newLoadedObjects.Length - 1; i >= 0; i--) poolDescr.pool.Release(newLoadedObjects[i]);

            return newLoadedObjects;
        }

        /// <summary>
        ///     Spawns a <see cref="UnityEngine.GameObject" /> from an initialized pool.
        ///     Creates a new pool if none is found for the prefab.
        /// </summary>
        /// <param name="prefab">Prefab to search for.</param>
        /// <returns>The Object spawned into the scene.</returns>
        public static GameObject Get(GameObject prefab)
        {
            if (!poolDictionary.TryGetValue(prefab.GetInstanceID(), out var poolDescr))
            {
                var pool = new ObjectPool<GameObject>(
                    () => CreateSetup(prefab),
                    GetSetup,
                    ReleaseSetup,
                    DestroySetup);

                poolDescr = new PoolDescr(prefab, pool);
            }

            GameObject obj;
            if (poolDescr.pool.CountInactive == 0)
            {
                obj = poolDescr.pool.Get();

                if (!obj.TryGetComponent<PGPoolable>(out var pgPoolable))
                    obj.AddComponent<PGPoolable>();
            }
            else
            {
                obj = poolDescr.pool.Get();    
            }
            
            var pgPoolables = new List<PGPoolable>();
            pgPoolables.AddRange(obj.GetComponentsInChildren<PGPoolable>());
            
            if (pgPoolables.Count == 0)
            {
                pgPoolables.Add(obj.AddComponent<PGPoolable>());
            }

            foreach (var pgPoolable in pgPoolables)
            {
                pgPoolable.pooled = false;
                pgPoolable.prefab = prefab;
                pgPoolable.OnPoolSpawn();
            }

            return obj;
        }

        /// <summary>
        ///     Releases a <see cref="UnityEngine.GameObject" /> to an existing pool.
        /// </summary>
        /// <param name="obj">Object that was spawned into the scene. Must be stored by the using class.</param>
        public static void Release(GameObject obj)
        {
#if UNITY_EDITOR
            if (currentPlayMode == PlayModeStateChange.ExitingPlayMode)
            {
                return;
            }
#endif
            if (obj == null) return;
            if (!obj.scene.isLoaded) return;
            
            List<PGPoolable> pgPoolables = new List<PGPoolable>();
            pgPoolables.AddRange(obj.GetComponentsInChildren<PGPoolable>());;
            if (pgPoolables.Count == 0)
            {
                Debug.LogWarning(obj.name + " has no PG_Poolable component and will be destroyed!");
                Object.Destroy(obj);
                return;
            }

            if (!poolDictionary.TryGetValue(pgPoolables[0].prefab.GetInstanceID(), out var foundPoolStruct))
            {
                Debug.LogWarning("No pool found for " + obj.name);
                Object.Destroy(obj);
                return;
            }

            foreach (var pgPoolable in pgPoolables)
            {
                if (pgPoolable.pooled) return;
                pgPoolable.pooled = true;
                pgPoolable.OnPoolUnSpawn();
            }

            foundPoolStruct.pool.Release(obj);
        }
        
        /********************************************************************************************************************************/
        
        public static int GetCountActive(GameObject prefab)
        {
            return poolDictionary.TryGetValue(prefab.GetInstanceID(), out var foundPoolStruct) ? foundPoolStruct.pool.CountActive : 0;
        }

        public static int GetCountInactive(GameObject prefab)
        {
            return poolDictionary.TryGetValue(prefab.GetInstanceID(), out var foundPoolStruct) ? foundPoolStruct.pool.CountInactive : 0;
        }
        
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        private static GameObject CreateSetup(GameObject prefab)
        {
            var obj = Object.Instantiate(prefab);
            return obj;
        }

        private static void GetSetup(GameObject obj)
        {
            obj.SetActive(true);
        }

        private static void ReleaseSetup(GameObject obj)
        {
            obj.transform.SetParent(null);
            obj.SetActive(false);
        }

        private static void DestroySetup(GameObject obj)
        {
            Object.Destroy(obj);
        }

        

    }
}