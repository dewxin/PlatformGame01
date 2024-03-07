using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RectCornerEnum
{
    None = 0,
    BottomLeft,
    TopLeft,
    TopRight,
    BottomRight,
    EnumCount,

}
public class RectCorners
{

    public Vector3 Center => (BottomLeft + TopRight) / 2f;
    public Vector3 Size => (TopRight - BottomLeft);

    public Vector3 BottomLeft { get; set; }
    public Vector3 TopLeft { get; set; }
    public Vector3 TopRight { get; set; }
    public Vector3 BottomRight { get; set; }


    public override string ToString()
    {
        return "BottomLeft:" + BottomLeft.ToString()
            + ", TopLeft:" + TopLeft.ToString()
            + ", TopRight:" + TopRight.ToString()
            + ", BottomRight:" + BottomRight.ToString();
    }

    public Vector3 this[RectCornerEnum i]
    {
        get 
        {
            if(i == RectCornerEnum.BottomLeft) return BottomLeft;
            else if(i == RectCornerEnum.TopLeft) return TopLeft;
            else if(i == RectCornerEnum.TopRight) return TopRight;
            else if(i == RectCornerEnum.BottomRight) return BottomRight;

            throw new Exception("Invalid RectCornerEnum");

        }
    }

    public IEnumerator<Vector3> GetEnumerator()
    {
        yield return BottomLeft;
        yield return TopLeft;
        yield return TopRight;
        yield return BottomRight;
    }

    public RectCorners Apply(Func<Vector3, Vector3> func)
    {
        BottomLeft = func(BottomLeft);
        TopLeft = func(TopLeft);
        TopRight = func(TopRight);
        BottomRight = func(BottomRight);

        return this;
    }

    public List<Vector3> GetVertexList()
    {
        var vertexList = new List<Vector3>
        {
            BottomLeft,
            TopLeft,
            TopRight,
            BottomRight
        };
        return vertexList;
    }

    public bool IsRaycastHit(Ray ray,out float distance)
    {
        Plane plane = new Plane(BottomLeft,TopLeft,TopRight);

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            return PointInRect(hitPoint);
        }

        return false;
    }

    private bool PointInRect(Vector3 testPoint)
    {
        Vector3 lastCrossResult = Vector3.zero;

        var mCorners = GetVertexList();

        for (var i = 0; i < mCorners.Count(); i++)
        {
            //If point is in the polygon
            if (mCorners[i] == testPoint)
                return true;

            var corner1 = mCorners[i];
            var i2 = (i + 1) % mCorners.Count();
            var corner2 = mCorners[i2];


            var cross = Vector3.Cross(testPoint-corner1, testPoint -corner2);
            if(cross == Vector3.zero)
                return false;
            if (Vector3.Dot(cross, lastCrossResult) < 0)
            {
                return false;
            }

            lastCrossResult = cross;
        }

        //If no change in direction, then on same side of all segments, and thus inside
        return true;

    }
}
