// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class SDictionary<TKey, TValue>: Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] protected List<TKey> keys = new List<TKey>();
    [SerializeField] protected List<TValue> values = new List<TValue>();

    //public SDictionary() { }
    //public SDictionary(SDictionary<TKey, TValue> copy)
    //{
    //    keys = new List<TKey>(copy.Keys);
    //    values = new List<TValue>(copy.Values);
    //}

    public KeyValuePair<TKey, TValue> At(int index)
    {
        return new KeyValuePair<TKey, TValue>(keys[index], values[index]);
    }

    public TKey KeyAt(int index)
    {
        return keys[index];
    }

    public TValue ValueAt(int index)
    {
        return values[index];
    }

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count != values.Count)
            throw new System.Exception(string.Format($"there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for(int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}

[Serializable] public class SDictionaryStringInt: SDictionary<string, int> { }
[Serializable] public class SDictionaryIntString: SDictionary<int, string> { }
