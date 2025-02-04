// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     List that can be used as value in a <see cref="SerializableDictionary{TKey,TValue}"/>.
    /// </summary>
    [Serializable]
    public class SerializableList<T> : IList<T>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<T> list = new List<T>();

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() { }

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }
        
        public void Sort()
        {
            list.Sort();
        }

        public void Sort(Comparison<T> comparison)
        {
            list.Sort(comparison);
        }
    }
}