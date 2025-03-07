using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

[CreateAssetMenu(fileName = "New Character", menuName = "Scriptable Objects/Characters/Character")]
public class Character : ScriptableObject
{
    [Serializable]
    public struct ClueResponse
    {
        public string clueID;
        public string nodeResponse;
    }

    [SerializeField]
    private string m_characterID;
    public string CharacterID { get => m_characterID; private set => m_characterID = value; }

    [SerializeField]
    private string m_name;
    public string Name { get => m_name; private set => m_name = value; }

    [SerializeField]
    private string m_startingNode;
    public string StartingNode { get => m_startingNode; private set => m_startingNode = value; }

    [SerializeField]
    private string m_startingIdle;
    public string StartingIdle { get => m_startingIdle; private set => m_startingIdle = value; }

    [SerializeField]
    private string m_startingDismissal;
    public string StartingDismissal { get => m_startingDismissal; private set => m_startingDismissal = value; }

    [SerializeField] 
    private List<ClueResponse> m_clueResponses;
    public List<ClueResponse> ClueResponses { get => m_clueResponses; private set => m_clueResponses = value; }

    private void OnValidate()
    {
#if UNITY_EDITOR
        m_characterID = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
#endif
    }
}