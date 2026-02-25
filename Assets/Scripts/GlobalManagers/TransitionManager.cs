using System.Collections;
using UnityEngine;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Animator _animator;
    public bool IsTransitioning
    {
        get { return _animator.IsInTransition(0) || _isTransitioning; }
    }

    public bool _isTransitioning = false;

    const string k_HideStateName = "ScreenTransitionHide";
    const string k_ShowStateName = "ScreenTransitionShow";
    const string k_FadeInStateName = "ScreenTransitionFadeIn";
    const string k_FadeOutStateName = "ScreenTransitionFadeOut";

    void Awake()
    {
        InitializeSingleton();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        StartCoroutine(IFadeInTransition());
    }

    public void FadeOut()
    {
        StartCoroutine(IFadeOutTransition());
    }

    private IEnumerator IFadeInTransition()
    {
        yield return ITransition("TransitionFadeIn");
        _isTransitioning = true;
        while (_isTransitioning)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(k_ShowStateName) || stateInfo.IsName(k_FadeInStateName))
            {
                yield return null;
            } else
            {
                _isTransitioning = false;
            }
        }
    }

    private IEnumerator IFadeOutTransition()
    {
        yield return ITransition("TransitionFadeOut");
        _isTransitioning = true;
        while (_isTransitioning)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(k_HideStateName) || stateInfo.IsName(k_FadeOutStateName))
            {
                yield return null;
            }
            else
            {
                _isTransitioning = false;
            }
        }
    }

    private IEnumerator ITransition(string transition)
    {
        while (IsTransitioning)
        {
            yield return null;
        }
        _animator.SetTrigger(transition);
    }
}
