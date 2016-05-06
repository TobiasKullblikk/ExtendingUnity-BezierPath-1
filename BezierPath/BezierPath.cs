using UnityEngine;
using System.Collections.Generic;

public class BezierPath : MonoBehaviour
{
    // TODO: Add length get property, respecting closed paths

    public List<BezierPoint> bezierPoints = new List<BezierPoint>();
    public bool closed = false;
    public bool hideTransform = false, freeHandle = true, dialogOpen = true;
    public Color color = Color.black, colorSelected = Color.red;
    public float sizePosition = 0.25f, sizeHandle = 0.15f;

    private float _length = 0;

    /// <summary>
    /// Generates the curve for the BezierPoint with current index and the one
    /// below. Will generate as if the path was closed (Does not consider the 'closed' variable).
    /// </summary>
    public void Generate(int index)
    {
        if (bezierPoints.Count > 0)                                             // Only if there are any BezierPoints in the path (Wraps around to itself if only 1 BezierPoint)
        {
                                                                                // Mod method used to wrap around, if index goes below 0 or above count
            bezierPoints[index].IterateBezierPoints(bezierPoints[Mod(index + 1, bezierPoints.Count)]);
            bezierPoints[Mod(index - 1, bezierPoints.Count)].IterateBezierPoints(bezierPoints[index]);
        }

        _length = 0;                                                            
        for (int i = 0; i < bezierPoints.Count - 1; i++)                        // Recalculates the full curve length
            _length += bezierPoints[i].length;                                  // Does not add the length of the closing-part, it will be added with 'length'-property
    }

    /// <summary>
    /// Mathematical Modulo (int). Works differently than (%)-operator on negative values.
    /// </summary>
    public static int Mod(int a, int b)
    {
        return a - b * (int)Mathf.Floor(a / (float)b);
    }
}
