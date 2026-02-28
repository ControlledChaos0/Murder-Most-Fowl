using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class CommandManager : Singleton<CommandManager>
    {
        private Queue<Command> _commandQueue;
        private Command _currentCommand;

        private void Awake()
        {
            InitializeSingleton();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _commandQueue = new Queue<Command>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateQueue();
        }

        public void Queue(Command command)
        {
            _commandQueue.Enqueue(command);
        }

        public void ClearQueue()
        {
            ClearCommand();
            _commandQueue.Clear();
            PlayerController.Instance.Interactable = null;
        }

        private void ClearCommand()
        {
            if (_currentCommand != null && !_currentCommand.IsCompleted)
            {
                _currentCommand.Stop();
            }
            _currentCommand = null;
        }

        private void UpdateQueue()
        {
            if (_currentCommand != null && _currentCommand.IsCompleted)
            {
                _currentCommand = null;
            }    
            if (_currentCommand == null && _commandQueue.Count > 0)
            {
                Dequeue();
                UpdateQueue();
            }
        }
        private void Dequeue()
        {
            _currentCommand = _commandQueue.Dequeue();
            _currentCommand.Execute();

            if (_commandQueue.Count > 0)
            {
                _commandQueue.Peek().Ready();
            }
        }
    }
}
