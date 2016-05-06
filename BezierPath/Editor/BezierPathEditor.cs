using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierPath))]                                              // Editor used when a GameObject with a BezierPath Component is selected
public class BezierPathEditor : Editor
{
    private BezierPath bezierPath;                                              // Stores current BezierPath

    void OnSceneGUI()
    {
        bezierPath = (BezierPath)target;                                        // Gets currently selected BezierPath

        // Removed later, don't want to regenerate full path every update 
        for (int i = 0; i < bezierPath.bezierPoints.Count; i++)                 // Generates full path
            bezierPath.Generate(i);

        if (bezierPath.bezierPoints.Count > 1)                                  // Draws the path if there are two or more BezierPoints
            DrawBezierPath();
    }

    /// <summary>
    /// Draws the path, respecting the 'closed' variable.
    /// </summary>
    private void DrawBezierPath()
    {
                                                                                // Loops through all BezierPoints (without last point if path not closed)
        for (int bezierPointNumber = 0; bezierPointNumber < bezierPath.bezierPoints.Count - (bezierPath.closed ? 0 : 1); bezierPointNumber++)
        {
                                                                                // Loops through calculated points(except last) in current BezierPoint and draws a line between current and next calculated point
            for (int iteration = 0; iteration < bezierPath.bezierPoints[bezierPointNumber].points.Count - 1; iteration++)
                Handles.DrawLine(bezierPath.transform.TransformPoint(bezierPath.bezierPoints[bezierPointNumber].points[iteration]),
                    bezierPath.transform.TransformPoint(bezierPath.bezierPoints[bezierPointNumber].points[iteration + 1]));
        }
    }
}
