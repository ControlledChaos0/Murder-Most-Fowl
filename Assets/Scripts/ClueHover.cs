using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueHover : MonoBehaviour
{
    [SerializeField]
    private MagnifyingController m_magnifyingGlass;
    private Coroutine m_currentAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter() {
        if (m_currentAnimation != null)
        {
            StopCoroutine(m_currentAnimation);
            m_currentAnimation = null;
        }
        
        m_currentAnimation = StartCoroutine(m_magnifyingGlass.IncreaseSize());
    }

    void OnMouseExit()
    {
        if (m_currentAnimation != null)
        {
            StopCoroutine(m_currentAnimation);
            m_currentAnimation = null;
        }
        
        m_currentAnimation = StartCoroutine(m_magnifyingGlass.ResetSize());
    }
    
}
