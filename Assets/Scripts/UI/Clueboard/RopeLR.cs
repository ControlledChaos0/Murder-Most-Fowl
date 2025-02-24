using System;
using System.Linq;
using UnityEngine;

public class RopeLR : MonoBehaviour
{
    public RopePointProvider RopeProvider = new RopePointProvider();
    
    private LineRenderer lineRenderer;

    private void Start()
    {
        RopeProvider.Start();
        InitializeLineRenderer();
    }

    private void Update()
    {
        RopeProvider.Update();
        lineRenderer.SetPositions(RopeProvider.Points.Select(v => (Vector3) v).ToArray());
    }

    private void FixedUpdate()
    {
        RopeProvider.FixedUpdate();
    }
    
    private void InitializeLineRenderer()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }
}