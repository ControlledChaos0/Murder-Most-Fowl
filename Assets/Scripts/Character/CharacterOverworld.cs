using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class CharacterOverworld : MonoBehaviour,
    IPointerClickHandler
{
    [SerializeField] private string yarnNode;
    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueHelper.Instance.DialogueRunner.StartDialogue(yarnNode);
    }

    [YarnCommand("set_node")]
    public void SetNode(string node)
    {
        yarnNode = node;
    }
}
