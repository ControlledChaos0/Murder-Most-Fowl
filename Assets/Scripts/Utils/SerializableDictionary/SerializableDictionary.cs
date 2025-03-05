using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<KeyType, ValueType> : Dictionary<KeyType, ValueType>, ISerializationCallbackReceiver
{
    public List<KeyType> SerializedKeys = new();
    public List<ValueType> SerializedValues = new();
    
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        SynchroniseToSerializedData();
    }

#if UNITY_EDITOR
    public void EditorOnly_Add(KeyType key, ValueType value)
    {
        SerializedKeys.Add(key);
        SerializedValues.Add(value);
    }
#endif
    
    public void SynchroniseToSerializedData()
    {
        this.Clear();
        if ((SerializedKeys != null) && (SerializedValues != null))
        {
            int numElements = Mathf.Min(SerializedKeys.Count, SerializedValues.Count);
            for (int i = 0; i < numElements; ++i)
            {
                this[SerializedKeys[i]] = SerializedValues[i];
            }
        }
        else
        {
            SerializedKeys = new();
            SerializedValues = new List<ValueType>();
        }
        if (SerializedKeys.Count != SerializedValues.Count)
        {
            SerializedKeys = new(Keys);
            SerializedValues = new(Values);
        }
    }
}
