using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class ToolBarExample : EditorWindow
{

    // open the window from the menu item Example -> GUI Color
    [MenuItem("Tools/Example/ToolBarExample")]
    static void Init()
    {
        EditorWindow window = GetWindow<ToolBarExample>();
        window.Show();
    }


    public int toolbarInt = 0;
    public string[] toolbarStrings = new string[] { "Toolbar1", "Toolbar2", "Toolbar3" };

    void OnGUI()
    {
        var style = new GUIStyle( GUI.skin.label);
        style.richText= true;

        var scriptAsset = MonoScript.FromScriptableObject(this);
        //AssetDatabase.OpenAsset(scriptAsset);
        var scriptAssetPath = AssetDatabase.GetAssetPath(scriptAsset);
        //Debug.Log(scriptAssetPath);
        EditorGUILayout.SelectableLabel($"<a href=\"{scriptAssetPath}\">Link Click Me</a>", style);

        var skin = Resources.Load<GUISkin>("MySkin");
        GUI.skin = skin;
        //toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
        toolbarInt = GUILayout.Toolbar( toolbarInt, toolbarStrings);

        GUILayout.Label($"{toolbarStrings[toolbarInt]} is selected");
    }

}
