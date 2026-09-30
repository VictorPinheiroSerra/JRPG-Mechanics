using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [System.Serializable]
    public struct TPair
    {
        public TKey Key;
        public TValue Value;

        public TPair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    [SerializeField] List<TPair> pairs = new List<TPair>();
    Dictionary<TKey, TValue> dictionary;

    public Dictionary<TKey, TValue> Dictionary
    {
        get
        {
            if (dictionary == null)
                RebuildDictionary();
            return dictionary;
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            return Dictionary[key];
        }
        set
        {
            Dictionary[key] = value;

            int index = pairs.FindIndex(p => EqualityComparer<TKey>.Default.Equals(p.Key, key));
            if (index >= 0)
            {
                pairs[index] = new TPair(key, value);
            }
            else
            {
                pairs.Add(new TPair(key, value));
            }
        }
    }

    private void RebuildDictionary()
    {
        dictionary = new Dictionary<TKey, TValue>();
        foreach (TPair pair in pairs)
        {
            if (!dictionary.ContainsKey(pair.Key))
                dictionary.Add(pair.Key, pair.Value);
            else
                Debug.LogWarning($"Duplicate key '{pair.Key}' found in SerializableDictionary.");
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (Dictionary.ContainsKey(key))
            return;
        pairs.Add(new TPair(key, value));
        Dictionary.Add(key, value);
    }

    public List<TKey> Keys() => Dictionary.Keys.ToList();
    public List<TValue> Values() => Dictionary.Values.ToList();
    public Dictionary<TKey, TValue> GetDictionary() => Dictionary;
}
