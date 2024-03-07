using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBoundry : Boundry
{

    [SerializeField]
    private float width = 2f;

    void Start()
    {
    }


    public (Vector3,Vector3) GetBoundry()
    {

        var leftPoint = transform.position;
        leftPoint.x -= width / 2;

        var rightPoint = transform.position;
        rightPoint.x += width / 2;
        return (leftPoint, rightPoint);
    }

    private void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;

        var boundry = GetBoundry();
        DrawTopDownLine(boundry.Item1);
        DrawTopDownLine(boundry.Item2);

        Gizmos.DrawLine(boundry.Item1, boundry.Item2);


    }

    private void DrawTopDownLine(Vector3 point)
    {
        var top = new Vector3(point.x, point.y + 1f, point.z);
        var down = new Vector3(point.x, point.y - 1f, point.z);
        Gizmos.DrawLine(top, down);
    }


}
