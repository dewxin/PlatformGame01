using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToolWindow : EditorWindow
{
    public SceneAsset sceneAsset;


    [MenuItem("Tools/Windows/SceneTool")]
    private static void CreateWindow()
    {
        GetWindow<SceneToolWindow>();
    }


    private void OnGUI()
    {
        sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(sceneAsset, typeof(SceneAsset), true);

        if (GUILayout.Button("Scene"))
        {
            OperateScene();
        }

    }

    private void OperateScene()
    {
        Debug.Log(sceneAsset.name);
        Debug.Log(AssetDatabase.GetAssetPath(sceneAsset));
        var scene = EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset), OpenSceneMode.Additive);
        var gameobjArray = scene.GetRootGameObjects();

        var enviroment = gameobjArray.ToList().First(go => go.tag == "Evironment");
    }
}
