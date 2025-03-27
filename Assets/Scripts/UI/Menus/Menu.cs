using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] 
    protected bool m_IsBaseMenu;

    [SerializeField] 
    protected GameObject m_MenuObject;

    public void Start()
    {
        if (m_IsBaseMenu)
        {
            Enter();
        }
    }

    public void Transition(string menu)
    {
        //menu.TransitionTo();
    }

    public void EnterTransition()
    {
        m_MenuObject.SetActive(true);
    }

    public void ExitTransition()
    {
        m_MenuObject.SetActive(false);
    }

    public virtual void Enter()
    {
        UIManager.Instance.Enter(this);
    }

    public virtual void Exit()
    {
        UIManager.Instance.Exit();
    }
}
