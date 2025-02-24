using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class RopePointProvider
{
    public event Action OnPointsChanged;

    [Header("Rope Transforms")] [Tooltip("The rope will start at this point")] [SerializeField]
    private Vector3 startPoint;

    public Vector3 StartPoint => startPoint;

    [Tooltip("This will move at the center hanging from the rope, like a necklace, for example")] [SerializeField]
    private Vector3 midPoint;

    public Vector3 MidPoint => midPoint;

    [Tooltip("The rope will end at this point")] [SerializeField]
    private Vector3 endPoint;

    public Vector3 EndPoint => endPoint;

    [Header("Rope Settings")]
    [Tooltip(
        "How many points should the rope have, 2 would be a triangle with straight lines, 100 would be a very flexible rope with many parts")]
    [Range(2, 100)]
    public int linePoints = 10;

    [Tooltip(
        "Value highly dependent on use case, a metal cable would have high stiffness, a rubber rope would have a low one")]
    public float stiffness = 350f;

    [Tooltip("0 is no damping, 50 is a lot")]
    public float damping = 15f;

    [Tooltip(
        "How long is the rope, it will hang more or less from starting point to end point depending on this value")]
    public float ropeLength = 15;
    
    [Header("Rational Bezier Weight Control")]
    [Tooltip("Adjust the middle control point weight for the Rational Bezier curve")]
    [Range(1, 15)]
    public float midPointWeight = 1f;

    private const float
        StartPointWeight =
            1f; //these need to stay at 1, could be removed but makes calling the rational bezier function easier to read and understand

    private const float EndPointWeight = 1f;

    [Header("Midpoint Position")]
    [Tooltip("Position of the midpoint along the line between start and end points")]
    [Range(0.25f, 0.75f)]
    public float midPointPosition = 0.5f;

    private Vector3 currentValue;
    private Vector3 currentVelocity;
    private Vector3 targetValue;
    public Vector3 otherPhysicsFactors { get; set; }
    private const float valueThreshold = 0.01f;
    private const float velocityThreshold = 0.01f;

    private bool isFirstFrame = true;

    private Vector3 prevStartPointPosition;
    private Vector3 prevEndPointPosition;
    private float prevMidPointPosition;
    private float prevMidPointWeight;

    private float prevLineQuality;
    private float prevstiffness;
    private float prevDampness;
    private float prevRopeLength;

    public Vector2[] Points { get; private set; }  = new Vector2[0];

    public void Start()
    {
        if (AreEndPointsValid())
        {
            currentValue = GetMidPoint();
            targetValue = currentValue;
            currentVelocity = Vector3.zero;
            SetSplinePoint(); // Ensure initial spline point is set correctly
        }
    }
    
    public void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (AreEndPointsValid())
            {
                RecalculateRope();
                SimulatePhysics();
            }
            else
            {
                Points = new Vector2[0];
            }
        }
    }

    public void Update()
    {
        if (AreEndPointsValid())
        {
            SetSplinePoint();

            if (!Application.isPlaying && (IsPointsMoved() || IsRopeSettingsChanged()))
            {
                SimulatePhysics();
                NotifyPointsChanged();
            }

            prevStartPointPosition = startPoint;
            prevEndPointPosition = endPoint;
            prevMidPointPosition = midPointPosition;
            prevMidPointWeight = midPointWeight;

            prevLineQuality = linePoints;
            prevstiffness = stiffness;
            prevDampness = damping;
            prevRopeLength = ropeLength;
        }
    }

    private bool AreEndPointsValid()
    {
        return startPoint != null && endPoint != null;
    }

    private void SetSplinePoint()
    {
        if (Points.Length != linePoints + 1)
        {
            Points = new Vector2[linePoints + 1];
        }

        Vector3 mid = GetMidPoint();
        targetValue = mid;
        mid = currentValue;

        if (midPoint != null)
        {
            midPoint = GetRationalBezierPoint(startPoint, mid, endPoint, midPointPosition,
                StartPointWeight, midPointWeight, EndPointWeight);
        }

        for (int i = 0; i < linePoints; i++)
        {
            Vector3 p = GetRationalBezierPoint(startPoint, mid, endPoint, i / (float)linePoints,
                StartPointWeight, midPointWeight, EndPointWeight);
            Points[i] = p;
        }

        Points[linePoints] = endPoint;
    }

    private float CalculateYFactorAdjustment(float weight)
    {
        //float k = 0.360f; //after testing this seemed to be a good value for most cases, more accurate k is available.
        float k = Mathf.Lerp(0.493f, 0.323f,
            Mathf.InverseLerp(1, 15,
                weight)); //K calculation that is more accurate, interpolates between precalculated values.
        float w = 1f + k * Mathf.Log(weight);
        return w;
    }

    private Vector3 GetMidPoint()
    {
        Vector3 startPointPosition = startPoint;
        Vector3 endPointPosition = endPoint;
        Vector3 midpos = Vector3.Lerp(startPointPosition, endPointPosition, midPointPosition);
        float yFactor = (ropeLength - Mathf.Min(Vector3.Distance(startPointPosition, endPointPosition), ropeLength)) /
                        CalculateYFactorAdjustment(midPointWeight);
        midpos.y -= yFactor;
        return midpos;
    }

    private Vector3 GetRationalBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t, float w0, float w1, float w2)
    {
        //scale each point by its weight (can probably remove w0 and w2 if the midpoint is the only adjustable weight)
        Vector3 wp0 = w0 * p0;
        Vector3 wp1 = w1 * p1;
        Vector3 wp2 = w2 * p2;

        //calculate the denominator of the rational Bézier curve
        float denominator = w0 * Mathf.Pow(1 - t, 2) + 2 * w1 * (1 - t) * t + w2 * Mathf.Pow(t, 2);
        //calculate the numerator and devide by the demoninator to get the point on the curve
        Vector3 point = (wp0 * Mathf.Pow(1 - t, 2) + wp1 * 2 * (1 - t) * t + wp2 * Mathf.Pow(t, 2)) / denominator;

        return point;
    }

    public Vector3 GetPointAt(float t)
    {
        if (!AreEndPointsValid())
        {
            Debug.LogError("StartPoint or EndPoint is not assigned.");
            return Vector3.zero;
        }

        return GetRationalBezierPoint(startPoint, currentValue, endPoint, t, StartPointWeight,
            midPointWeight, EndPointWeight);
    }

    public void FixedUpdate()
    {
        if (AreEndPointsValid())
        {
            if (!isFirstFrame)
            {
                SimulatePhysics();
            }

            isFirstFrame = false;
        }
    }

    private void SimulatePhysics()
    {
        float dampingFactor = Mathf.Max(0, 1 - damping * Time.fixedDeltaTime);
        Vector3 acceleration = (targetValue - currentValue) * stiffness * Time.fixedDeltaTime;
        currentVelocity = currentVelocity * dampingFactor + acceleration + otherPhysicsFactors;
        currentValue += currentVelocity * Time.fixedDeltaTime;

        if (Vector3.Distance(currentValue, targetValue) < valueThreshold &&
            currentVelocity.magnitude < velocityThreshold)
        {
            currentValue = targetValue;
            currentVelocity = Vector3.zero;
        }
    }

    /*private void OnDrawGizmos()
    {
        if (!AreEndPointsValid())
            return;

        Vector3 midPos = GetMidPoint();
        // Uncomment if you need to visualize midpoint
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(midPos, 0.2f);
    }*/

    // New API methods for setting start and end points
    // with instantAssign parameter to recalculate the rope immediately, without 
    // animating the rope to the new position.
    // When newStartPoint or newEndPoint is null, the rope will be recalculated immediately

    public void SetStartPoint(Vector3 newStartPoint, bool instantAssign = false)
    {
        startPoint = newStartPoint;
        prevStartPointPosition = startPoint == null ? Vector3.zero : startPoint;

        if (instantAssign || newStartPoint == null)
        {
            RecalculateRope();
        }

        NotifyPointsChanged();
    }

    public void SetMidPoint(Vector3 newMidPoint, bool instantAssign = false)
    {
        midPoint = newMidPoint;
        prevMidPointPosition = midPoint == null ? 0.5f : midPointPosition;

        if (instantAssign || newMidPoint == null)
        {
            RecalculateRope();
        }

        NotifyPointsChanged();
    }

    public void SetEndPoint(Vector3 newEndPoint, bool instantAssign = false)
    {
        endPoint = newEndPoint;
        prevEndPointPosition = endPoint == null ? Vector3.zero : endPoint;

        if (instantAssign || newEndPoint == null)
        {
            RecalculateRope();
        }

        NotifyPointsChanged();
    }

    public void RecalculateRope()
    {
        if (!AreEndPointsValid())
        {
            Points = new Vector2[0];
            return;
        }

        currentValue = GetMidPoint();
        targetValue = currentValue;
        currentVelocity = Vector3.zero;
        SetSplinePoint();
    }

    private void NotifyPointsChanged()
    {
        OnPointsChanged?.Invoke();
    }

    private bool IsPointsMoved()
    {
        var startPointMoved = startPoint != prevStartPointPosition;
        var endPointMoved = endPoint != prevEndPointPosition;
        return startPointMoved || endPointMoved;
    }

    private bool IsRopeSettingsChanged()
    {
        var lineQualityChanged = !Mathf.Approximately(linePoints, prevLineQuality);
        var stiffnessChanged = !Mathf.Approximately(stiffness, prevstiffness);
        var dampnessChanged = !Mathf.Approximately(damping, prevDampness);
        var ropeLengthChanged = !Mathf.Approximately(ropeLength, prevRopeLength);
        var midPointPositionChanged = !Mathf.Approximately(midPointPosition, prevMidPointPosition);
        var midPointWeightChanged = !Mathf.Approximately(midPointWeight, prevMidPointWeight);

        return lineQualityChanged
               || stiffnessChanged
               || dampnessChanged
               || ropeLengthChanged
               || midPointPositionChanged
               || midPointWeightChanged;
    }
}