using UnityEngine;

namespace GlobalManagers
{
    [RequireComponent(typeof(SceneManager), typeof(StateManager))]
    public class GameManager : MonoBehaviour
    {
        private static GameManager Instance { get; set; }

        private SceneManager _mSceneManager;
        public static SceneManager SceneManager => Instance._mSceneManager;

        private StateManager _mStateManager;
        public static StateManager StateManager => Instance._mStateManager;
        public static State State => StateManager.ActiveState;

        private ClueManager _mClueManager;
        public static ClueManager ClueManager => Instance._mClueManager;
        private CharacterManager _mCharacterManager;
        public static CharacterManager CharacterManager => Instance._mCharacterManager;
        
        private CursorManager _mCursorManager;
        public static CursorManager CursorManager => Instance._mCursorManager;
    
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;

            _mSceneManager = GetComponent<SceneManager>();
            _mStateManager = GetComponent<StateManager>();
            _mClueManager = GetComponent<ClueManager>();
            _mCharacterManager = GetComponent<CharacterManager>();
            _mCursorManager = GetComponent<CursorManager>();
        }
    }
}