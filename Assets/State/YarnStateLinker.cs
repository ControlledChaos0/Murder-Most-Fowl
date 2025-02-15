using Yarn.Unity;
using System.Reflection;
using System.Collections.Generic;

public class YarnStateLinker : VariableStorageBehaviour
{
    public override bool TryGetValue<T>(string variableName, out T result)
    {
        FieldInfo f = typeof(State).GetField(variableName);
        if (typeof(T) == f.FieldType)
        {
            result = (T)f.GetValue(GameManager.State);
            return true;
        }
        result = default;
        return false;
    }
    public override void SetValue(string variableName, string stringValue)
    {
        FieldInfo f = typeof(State).GetField(variableName);
        if (f == null) throw new KeyNotFoundException(variableName + " does not exist.");
        if (f.FieldType == typeof(string)) f.SetValue(GameManager.State, stringValue);
    }

    public override void SetValue(string variableName, float floatValue)
    {
        FieldInfo f = typeof(State).GetField(variableName);
        if (f == null) throw new KeyNotFoundException(variableName + " does not exist.");
        if (f.FieldType == typeof(float)) f.SetValue(GameManager.State, floatValue);
    }

    public override void SetValue(string variableName, bool boolValue)
    {
        FieldInfo f = typeof(State).GetField(variableName);
        if (f == null) throw new KeyNotFoundException(variableName + " does not exist.");
        if (f.FieldType == typeof(string)) f.SetValue(GameManager.State, boolValue);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException("Yarn cannot clear all state values.");
    }

    public override bool Contains(string variableName)
    {
        return typeof(State).GetField(variableName) != null;
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
    {
        if (clear)
        {
            Clear();
            return;
        }

        foreach (KeyValuePair<string, float> kvp in floats) SetValue(kvp.Key, kvp.Value);
        foreach (KeyValuePair<string, string> kvp in strings) SetValue(kvp.Key, kvp.Value);
        foreach (KeyValuePair<string, bool> kvp in bools) SetValue(kvp.Key, kvp.Value);
    }

    public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
    {
        Dictionary<string, float> floatVariables = new();
        Dictionary<string, string> stringVariables = new();
        Dictionary<string, bool> boolVariables = new();

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
