using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class ClueStringCollider : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    public ClueString clueString;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Pointer Enter {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
