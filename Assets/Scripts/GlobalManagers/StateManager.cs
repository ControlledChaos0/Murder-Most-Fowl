using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MessagePack;

[RequireComponent(typeof(GameManager))]
public class StateManager : MonoBehaviour
{
    private string m_saveDirectory;
    private List<uint> m_saves;

    [SerializeField]
    private StateAsset m_stateOverride;

    [SerializeField]
    private State m_activeState;
    public State ActiveState => m_activeState;

    private void Awake()
    {
        if (m_stateOverride != null) m_activeState = m_stateOverride.State;

        m_saveDirectory = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(m_saveDirectory);

        m_saves = new List<uint>();
        foreach (string s in Directory.GetFiles(m_saveDirectory))
        {
            m_saves.Add(uint.Parse(Path.GetFileNameWithoutExtension(s)));
        }
    }

    private void Start()
    {
        NewState();
    }

    public void NewState()
    {
        m_activeState = new();
        m_activeState.SaveID = AssignSaveID();
        m_activeState.SaveCreated = DateTime.Now;

        m_activeState.CharacterStates = GameManager.CharacterManager.InitCharacterState(m_activeState);
    }

    private uint AssignSaveID()
    {
        uint i = 0;
        while (m_saves.Contains(i)) i++;
        m_saves.Add(i);
        return i;
    }

    public Coroutine Save() 
    {
        if (m_activeState == null) return null;
        m_activeState.LastSaved = DateTime.Now;
        return StartCoroutine(ISave());
    }

    private IEnumerator ISave()
    {
        string path = m_saveDirectory + m_activeState.SaveID + ".sdat";
        using FileStream fs = File.Create(path);
        Task t = MessagePackSerializer.SerializeAsync(fs, m_activeState);
        yield return new WaitUntil(() => t.IsCompleted);
    }

    public Coroutine Load(uint saveID)
    {
        string path = m_saveDirectory + saveID + ".sdat";
        if (!File.Exists(path)) return null;
        return StartCoroutine(ILoad(path));
    }

    private IEnumerator ILoad(string path)
    {
        using FileStream fs = File.OpenRead(path);
        ValueTask<State> t = MessagePackSerializer.DeserializeAsync<State>(fs);
        yield return new WaitUntil(() => t.IsCompleted);
        m_activeState = t.Result;
    }
}
