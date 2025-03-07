using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

[CreateAssetMenu(fileName = "New Clue", menuName = "Scriptable Objects/Clues/Clue")]
public class Clue : ScriptableObject
{
    [SerializeField]
    private string m_clueID;
    public string ClueID { get => m_clueID; private set => m_clueID = value; }

    [SerializeField]
    private string m_name;
    public string Name { get => m_name; private set => m_name = value; }

    [SerializeField]
    private string m_clarificationInfo;
    public string ClarificationInfo { get => m_clarificationInfo; private set => m_clarificationInfo = value; }

    [SerializeField, TextArea(5, 20)]
    private string m_description;
    public string Description { get => m_description; private set => m_description = value; }

    [SerializeField]
    private Sprite m_icon;
    public Sprite Icon { get => m_icon; private set => m_icon = value; }

    private void OnValidate()
    {
#if UNITY_EDITOR
        m_clueID = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
#endif
    }
}
