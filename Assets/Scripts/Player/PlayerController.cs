using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField]
    private PlayerBody _defaultPlayer;
    [SerializeField]
    private float m_Speed;

    private PlayerBody _currentPlayer;

    private Vector2 m_OldPos;
    private Vector2 m_NewPos;
    private float m_ClickTime;
    private float m_MoveTime => Vector2.Distance(m_NewPos, m_OldPos) / m_Speed;
    private float m_Direction;
    private bool m_DirectionalMovement;
    private bool m_IsMoving;

    private Transform PlayerTransform
    {
        get => _currentPlayer.transform;
    }

    private SpriteRenderer PlayerSprite
    {
        get => _currentPlayer.Sprite;
    }

    public bool IsMoving
    {
        get { return m_IsMoving; }
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_currentPlayer == null)
        {
            _currentPlayer = _defaultPlayer;
        }

        m_IsMoving = false;
        m_DirectionalMovement = false;

        PlayerTransform.position = ScreenManager.Instance.GetClosestFloorLocation(new Ray(PlayerTransform.position, PlayerTransform.forward));
        InputController.Instance.Click += OnClick;
        InputController.Instance.Move += OnMove;

        m_OldPos = PlayerTransform.position;
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
            if (m_DirectionalMovement)
            {
                PlayerTransform.position += ((new Vector3(m_Direction, 0.0f, 0.0f)) * m_Speed * Time.deltaTime);
                PlayerTransform.position = ScreenManager.Instance.GetClosestFloorLocation(PlayerTransform.position);
                m_NewPos = PlayerTransform.position;
            }
            else if (Vector2.Distance(PlayerTransform.position, m_NewPos) >= 0.001f)
            {
                PlayerTransform.position = Vector2.Lerp(m_OldPos, m_NewPos, m_ClickTime / m_MoveTime);
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

    void OnDestroy()
    {
        InputController.Instance.Click -= OnClick;
        InputController.Instance.Move -= OnMove;
    }

    public void OnClick(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerClick?.layer == LayerMask.NameToLayer("UI"))
        {
            return;
        }

        Ray ray = new Ray(eventData.position, CameraController.Instance.CameraTransform.forward);
        Move(ray);
    }

    public void OnMove(float direction)
    {
        Debug.Log($"moving {direction}");
        m_Direction = direction;
        if (direction != 0.0f)
        {
            m_NewPos = PlayerTransform.position;
            m_OldPos = m_NewPos;
            m_DirectionalMovement = true;
            m_IsMoving = true; 
            PlayerSprite.flipX = direction < 0.0f;
            CommandManager.Instance.ClearQueue();
        }
        else
        {
            m_DirectionalMovement = false;
        }
    }

    [YarnCommand("move")]
    public void Move(Vector2 pos) 
    {
        if (DialogueHelper.Instance.InDialogue || ClueBoardManager.Instance.InClueboard)
        {
            return;
        }

        m_OldPos = PlayerTransform.position;
        m_NewPos = pos;
        Debug.Log(m_NewPos);

        if (m_OldPos.x > m_NewPos.x) {
            PlayerSprite.flipX = true;
        } else {
            PlayerSprite.flipX = false;
        }

        m_ClickTime = 0.0f;
        m_IsMoving = true;
    }

    public void Move(Ray ray)
    {
        Move(ScreenManager.Instance.GetClosestFloorLocation(ray));
    }

    public void StopPlayer()
    {
        m_NewPos = PlayerTransform.position;
        m_OldPos = m_NewPos;
    }

    public void SwitchCurrentPlayer(PlayerBody newPlayer, TeleportInfo teleport)
    {
        m_IsMoving = false;
        _currentPlayer = newPlayer;
        Teleport(teleport);
    }

    public void Teleport(TeleportInfo teleport)
    {
        m_IsMoving = false;
        PlayerTransform.position = ScreenManager.Instance.GetClosestFloorLocation(teleport.teleportPos.position);
        PlayerSprite.flipX = teleport.flip != 0;
    }
}
