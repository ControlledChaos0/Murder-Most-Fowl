using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _characterSprite;
    [SerializeField]
    private float m_Speed;

    private Vector2 m_OldPos;
    private Vector2 m_NewPos;
    private float m_ClickTime;
    private float m_MoveTime => Vector2.Distance(m_NewPos, m_OldPos) / m_Speed;
    private bool m_IsMoving;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = ScreenManager.Instance.GetClosestFloorLocation(new Ray(transform.position, transform.forward));
        InputController.Instance.Click += OnClick;

        m_OldPos = transform.position;
        m_NewPos = m_OldPos;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (m_IsMoving)
        {
            if (Vector2.Distance(transform.position, m_NewPos) >= 0.001f)
            {
                transform.position = Vector2.Lerp(m_OldPos, m_NewPos, m_ClickTime / m_MoveTime);
                m_ClickTime += Time.deltaTime;
            }
            else
            {
                m_IsMoving = false;
            }
        }

        Debug.DrawLine(new Vector2(m_NewPos.x - .05f, m_NewPos.y - .05f), new Vector2(m_NewPos.x + .05f, m_NewPos.y + .05f));
        Debug.DrawLine(new Vector2(m_NewPos.x + .05f, m_NewPos.y - .05f), new Vector2(m_NewPos.x - .05f, m_NewPos.y + .05f));
    }

    public void OnClick(PointerEventData eventData)
    {
        Move(new Ray(eventData.position, CameraController.Instance.CameraTransform.forward));
    }

    [YarnCommand("move")]
    public void Move(Ray ray) 
    {
        if (DialogueHelper.Instance.InDialogue || ClueBoardManager.Instance.InClueboard)
        {
            return;
        }

        m_OldPos = transform.position;
        m_NewPos = ScreenManager.Instance.GetClosestFloorLocation(ray);
        Debug.Log(m_NewPos);

        if (m_OldPos.x > m_NewPos.x) {
            _characterSprite.flipX = true;
        } else {
            _characterSprite.flipX = false;
        }

        m_ClickTime = 0.0f;
        m_IsMoving = true;
    }

    public bool getIsMoving()
    {
        return m_IsMoving;
    }
}
