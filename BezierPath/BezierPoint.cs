using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BezierPoint
{
    // TODO: Make handles act according to current ArmMode in the 'set' accessors
    public Vector3 handleBack { get{ return _handleBack; } set { _handleBack = value; } }
    public Vector3 handleFront { get { return _handleFront; } set { _handleFront = value; } }

    public Vector3 position;                                                    // BezierPoint position
    public List<Vector3> points = new List<Vector3>();                          // Calculated points
    public List<float> lengths = new List<float>();                             // Lengths between respective calculated points
    public float length = 0;                                                    // Sum of lengths
    public int iterations;                                                      // Amount of points between endpoints
    public ArmMode armMode;                                                     // How handles act (Mirror, MirrorAngle, Induvidual)

    [SerializeField]
    private Vector3 _handleBack, _handleFront;                                  // Handle positions, origin at BezierPoint position

    public BezierPoint(Vector3 pos, Vector3 _handleB, Vector3 _handleF, int iter, ArmMode armM)
    {
        position = pos;
        _handleBack = _handleB;
        _handleFront = _handleF;
        iterations = iter;
        armMode = armM;
    }

    /// <summary>
    /// Calculates all points between this BezierPoint and another. Calculated points
    /// are saved to 'points'. Also stores the lengths between these and the length-sum.
    /// Endpoints are always stored (Even if 'iterations' == 0).
    /// </summary>
    public void IterateBezierPoints(BezierPoint bezierPoint)
    {
        points.Clear();
        lengths.Clear();
        length = 0;

        Vector3 point_0 = position;                                             // Sets the req. points for the Cubic Bezier Curve formula; CalculateBezier()
        Vector3 point_1 = point_0 + handleFront;
        Vector3 point_3 = bezierPoint.position;
        Vector3 point_2 = point_3 + bezierPoint.handleBack;

        for (int iteration = 0; iteration <= iterations + 1; iteration++)       // Goes through every iteration, and the endpoints(t==0 and t==1)
        {
            float t = iteration / (float)(iterations + 1);                      // Current value t; 0 <= t >= 1
            points.Add(CalculateBezier(point_0, point_1, point_2, point_3, t)); // Calculates the current point and adds it to the list
            if (iteration != 0)                                                 // Adds the distance from last point to the current
            { lengths.Add(Vector3.Distance(points[iteration - 1], points[iteration])); length += lengths[lengths.Count - 1]; }
        }
    }

    /// <summary>
    /// Calculates a single point on the Cubic Bezier Curve with the value t,
    /// t between 0 and 1 (including both).
    /// </summary>
    public static Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (Mathf.Pow(1 - t, 3) * p0) + (3 * Mathf.Pow(1 - t, 2) * t * p1) + (3 * (1 - t) * t * t * p2) + (t * t * t * p3);
    }
}

public enum ArmMode
{
    Mirror,                                                                     // Perfectly mirrors the handles
    MirrorAngle,                                                                // Perfectly mirrors the angle, induvidual distance
    Induvidual                                                                  // Handles act induvidually
}
