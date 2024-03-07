using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TeleportOutPosition), true)]
public class TeleportOutPositionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var teleportOutPos = target as TeleportOutPosition;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(teleportOutPos.scenePath);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        var teleportPosProperty = serializedObject.FindProperty("TeleportPos");
        EditorGUILayout.PropertyField(teleportPosProperty);

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("scenePath");
            scenePathProperty.stringValue = newPath;

            var sceneNameProperty = serializedObject.FindProperty("sceneName");
            sceneNameProperty.stringValue = newScene.name;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
