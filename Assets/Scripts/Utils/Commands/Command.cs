using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Command
{
    protected bool _isReady;
    protected bool _isStarted;

    public bool IsReady
    {
        get => _isReady;                             
    }
    public bool IsStarted
    {
        get => _isStarted;
    }
    public bool IsCompleted
    {
        get => _isStarted && IsCompletedInternal();
    }
    
    protected abstract bool IsCompletedInternal();
    public abstract void Stop();
    protected abstract void ReadyCommand();
    protected abstract void ExecuteCommand();

    public virtual void Ready()
    {
        _isReady = true;
        ReadyCommand();
    }

    public virtual void Execute()
    {
        if (!_isReady)
        {
            ReadyCommand();
        }
        _isStarted = true;
        ExecuteCommand();
    }
}
