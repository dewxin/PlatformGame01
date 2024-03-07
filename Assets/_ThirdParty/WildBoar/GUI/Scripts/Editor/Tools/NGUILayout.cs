using UnityEngine;

//using static UnityEditor.EditorGUILayout;
using static UnityEngine.GUILayout;

public class NGUILayout
{
    public static HorizontalScope HorizontalScope()
    {
        return new HorizontalScope();
    }

    public static HorizontalScope HorizontalScope(params GUILayoutOption[] options)
    {
        return new HorizontalScope(options);
    }

    public static HorizontalScope HorizontalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
    {
        return new HorizontalScope(content, style, options);
    }
}