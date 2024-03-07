using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AdjustPlatformTool))]
public class AdjustPlatformToolEditor : Editor
{
    private float degDiffThreshold = 10;

    public override void OnInspectorGUI()
    {
        AdjustPlatformTool tool = (AdjustPlatformTool)target;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("degDiffThreshold");


        degDiffThreshold = EditorGUILayout.FloatField(degDiffThreshold);

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("MakePlatformOrthogonal"))
        {
            tool.AjustPlatformOrthogonal(degDiffThreshold);
            EditorUtility.SetDirty(tool.gameObject);
        }
    }
}
