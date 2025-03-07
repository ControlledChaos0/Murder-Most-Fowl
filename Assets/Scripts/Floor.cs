using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Floor : MonoBehaviour
{
    [SerializeField]
    private SplineContainer _floorSplineContainer;

    private Spline _floorSpline;

    // Start is called before the first frame update
    void Awake()
    {
        _floorSpline = _floorSplineContainer.Spline;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Kinda ugly fix
    // Basically, GetNearestPoint only works in the Spline's local position
    // If it is in a shifted object, or the local space != world space, then it doesn't line up
    // What we have to do is create a new spline that takes the possible shift into consideration
    //
    // Not the most efficient but it's literally 2 points getting cycled through every time movement happens which is a 1 off
    // so not actually that bad all things considered
    public Vector2 GetClosestFloorLocation(Ray clickRay) {
        Vector2 closestPoint = Vector2.positiveInfinity;
        Spline _offsetSpline = new Spline(_floorSpline.Knots);

        int i = 0;
        foreach (BezierKnot knot in _floorSpline.Knots)
        {
            BezierKnot tempKnot = knot;
            tempKnot.Position += (float3)_floorSplineContainer.transform.position;
            _offsetSpline.SetKnot(i, tempKnot);
            i++;
        }

        SplineUtility.GetNearestPoint(_offsetSpline, clickRay, out float3 nearestPoint, out float t);
        Vector3 worldPoint = nearestPoint;
        closestPoint = worldPoint;

        return closestPoint;
    }
}
