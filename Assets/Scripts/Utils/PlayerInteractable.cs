using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerInteractable : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController.Instance.Move(new Ray(GetComponent<Transform>().position, CameraController.Instance.CameraTransform.forward));
        StartCoroutine(WaitUntilStopMoving());
    }

    protected IEnumerator WaitUntilStopMoving()
    {
        while (PlayerController.Instance.IsMoving)
        {
            yield return null;
        }
        OnPointerClick();
    }

    protected abstract void OnPointerClick();
}
