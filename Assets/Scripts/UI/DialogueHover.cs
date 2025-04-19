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

    private SpriteRenderer sr;
    private float _scale;
    private float _startScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = dialogueIndicator.GetComponent<SpriteRenderer>();
        _scale = sr.transform.localScale.x;
        _startScale = _scale / 3.0f;
        sr.enabled = false;
        dialogueIndicator.transform.localScale = new Vector3(_startScale, _startScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueAnimation == null)
        {
            sr.enabled = false;
            dialogueIndicator.transform.localScale = new Vector3(_startScale, _startScale, 1);
        }
    }

    void OnMouseEnter()
    {
        sr.enabled = true;
        dialogueAnimation = StartCoroutine(openDialogue());
    }

    void OnMouseExit()
    {
        if (dialogueAnimation != null)
        {
            sr.enabled = false;
            StopCoroutine(dialogueAnimation);
            dialogueIndicator.transform.localScale = new Vector3(_startScale, _startScale, 1);
        }
    }

    public IEnumerator openDialogue()
    {
        float startTime = Time.time;
        while (Time.time - startTime < scaleDuration)
        {
            float t = (Time.time - startTime) / scaleDuration;
            float scale = Mathf.Lerp(_startScale, _scale, curve.Evaluate(t));
            dialogueIndicator.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }

}
