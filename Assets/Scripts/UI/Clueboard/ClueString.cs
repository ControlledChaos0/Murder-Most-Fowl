using System;
using System.Collections.Generic;
using System.Linq;
using Clues;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UILineRenderer))]
public class ClueString : MonoBehaviour,
    IPointerClickHandler
{
    [SerializeField]
    private RopePointProvider ropePointProvider = new RopePointProvider();

    [SerializeField]
    private float lengthScale;

    [SerializeField] private RectTransform tTransform;
    [SerializeField] private GameObject stringMenu;
    
    private RectTransform[] transforms;
    
    private UILineRenderer _lineRenderer;
    private MeshCollider _meshCollider;
    private Pin[] _pins;

    public Pin[] Pins
    {
        get => _pins;
    }
    
    void Awake()
    {
        _pins = new Pin[2];
        _lineRenderer = GetComponent<UILineRenderer>();
    }

    void Start()
    {
        transforms = new RectTransform[ropePointProvider.linePoints];
        for (int i = 0; i < tTransform.childCount; i++)
        {
            transforms[i] = (RectTransform)tTransform.GetChild(i);
        }
    }

    void OnDestroy()
    {
        if (stringMenu.Equals(ClueBoardManager.Instance.SelectedString.stringMenu))
        {
            OnDeselect(null);
        }
        _pins[0]?.ClueStrings.Remove(this);
        _pins[1]?.ClueStrings.Remove(this);
    }

    private bool _lrNeedSetParent = true;
    private void SetLineRendererParent()
    {
        if (!_lrNeedSetParent) return;
        _lineRenderer.rectTransform.SetParent(ClueBoardManager.Instance.StringRenderers);
        _lrNeedSetParent = false;
    }
    
    private void Update()
    {
        SetLineRendererParent();
        ropePointProvider.Update();

        _lineRenderer.points = ropePointProvider.Points;
        _lineRenderer.SetVerticesDirty();

        if (_pins[1])
        {
            transform.position = _pins[0].transform.position;
            Vector2 endPos = (_pins[1].transform.position - transform.position) / (ClueBoard.Instance.BoardTransform.localScale.x * ClueBoardManager.Instance.transform.localScale.x);
            SetEndPoint(endPos);
        }

        UpdateCollision();
        UpdateStringMenu();

        SetRopeLen();
    }

    void SetRopeLen()
    {
        float endPointDistance = Vector2.Distance(ropePointProvider.Points[0], ropePointProvider.Points.Last());
        
        ropePointProvider.ropeLength = endPointDistance * lengthScale;
    }

    private void FixedUpdate()
    {
        ropePointProvider.FixedUpdate();
    }

    public void SetEndPoint(Vector2 pos)
    {
        ropePointProvider.SetEndPoint(pos);
    }

    private void UpdateCollision()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            RectTransform rectTrans = transforms[i];

            Vector2 point1 = _lineRenderer.points[i];
            Vector2 point2 = _lineRenderer.points[i+1];

            float dist = Vector2.Distance(point1, point2);
            if (dist == 0.0f)
            {
                rectTrans.gameObject.SetActive(false);
                continue;
            }

            rectTrans.gameObject.SetActive(true);

            rectTrans.anchoredPosition = point1;
            rectTrans.sizeDelta = new Vector2(_lineRenderer.thickness, Vector2.Distance(point1, point2));
            rectTrans.localRotation = Quaternion.LookRotation(transform.forward, point2 - point1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public void OnSelect(BaseEventData eventData)
    {
        ClueBoardManager.Instance.SelectedString.stringMenu.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ClueBoardManager.Instance.SelectedString.stringMenu.SetActive(false);
        ClueBoardManager.Instance.SelectedString = null;
    }

    private void UpdateStringMenu()
    {
        stringMenu.transform.localPosition = ropePointProvider.MidPoint;
    }

    public bool CheckIfDuplicate()
    {
        return _pins[0].ClueStrings.Any(x => (x.Pins[0].Equals(_pins[0]) && x.Pins[1].Equals(_pins[1])) || (x.Pins[1].Equals(_pins[0]) && x.Pins[0].Equals(_pins[1])));
    }

    public void Deduce()
    {
        ClueObjectUI clue1 = _pins[0].gameObject.GetComponentInParent<ClueObjectUI>();
        ClueObjectUI clue2 = _pins[1].gameObject.GetComponentInParent<ClueObjectUI>();
        Clue deduction = ClueManager.GetClueFromDeduction(clue1.Clue.ClueID, clue2.Clue.ClueID);

        if (deduction != null)
        {
            ClueBoardManager.Instance.InstantiateClue(deduction.ClueID);
        }

        stringMenu.gameObject.SetActive(false);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}