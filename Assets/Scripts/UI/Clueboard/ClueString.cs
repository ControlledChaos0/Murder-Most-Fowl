using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UILineRenderer))]
public class ClueString : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private RopePointProvider ropePointProvider = new RopePointProvider();

    [SerializeField]
    private float lengthScale;

    [SerializeField] private RectTransform[] transforms;
    
    private UILineRenderer _lineRenderer;
    private MeshCollider _meshCollider;
    
    void Awake()
    {
        _lineRenderer = GetComponent<UILineRenderer>();
    }

    void Start()
    {
        transforms = new RectTransform[ropePointProvider.linePoints];
        for (int i = 0; i < transform.childCount; i++)
        {
            transforms[i] = (RectTransform)transform.GetChild(i);
        }
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

        UpdateCollision();

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enterrrrrr");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("God fucking dammit");
    }

    public void SetRaycastTarget(bool set)
    {
        //_lineRenderer.raycastTarget = set;
    }
}