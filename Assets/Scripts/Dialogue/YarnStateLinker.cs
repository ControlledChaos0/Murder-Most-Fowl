using Yarn.Unity;
using System.Reflection;
using System.Collections.Generic;
using System;
using GlobalManagers;
using UnityEngine;

public class YarnStateLinker : VariableStorageBehaviour
{
    private Dictionary<string, object> m_yarnVariables;
    private Dictionary<string, Type> m_yarnVariableTypes;

    public YarnStateLinker()
    {
        m_yarnVariables = new();
        m_yarnVariableTypes = new();
    }

    private bool IsYarnVariable(string variableName)
    {
        return variableName.StartsWith("$y_");
    }

    public override bool TryGetValue<T>(string variableName, out T result)
    {
        if (IsYarnVariable(variableName))
        {
            if (!m_yarnVariables.ContainsKey(variableName))
            {
                Debug.LogError($"Variable {variableName} does not exist.");
                result = default;
                return false;
            }

            var value = m_yarnVariables[variableName];

            if (typeof(T).IsAssignableFrom(value.GetType()))
            {
                result = (T)value;
                return true;
            }
            else
            {
                Debug.LogError($"Variable {variableName} exists, but is the wrong type (expected {typeof(T)}, got {value.GetType()})");
                result = default;
                return false;
            }
        }

        variableName = variableName.Replace("$", string.Empty);
        FieldInfo f = typeof(State).GetField(variableName);

        if (f == null)
        {
            Debug.LogError($"Variable {variableName} does not exist.");
            result = default;
            return false;
        }

        if (typeof(T).IsAssignableFrom(f.FieldType))
        {
            result = (T)f.GetValue(GameManager.State);
            return true;
        }
        else
        {
            Debug.LogError($"Variable {variableName} exists, but is the wrong type (expected {typeof(T)}, got {f.FieldType})");
            result = default;
            return false;
        }
    }
    public override void SetValue(string variableName, string stringValue)
    {
        if (IsYarnVariable(variableName))
        {
            m_yarnVariables[variableName] = stringValue;
            m_yarnVariableTypes[variableName] = typeof(string);
            return;
        }

        variableName = variableName.Replace("$", string.Empty);
        FieldInfo f = typeof(State).GetField(variableName);

        if (f == null)
        {
            Debug.LogError($"Variable {variableName} does not exist.");
            return;
        }

        if (f.FieldType == typeof(string)) f.SetValue(GameManager.State, stringValue);
        else Debug.LogError($"Variable {variableName} exists, but is the wrong type (expected {typeof(string)}, got {f.FieldType})");
    }

    public override void SetValue(string variableName, float floatValue)
    {
        if (IsYarnVariable(variableName))
        {
            m_yarnVariables[variableName] = floatValue;
            m_yarnVariableTypes[variableName] = typeof(float);
            return;
        }

        variableName = variableName.Replace("$", string.Empty);
        FieldInfo f = typeof(State).GetField(variableName);

        if (f == null)
        {
            Debug.LogError($"Variable {variableName} does not exist.");
            return;
        }

        if (f.FieldType == typeof(float)) f.SetValue(GameManager.State, floatValue);
        else Debug.LogError($"Variable {variableName} exists, but is the wrong type (expected {typeof(float)}, got {f.FieldType})");
    }

    public override void SetValue(string variableName, bool boolValue)
    {
        if (IsYarnVariable(variableName))
        {
            m_yarnVariables[variableName] = boolValue;
            m_yarnVariableTypes[variableName] = typeof(bool);
            return;
        }

        variableName = variableName.Replace("$", string.Empty);
        FieldInfo f = typeof(State).GetField(variableName);

        if (f == null)
        {
            Debug.LogError($"Variable {variableName} does not exist.");
            return;
        }

        if (f.FieldType == typeof(bool)) f.SetValue(GameManager.State, boolValue);
        else Debug.LogError($"Variable {variableName} exists, but is the wrong type (expected {typeof(bool)}, got {f.FieldType})");
    }

    public override void Clear()
    {
        m_yarnVariables.Clear();
        m_yarnVariableTypes.Clear();
    }

    public override bool Contains(string variableName)
    {
        if (IsYarnVariable(variableName)) return m_yarnVariables.ContainsKey(variableName);

        variableName = variableName.Replace("$", string.Empty);
        return typeof(State).GetField(variableName) != null;
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
    {
        if (clear) Clear();

        foreach (KeyValuePair<string, float> kvp in floats) SetValue(kvp.Key, kvp.Value);
        foreach (KeyValuePair<string, string> kvp in strings) SetValue(kvp.Key, kvp.Value);
        foreach (KeyValuePair<string, bool> kvp in bools) SetValue(kvp.Key, kvp.Value);
    }

    public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
    {
        Dictionary<string, float> floatVariables = new();
        Dictionary<string, string> stringVariables = new();
        Dictionary<string, bool> boolVariables = new();

        foreach (KeyValuePair<string, object> kvp in m_yarnVariables)
        {
            var type = m_yarnVariableTypes[kvp.Key];

            if (type == typeof(float))
            {
                float value = System.Convert.ToSingle(kvp.Value);
                floatVariables.Add(kvp.Key, value);
            }
            else if (type == typeof(string))
            {
                string value = System.Convert.ToString(kvp.Value);
                stringVariables.Add(kvp.Key, value);
            }
            else if (type == typeof(bool))
            {
                bool value = System.Convert.ToBoolean(kvp.Value);
                boolVariables.Add(kvp.Key, value);
            }
        }

        foreach (FieldInfo f in typeof(State).GetFields())
        {
            if (f.FieldType == typeof(float))
                floatVariables.Add(f.Name, (float)f.GetValue(GameManager.State));
            else if (f.FieldType == typeof(string))
                stringVariables.Add(f.Name, (string)f.GetValue(GameManager.State));
            else if (f.FieldType == typeof(bool))
                boolVariables.Add(f.Name, (bool)f.GetValue(GameManager.State));
        }

        return (floatVariables, stringVariables, boolVariables);
    }
}