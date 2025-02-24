using System;
using System.Linq;
using UnityEngine; 

[RequireComponent(typeof(UILineRenderer))]
public class ClueString : MonoBehaviour
{
    [SerializeField]
    private RopePointProvider ropePointProvider = new RopePointProvider();

    [SerializeField]
    private float lengthScale;
    
    private UILineRenderer _lineRenderer;
    
    void Awake()
    {
        _lineRenderer = GetComponent<UILineRenderer>();
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
}