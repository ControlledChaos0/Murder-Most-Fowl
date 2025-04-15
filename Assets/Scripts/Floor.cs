using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Floor : MonoBehaviour
{
    [SerializeField]
    private SplineContainer _floorSplineContainer;

    private Spline _floorSpline;
    private Vector2[] _floorPoints;

    // Start is called before the first frame update
    void Awake()
    {
        _floorSpline = _floorSplineContainer.Spline;
    }
    void Start()
    {
        // Basically, GetNearestPoint only works in the Spline's local position
        // If it is in a shifted object, or the local space != world space, then it doesn't line up
        // What we have to do is create a new spline that takes the possible shift into consideration

        Spline _offsetSpline = new Spline(_floorSpline.Knots);
        int i = 0;
        foreach (BezierKnot knot in _floorSpline.Knots)
        {
            BezierKnot tempKnot = knot;
            tempKnot.Position += (float3)_floorSplineContainer.transform.position;
            SetFloorPoints((Vector3)tempKnot.Position);
            _offsetSpline.SetKnot(i, tempKnot);
            i++;
        }

        _floorSpline = _offsetSpline;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetFloorPoints(Vector2 position)
    {
        if (_floorPoints == null)
        {
            _floorPoints = new[] { position, position };
            return;
        }

        if (_floorPoints[0].x > position.x)
        {
            _floorPoints[0] = position;
        } else if (_floorPoints[1].x < position.x)
        {
            _floorPoints[1] = position;
        }
    }

    public Vector2 GetClosestFloorLocation(Ray clickRay) {
        Vector2 closestPoint = Vector2.positiveInfinity;

        SplineUtility.GetNearestPoint(_floorSpline, clickRay, out float3 nearestPoint, out float t);
        Vector3 worldPoint = nearestPoint;
        closestPoint = worldPoint;

        return closestPoint;
    }

    public Vector2 GetClosestFloorLocation(Vector3 point)
    {
        Vector2 closestPoint = Vector2.positiveInfinity;

        SplineUtility.GetNearestPoint(_floorSpline, point, out float3 nearestPoint, out float t);
        Vector3 worldPoint = nearestPoint;
        closestPoint = worldPoint;

        return closestPoint;
    }
}
