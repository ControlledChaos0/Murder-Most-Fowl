using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MagnifyingController : MonoBehaviour
{
    private Vector3 mousePosition;

    [SerializeField]
	private float moveSpeed = 0.1f;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private float scaleDuration = 0.5f;

    [SerializeField]
    private Sprite m_spritaA;
    [SerializeField]
    private Sprite m_spritaB;
    private SpriteRenderer m_spriteRenderer;

    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer.sprite = m_spritaA;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }

    public IEnumerator IncreaseSize()
    {
        m_spriteRenderer.sprite = m_spritaB;
        float startSize = transform.localScale.x;
        float startTime = Time.time;
        while (Time.time - startTime < scaleDuration)
        {
            float t = (Time.time - startTime) / scaleDuration;
            float scale = Mathf.Lerp(startSize, 0.8f, curve.Evaluate(t));
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }

    public IEnumerator ResetSize()
    {
        m_spriteRenderer.sprite = m_spritaA;
        float startSize = transform.localScale.x;
        float startTime = Time.time;
        while (Time.time - startTime < scaleDuration)
        {
            float t = (Time.time - startTime) / scaleDuration;
            float scale = Mathf.Lerp(startSize, 0.6f, curve.Evaluate(t));
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }
}
