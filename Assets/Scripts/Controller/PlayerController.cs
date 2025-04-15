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
    private float m_Direction;
    private bool m_DirectionalMovement;
    private bool m_IsMoving;

    // Start is called before the first frame update
    void Start()
    {
        m_IsMoving = false;
        m_DirectionalMovement = false;

        transform.position = ScreenManager.Instance.GetClosestFloorLocation(new Ray(transform.position, transform.forward));
        InputController.Instance.Click += OnClick;
        InputController.Instance.Move += OnMove;

        m_OldPos = transform.position;
        m_NewPos = m_OldPos;
    }

    // Update is called once per frame

    void Update()
    {
        if (DialogueHelper.Instance.InDialogue || ClueBoardManager.Instance.InClueboard)
        {
            return;
        }

        if (m_IsMoving)
        {
            if (Vector2.Distance(transform.position, m_NewPos) >= 0.001f)
            {
                transform.position = Vector2.Lerp(m_OldPos, m_NewPos, m_ClickTime / m_MoveTime);
                m_ClickTime += Time.deltaTime;
            }
            else if (m_DirectionalMovement)
            {
                transform.position += ((new Vector3(m_Direction,0.0f,0.0f)) * m_Speed * Time.deltaTime);
                transform.position = ScreenManager.Instance.GetClosestFloorLocation(transform.position);
                m_NewPos = transform.position;
            }
            else
            {
                m_IsMoving = false;
            }
        }

        Debug.DrawLine(new Vector2(m_NewPos.x - .05f, m_NewPos.y - .05f), new Vector2(m_NewPos.x + .05f, m_NewPos.y + .05f));
        Debug.DrawLine(new Vector2(m_NewPos.x + .05f, m_NewPos.y - .05f), new Vector2(m_NewPos.x - .05f, m_NewPos.y + .05f));
    }

    void OnDestroy()
    {
        InputController.Instance.Click -= OnClick;
        InputController.Instance.Move -= OnMove;
    }

    public void OnClick(PointerEventData eventData)
    {
        Move(new Ray(eventData.position, CameraController.Instance.CameraTransform.forward));
    }

    public void OnMove(float direction)
    {
        Debug.Log($"moving {direction}");
        m_Direction = direction;
        if (direction != 0.0f)
        {
            m_NewPos = transform.position;
            m_OldPos = m_NewPos;
            m_DirectionalMovement = true;
            m_IsMoving = true;
        }
        else
        {
            m_DirectionalMovement = false;
        }
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
