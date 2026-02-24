using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Command
{
    private bool _isStarted;
    
    public abstract bool IsCompleted();
    public abstract void Stop();
    protected abstract void ExecuteCommand();

    public void Execute()
    {
        _isStarted = true;
        ExecuteCommand();
    }
    public bool IsStarted()
    {
        return _isStarted;
    }
}
