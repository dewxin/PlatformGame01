using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenPlatformColliderTool))]
public class GenPlatformColliderToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GenPlatformColliderTool tool = (GenPlatformColliderTool)target;

        if(GUILayout.Button("Generate Collider"))
        {
            var colliderObj = tool.GenerateColliders();
            EditorUtility.SetDirty(colliderObj);
        }
    }
}
