using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]

    [SerializeField]
    private List<FMODUnity.EventReference> musicList;

    [SerializeField] private int startMusicIndex;


    private FMOD.Studio.Bus _musicBus;
    private FMOD.Studio.EventInstance _musicState;

    void Awake()
    {
        InitializeSingleton();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");

        _musicState = FMODUnity.RuntimeManager.CreateInstance(musicList[startMusicIndex]);
        _musicState.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        StopAllMusicEvents();

        //--------------------------------------------------------------------
        // 6: This shows how to release resources when the unity object is 
        //    disabled.
        //--------------------------------------------------------------------
        _musicState.release();
    }

    void StopAllMusicEvents()
    {
        _musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void ChangeBGM(int index)
    {
        if (index >= musicList.Count)
        {
            return;
        }

        _musicState.release();
        _musicState = FMODUnity.RuntimeManager.CreateInstance(musicList[index]);
        _musicState.start();
    }

    public void PauseEffect(bool paused)
    {
        float pauseVal = paused ? 1.0f : 0.0f;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause", pauseVal);
    }
}
