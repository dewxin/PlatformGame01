using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class VerticalPlatformPoint : MonoBehaviour
{
    public Transform horizontalPlatformStart;
    public Transform horizontalPlatformEnd;

    public void OnEnable()
    {
    }


    private void Update()
    {
        CorrectPosition();
    }

    private void CorrectPosition()
    {
        if (horizontalPlatformStart == null)
            return;
        if (horizontalPlatformEnd == null)
            return;
        var newPos = NearestPointOnLine(this.transform.position);

        var vec = newPos - transform.position;
        if (vec.magnitude > 0.001f)
            transform.position = newPos;
    }

    public Vector3 NearestPointOnLine(Vector3 pnt)
    {
        var v = pnt - horizontalPlatformStart.position;
        var lineDir = (horizontalPlatformEnd.position - horizontalPlatformStart.position).normalized;
        var d = Vector3.Dot(v, lineDir);
        return horizontalPlatformStart.position + lineDir * d;
    }

}
