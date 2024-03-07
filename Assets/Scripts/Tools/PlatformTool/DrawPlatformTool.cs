using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[ExecuteInEditMode]
public class DrawPlatformTool : MonoBehaviour
{
    private enum State
    {
        None,
        DrawingHorizontal,
        DrawingVertical,
    }
    const string HorizontalPlatformStr = "HorizontalPlatform";
    const string VerticalPlatformStr = "VerticalPlatform";

    public bool drawPlatform;
    public Color lineColor = Color.white;

    private System.Collections.Generic.List<Vector3> tmpHorizontalPlatformPointList= new System.Collections.Generic.List<Vector3>();
    private State state;
    private Vector3 tmpVerticalStartPoint;

#if UNITY_EDITOR
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        TryGenerateNewPoint();
    }

    private void TryGenerateNewPoint()
    {
        if (!drawPlatform)
        {
            TryGeneratePlatform(tmpHorizontalPlatformPointList, HorizontalPlatformStr);
            state = State.None;
            return;
        }

        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        if (ray.direction != Vector3.forward)
        {
            Debug.LogError("Need operate in 2d mode");
            return;
        }

        Selection.activeGameObject = this.gameObject;
        if(Tools.current != Tool.Custom)
            Tools.current = Tool.Custom;


        //左键点击
        if (Event.current.type == EventType.MouseDown&& Event.current.button == 0)
        {
            state = State.DrawingHorizontal;
            AddTmpHorizontalPoint(ray);
        }


        //右键点击
        if(Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            if(state == State.DrawingHorizontal) {
                TryGeneratePlatform(tmpHorizontalPlatformPointList, HorizontalPlatformStr);
                state = State.None;
            }
            else if (state == State.None)
            {
                tmpVerticalStartPoint = ray.origin;
                tmpVerticalStartPoint.z = 0;
                state = State.DrawingVertical;
            }
            else if(state == State.DrawingVertical)
            {
                if(TryGenerateVerticalPlatform(ray))
                {
                    state = State.None;
                }
            }
        }

    }


    private void AddTmpHorizontalPoint(Ray ray)
    {
        Vector3 point = ray.origin;
        point.z = 0;
        tmpHorizontalPlatformPointList.Add(point);
    }

    private void TryGenerateVerticalPlatform(System.Collections.Generic.List<VerticalPlatformPointInfo> pointList)
    {
        if (pointList.Count == 0 || pointList.Count == 1)
            return;

        UnityEditorInternal.InternalEditorUtility.AddTag(VerticalPlatformStr);

        var lineGO = new GameObject();
        lineGO.tag = VerticalPlatformStr;
        lineGO.name = VerticalPlatformStr + transform.childCount;
        lineGO.transform.SetParent(transform);

        foreach (var point in pointList)
        {
            var pointGO = new GameObject();
            pointGO.name = "waypoint" + lineGO.transform.childCount;
            pointGO.transform.position = point.Position;
            pointGO.transform.SetParent(lineGO.transform);

            var verticalPoint = pointGO.AddComponent<VerticalPlatformPoint>();
            verticalPoint.horizontalPlatformStart = point.HorizontalPlatformStart;
            verticalPoint.horizontalPlatformEnd = point.HorizontalPlatformEnd;
        }


        pointList.Clear();
    }

    private void TryGeneratePlatform(System.Collections.Generic.List<Vector3> pointList, string tag)
    {
        if (pointList.Count == 0 || pointList.Count == 1)
        {
            pointList.Clear();
            return;
        }

        UnityEditorInternal.InternalEditorUtility.AddTag(tag);

        var lineGO = new GameObject();
        lineGO.tag = tag;
        lineGO.name = tag + transform.childCount;
        lineGO.transform.SetParent(transform);

        foreach (var point in pointList)
        {
            var pointGO = new GameObject();
            pointGO.name = "waypoint" + lineGO.transform.childCount;
            pointGO.transform.position = point;
            pointGO.transform.SetParent(lineGO.transform);
        }


        pointList.Clear();
    }

    private bool TryGenerateVerticalPlatform(Ray mouseRay)
    {
        var endPoint = mouseRay.origin;
        endPoint.z = 0;

        if(GetVertialPlatformPoints(tmpVerticalStartPoint, endPoint, out var line))
        {
            TryGenerateVerticalPlatform(line);
            Debug.Log("generate vertical platform");
            return true;
        }

        Debug.Log("cannot generate platform");

        return false;
    }

    private bool GetVertialPlatformPoints(Vector3 start, Vector3 end, out System.Collections.Generic.List<VerticalPlatformPointInfo> resultList)
    {

        resultList = default;
        var platformList = GetHorizontalPlatformList();

        var intersectPointList= new System.Collections.Generic.List<VerticalPlatformPointInfo>();

        foreach(var platform in platformList)
        {
            foreach(var lineSegment in platform)
            {
                if(TryGetIntersectPoint(start, end, 
                    lineSegment.Item1.position, lineSegment.Item2.position, out var point))
                {
                    Debug.Log($"add point interset {point}");
                    var pointInfo = new VerticalPlatformPointInfo();
                    pointInfo.Position = point;
                    pointInfo.HorizontalPlatformStart = lineSegment.Item1;
                    pointInfo.HorizontalPlatformEnd = lineSegment.Item2;
                    intersectPointList.Add(pointInfo);
                }
            }
        }

        if (intersectPointList.Count < 2)
            return false;

        intersectPointList.Sort(
            (point1, point2) =>
            {
                var mag = ((point1.Position - start).magnitude - (point2.Position - start).magnitude);
                return (int)(mag * 100000);
            });


        resultList = new System.Collections.Generic.List<VerticalPlatformPointInfo>() { intersectPointList[0], intersectPointList[1] };
        return true;
    }

    private System.Collections.Generic.List<System.Collections.Generic.List<(Transform, Transform)>> GetHorizontalPlatformList()
    {
        var platformList = new System.Collections.Generic.List<System.Collections.Generic.List<(Transform, Transform)>>();
        foreach (Transform platform in transform)
        {
            if(platform.tag != HorizontalPlatformStr) continue;

            var pointList = new System.Collections.Generic.List<Transform>();
            foreach(Transform point in platform)
                pointList.Add(point);


            var lineList = new System.Collections.Generic.List<(Transform, Transform)>();
            for(int i =1;i <pointList.Count; ++i)
                lineList.Add((pointList[i - 1], pointList[i]));

            platformList.Add(lineList);
        }

        return platformList;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLineStrip(tmpHorizontalPlatformPointList.ToArray(),false);

        DrawChildrenLine();
        DrawHorizontalCutLine();
        DrawVerticalCutLine();
    }

    private void DrawHorizontalCutLine()
    {
        if (state != State.DrawingHorizontal)
            return;

        Gizmos.color = Color.red;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        var point = ray.origin;
        point.z = 0;
        Gizmos.DrawLine(tmpHorizontalPlatformPointList.Last(), point);
    }

    private void DrawVerticalCutLine()
    {
        if (state!=State.DrawingVertical)
            return;
        Gizmos.color = Color.red;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        var point = ray.origin;
        point.z = 0;
        Gizmos.DrawLine(tmpVerticalStartPoint, point);
    }

    private void DrawChildrenLine()
    {
        Gizmos.color = lineColor;

        foreach(Transform line in transform)
        {
            System.Collections.Generic.List<Vector3> pointPosList = new System.Collections.Generic.List<Vector3>();
            foreach (Transform point in line)
                pointPosList.Add(point.position);

            Gizmos.DrawLineStrip(pointPosList.ToArray(), false);
        }

    }


    /// <summary>
    /// 计算AB与CD两条线段的交点.
    /// </summary>
    /// <param name="a">A点</param>
    /// <param name="b">B点</param>
    /// <param name="c">C点</param>
    /// <param name="d">D点</param>
    /// <param name="intersectPos">AB与CD的交点</param>
    /// <returns>是否相交 true:相交 false:未相交</returns>
    private bool TryGetIntersectPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 intersectPos)
    {
        intersectPos = Vector3.zero;

        Vector3 ab = b - a;
        Vector3 ca = a - c;
        Vector3 cd = d - c;

        Vector3 v1 = Vector3.Cross(ca, cd);

        if (Mathf.Abs(Vector3.Dot(v1, ab)) > 1e-6)
        {
            // 不共面
            return false;
        }

        if (Vector3.Cross(ab, cd).sqrMagnitude <= 1e-6)
        {
            // 平行
            return false;
        }

        Vector3 ad = d - a;
        Vector3 cb = b - c;
        // 快速排斥
        if (Mathf.Min(a.x, b.x) > Mathf.Max(c.x, d.x) || Mathf.Max(a.x, b.x) < Mathf.Min(c.x, d.x)
           || Mathf.Min(a.y, b.y) > Mathf.Max(c.y, d.y) || Mathf.Max(a.y, b.y) < Mathf.Min(c.y, d.y)
           || Mathf.Min(a.z, b.z) > Mathf.Max(c.z, d.z) || Mathf.Max(a.z, b.z) < Mathf.Min(c.z, d.z)
        )
            return false;

        // 跨立试验
        if (Vector3.Dot(Vector3.Cross(-ca, ab), Vector3.Cross(ab, ad)) > 0
            && Vector3.Dot(Vector3.Cross(ca, cd), Vector3.Cross(cd, cb)) > 0)
        {
            Vector3 v2 = Vector3.Cross(cd, ab);
            float ratio = Vector3.Dot(v1, v2) / v2.sqrMagnitude;
            intersectPos = a + ab * ratio;
            return true;
        }

        return false;
    }
#endif
}


