using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu(fileName = "New Character", menuName = "Scriptable Objects/Characters/Character")]
public class Character : ScriptableObject
{
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
    private string m_startingDismissal;
    public string StartingDismissal { get => m_startingDismissal; private set => m_startingDismissal = value; }

    private void OnValidate()
    {
        m_characterID = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
    }
}