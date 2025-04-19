using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class CharacterOverworld : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string characterID;
    [SerializeField] private string yarnNode => GetCurrentNode();
    [SerializeField] private PlayerController player;

    public void OnPointerClick(PointerEventData eventData)
    {
        player.Move(new Ray(GetComponent<Transform>().position, CameraController.Instance.CameraTransform.forward));
        StartCoroutine(waitUntilStopMoving());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Inspect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Default);
    }

    public string GetCurrentNode()
    {
        return GameManager.CharacterManager.GetCurrentNode(characterID);
    }

    public string GetCurrentDismissal()
    {
        return GameManager.CharacterManager.GetCurrentDismissal(characterID);
    }

    public IEnumerator waitUntilStopMoving()
    {
        while(player.getIsMoving()) 
        {
            yield return new WaitForSeconds(0f);
        }
        realOnPointerClick();
        
    }

    public void realOnPointerClick() {
        string currNode = GetCurrentNode();
        if (currNode == "")
        {
            return;
        }
        DialogueHelper.Instance.DialogueRunner.StartDialogue(currNode);
    }
}
