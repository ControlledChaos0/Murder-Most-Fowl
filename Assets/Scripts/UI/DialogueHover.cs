using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DialogueHover : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueIndicator;

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private float scaleDuration = 0.5f;
    Coroutine dialogueAnimation = null;

    [SerializeField] 
    private GameObject _background;

    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = dialogueIndicator.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        dialogueIndicator.transform.localScale = new Vector3(0.2f, 0.2f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (_background.activeSelf && dialogueAnimation != null) 
        {
            sr.enabled = false;
            StopCoroutine(dialogueAnimation);
            dialogueIndicator.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }
    }

    void OnMouseEnter()
    {
        if (!_background.activeSelf)
        {
            sr.enabled = true;
            dialogueAnimation = StartCoroutine(openDialogue());
        }
    }

    void OnMouseExit()
    {
        if (dialogueAnimation != null)
        {
            sr.enabled = false;
            StopCoroutine(dialogueAnimation);
            dialogueIndicator.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }
    }

    public IEnumerator openDialogue()
    {
        float startSize = 0.2f;
        float startTime = Time.time;
        while (Time.time - startTime < scaleDuration)
        {
            float t = (Time.time - startTime) / scaleDuration;
            float scale = Mathf.Lerp(startSize, 0.45f, curve.Evaluate(t));
            dialogueIndicator.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }

}
