using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Command
{
    protected bool _isStarted;

    public bool Started
    {
        get => _isStarted;
    }
    public bool Completed
    {
        get => _isStarted && IsCompleted();
    }
    
    protected abstract bool IsCompleted();
    public abstract void Stop();
    protected abstract void ExecuteCommand();

    public void Execute()
    {
        _isStarted = true;
        ExecuteCommand();
    }
}
