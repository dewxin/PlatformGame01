//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor helper class containing functions related to drawing things in the Scene View using UnityEditor.Handles.
/// </summary>

static public class NGUIHandles
{

	static NGUIHandles()
	{
        Tools.viewToolChanged += Tools_viewToolChanged;

	}

    private static void Tools_viewToolChanged()
    {

    }

    static public bool TryRaycastPlane (Plane plane, Vector2 screenPos, out Vector3 hitPointPos)
	{
		float dist;
		Ray ray = HandleUtility.GUIPointToWorldRay(screenPos);

		if (plane.Raycast(ray, out dist))
		{
			hitPointPos = ray.GetPoint(dist);
			return true;
		}
		hitPointPos = Vector3.zero;
		return false;
	}

    /// <summary>
    /// Draw width and height around the rectangle specified by the four world-space corners.
    /// </summary>
    public static void DrawSizeLabel (Vector3[] fourCornersInWorldPos, int width, int height)
	{
		Vector2[] screenPos = new Vector2[4];

		for (int i = 0; i < 4; ++i)
			screenPos[i] = HandleUtility.WorldToGUIPoint(fourCornersInWorldPos[i]);

        Plane plane = new Plane(fourCornersInWorldPos[0], fourCornersInWorldPos[1], fourCornersInWorldPos[3]);
        Color color = new Color(1f, 1f, 1f, 1f);
        
		Vector2 parallelogramUp = (screenPos[1] - screenPos[0]).normalized;
		Vector2 parallelogramRight = (screenPos[3] - screenPos[0]).normalized;

		//left line
		Vector2 leftLineBottomPoint = screenPos[0] - parallelogramRight * 20f;
		Vector2 leftLineUpPoint = screenPos[1] - parallelogramRight * 20f;
        DrawShadowedLine(plane, leftLineBottomPoint, leftLineUpPoint, color);
        DrawShadowedLine(plane, leftLineBottomPoint - parallelogramRight * 10f, leftLineBottomPoint + parallelogramRight * 10f, color);
        DrawShadowedLine(plane, leftLineUpPoint - parallelogramRight * 10f, leftLineUpPoint + parallelogramRight * 10f, color);

		//bottom line
        Vector2 bottomLineLeftPoint = screenPos[0] - parallelogramUp * 20f;
		Vector2 bottomLineRightPoint = screenPos[3] - parallelogramUp * 20f;
		DrawShadowedLine(plane, bottomLineLeftPoint, bottomLineRightPoint, color);
		DrawShadowedLine(plane, bottomLineLeftPoint - parallelogramUp * 10f, bottomLineLeftPoint + parallelogramUp * 10f, color);
		DrawShadowedLine(plane, bottomLineRightPoint - parallelogramUp * 10f, bottomLineRightPoint + parallelogramUp * 10f, color);


        Handles.BeginGUI();
        DrawCenteredLabel((bottomLineLeftPoint + bottomLineRightPoint) * 0.5f, width.ToString());
        DrawCenteredLabel((leftLineBottomPoint + leftLineUpPoint) * 0.5f, height.ToString());
        Handles.EndGUI();
	}

	public static void DrawCornersLines(RectCorners rectCorners)
	{
		DrawCornersLines(rectCorners, Color.white);
    }

	public static void DrawCornersLines(RectCorners rectCorners, Color color)
	{
		Handles.color= color;
        Handles.DrawLine(rectCorners.BottomLeft, rectCorners.TopLeft);
        Handles.DrawLine(rectCorners.TopLeft, rectCorners.TopRight);
        Handles.DrawLine(rectCorners.TopRight, rectCorners.BottomRight);
        Handles.DrawLine(rectCorners.BottomRight, rectCorners.BottomLeft);

		
		//TODO 如果需要画shadow，需要像素的1单位，但下面这个是unity的场景单位1unit
		//Handles.color = new Color(0f, 0f, 0f, 0.5f);
  //      Handles.DrawLine(rectCorners.BottomLeft + Vector3.one, rectCorners.TopLeft + Vector3.one);
  //      Handles.DrawLine(rectCorners.TopLeft + Vector3.one, rectCorners.TopRight + Vector3.one);
  //      Handles.DrawLine(rectCorners.TopRight + Vector3.one, rectCorners.BottomRight + Vector3.one);
  //      Handles.DrawLine(rectCorners.BottomRight + Vector3.one, rectCorners.BottomLeft + Vector3.one);
    }

    public static void DrawShadowedLine (Plane plane, Vector2 lineEndPoint1, Vector2 lineEndPoint2, Color color)
	{
		Handles.color = new Color(0f, 0f, 0f, 0.5f);
		DrawLineOnPlane(plane, lineEndPoint1 + Vector2.one, lineEndPoint2 + Vector2.one);
		Handles.color = color;
		DrawLineOnPlane(plane, lineEndPoint1, lineEndPoint2);
	}


	//TODO 提高一下语义 ，可以新增一个参数比较少的函数
	static public void DrawShadowedLine (Vector3[] corners, Vector3 worldPos0, Vector3 worldPos1, Color c)
	{
		Plane p = new Plane(corners[0], corners[1], corners[3]);
		Vector2 s0 = HandleUtility.WorldToGUIPoint(worldPos0);
		Vector2 s1 = HandleUtility.WorldToGUIPoint(worldPos1);
		DrawShadowedLine(p, s0, s1, c);
	}

	static public void DrawLineOnPlane (Plane plane, Vector2 screenPos1, Vector2 screenPos2)
	{
		Vector3 hitPos1, hitPos2;
		if (TryRaycastPlane(plane, screenPos1, out hitPos1) && TryRaycastPlane(plane, screenPos2, out hitPos2))
			Handles.DrawLine(hitPos1, hitPos2);
	}

	/// <summary>
	/// Draw a centered label at the specified world coordinates.
	/// </summary>
	static public void DrawCenteredLabel (Vector3 worldPos, string text)
	{
		Vector2 screenPoint = HandleUtility.WorldToGUIPoint(worldPos);
		DrawCenteredLabel(screenPoint, text);
	}

	/// <summary>
	/// Draw a centered label at the specified screen coordinates.
	/// It's expected that this call happens inside Handles.BeginGUI() / Handles.EndGUI().
	/// </summary>
	static public void DrawCenteredLabel (Vector2 screenPos, string text)
	{
		if (Event.current.type == EventType.Repaint)
		{
			//float tw = Mathf.Max(2, text.Length) * 15f;
			//GUI.color = new Color(0f, 0f, 0f, 0.75f);
			//GUI.Box(new Rect(screenPos.x - tw * 0.5f, screenPos.y - 10f, tw, 20f), "", "WinBtnInactiveMac");

			GUI.Label(new Rect(screenPos.x - 30f, screenPos.y - 10f, 60f, 20f), text, "PreLabel");
		}
	}
}
