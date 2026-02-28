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
    private Animator _currentAnimator;
    private PlayerInteractable m_Interactable;

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

    public Vector3 PlayerPos
    {
        get => PlayerTransform.position;
    }

    public PlayerInteractable Interactable
    {
        get => m_Interactable;
        set => m_Interactable = value;
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

        _currentAnimator = _currentPlayer.GetComponentInChildren<Animator>();

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
            _currentAnimator.SetBool("moving", false);
            m_IsMoving = false;
            return;
        }

        if (m_IsMoving)
        {
            _currentAnimator.SetBool("moving", true);
            if ((Interactable != null && Interactable.WithinRange(PlayerTransform.position)))
            {
                m_Interactable = null;
                m_IsMoving = false;
            }
            else if (m_DirectionalMovement)
            {
                PlayerTransform.position += (m_Speed * Time.deltaTime * (new Vector3(m_Direction, 0.0f, 0.0f)));
                PlayerTransform.position = ScreenManager.Instance.GetClosestFloorLocation(PlayerTransform.position);
                m_OldPos = m_NewPos;
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
        } else
        {
            _currentAnimator.SetBool("moving", false);
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
        if (eventData == null)
        {
            return;
        }

        GameObject pointClick = eventData.pointerClick;
        int uiLayer = LayerMask.NameToLayer("UI");
        if ((pointClick != null ? pointClick.layer : -1) == uiLayer)
        {
            return;
        }

        CommandManager.Instance.ClearQueue();
        if (pointClick == null)
        {
            m_Interactable = null;
        } 
        else
        {
            m_Interactable = pointClick.GetComponentInParent<PlayerInteractable>();
        }
        Vector3 pos = m_Interactable != null ? m_Interactable.transform.position : eventData.position;
        Ray ray = new Ray(pos, CameraController.Instance.CameraTransform.forward) ;
        MoveCommand moveCommand = new MoveCommand(ScreenManager.Instance.GetClosestFloorLocation(ray));
        CommandManager.Instance.Queue(moveCommand);
     }

    public void OnMove(float direction)
    {
        if (DialogueHelper.Instance.InDialogue || ClueBoardManager.Instance.InClueboard)
        {
            _currentAnimator.SetBool("moving", false);
            m_IsMoving = false;
            return;
        }

        Debug.Log($"moving {direction}");
        m_Direction = direction;
        if (direction != 0.0f)
        {
            m_NewPos = PlayerTransform.position;
            m_OldPos = m_NewPos;
            m_DirectionalMovement = true;
            m_IsMoving = true; 
            PlayerSprite.flipX = direction < 0.0f;
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
            _currentAnimator.SetBool("moving", false);
            m_IsMoving = false;
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

    public void StopPlayer()
    {
        m_NewPos = PlayerTransform.position;
        m_OldPos = m_NewPos;
    }

    public void SwitchCurrentPlayer(PlayerBody newPlayer, TeleportInfo teleport)
    {
        m_IsMoving = false;
        _currentPlayer = newPlayer;
        _currentAnimator = _currentPlayer.GetComponentInChildren<Animator>();
        Teleport(teleport);
    }

    public void Teleport(TeleportInfo teleport)
    {
        m_IsMoving = false;
        PlayerTransform.position = ScreenManager.Instance.GetClosestFloorLocation(teleport.teleportPos.position);
        PlayerSprite.flipX = teleport.flip != 0;
    }
}
