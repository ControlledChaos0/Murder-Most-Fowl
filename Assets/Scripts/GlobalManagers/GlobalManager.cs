using UnityEngine;

namespace Manager
{
    public class GlobalManager : Singleton<GlobalManager>
    {
        private PlayerController _playerManager => PlayerController.Instance;
        private CameraController _cameraController => CameraController.Instance;
        private CommandManager _commandManager => CommandManager.Instance;
        private ScreenManager _screenManager => ScreenManager.Instance;

        void Awake()
        {
            InitializeSingleton();
        }

        // Update is called once per frame
        void Update()
        {
            //_commandManager.UpdateManager();
        }
    }
}