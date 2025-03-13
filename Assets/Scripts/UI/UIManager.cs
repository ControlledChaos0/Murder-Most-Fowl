using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private List<Menu> m_CurrentMenu;

    void Awake()
    {
        InitializeSingleton();
        m_CurrentMenu = new();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Enter(Menu menu)
    {
        if (m_CurrentMenu.Count != 0)
        {
            m_CurrentMenu[0].ExitTransition();
        }
        m_CurrentMenu.Insert(0, menu);
        m_CurrentMenu[0].EnterTransition();
    }

    public void Exit()
    {
        m_CurrentMenu[0].ExitTransition();
        m_CurrentMenu.RemoveAt(0);
        if (m_CurrentMenu.Count != 0)
        {
            m_CurrentMenu[0].EnterTransition();
        }
    }
}
