using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEditorInternal;
using System.IO;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;


public class MiniMapGenerator : EditorWindow
{
    GameObject gameObject;
    Editor gameObjectEditor;
    Editor transformEditor;

    Camera camera;

    private Texture2D texture2D;


    [MenuItem("Tools/Windows/MiniMapGenerator")]
    static void ShowWindow()
    {
        var win = GetWindow<MiniMapGenerator>();
        win.Show();
    }

    private void OnEnable()
    {
        camera = CreateSceneCamera();
        RenderPipelineManager.beginCameraRendering += OnBeginCamera;

    }

    private void OnBeginCamera(ScriptableRenderContext context, Camera cam)
    {
        if(camera== cam)
        {
            var shader = Shader.Find("CustomEffects/RedTint");
            var material = new Material(shader);
            var pass = new RedTintRenderPass(material);
            pass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            // Use the EnqueuePass method to inject a custom render pass
            cam.GetUniversalAdditionalCameraData()
                .scriptableRenderer.EnqueuePass(pass);
            

        }

    }

    private void OnDisable()
    {
        DestroyImmediate(camera.gameObject);
        RenderPipelineManager.beginCameraRendering -= OnBeginCamera;
    }

    void OnGUI()
    {
        DrawSceneView();
        //gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);

        //GUIStyle bgColor = new GUIStyle();
        //bgColor.normal.background = EditorGUIUtility.whiteTexture;
        //ShowPreview(gameObject, bgColor);

    }

    private Camera CreateSceneCamera()
    {
        GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags("MapGen_SceneCamera", HideFlags.DontSave, typeof(Camera));
        var camera = gameObject.GetComponent<Camera>();
        camera.enabled = false;
        return camera;
    }


    private void DrawSceneView()
    {
        if(GUILayout.Button("Set Camera"))
        {
            SetCamera(SceneView.lastActiveSceneView);
        }

        if (GUILayout.Button("Generate Scene View Png"))
        {
            //CaptureEditorScreenshot("sceneView.png", false);

            CameraCapture();

        }

       // GUI.DrawTexture(new Rect(10, 10, 500, 100), texture2D);
    }

    private void SetCamera(SceneView sceneView)
    {
        Debug.Assert(camera!= null);
        Debug.Assert(sceneView!=null);

        var root = GameObject.FindGameObjectWithTag("Environment");
        var mapBoundry = GetMapBoundry(new GameObject[] { root });
        var mapRect = mapBoundry.GetBoundry();


        camera.CopyFrom(sceneView.camera);

        camera.enabled = false;
        //camera.backgroundColor = Color.white;
        camera.cullingMask = LayerMask.GetMask("MapVisual");

        camera.transform.position = mapBoundry.GetBoundry().center;
        camera.transform.position -= camera.transform.forward*10f;

        camera.orthographicSize = mapRect.height / 2;
        camera.aspect = mapRect.width / mapRect.height;

        camera.SetReplacementShader(Shader.Find("Hidden/MiniMap"),"");


    }


    private RectBoundry GetMapBoundry(GameObject[] rootList)
    {
        foreach(var root in rootList)
        {
            var mapBoundryList = root.GetComponentsInChildren<RectBoundry>().Where(b => b.IsMapBoundry);
            if(mapBoundryList.Count() == 1)
                return mapBoundryList.Single();
        }
        return null;
    }



    void CameraCapture(float pixelPerUnit = 250)
    {

        SetCamera(SceneView.lastActiveSceneView);

        var widthPixel = camera.orthographicSize * pixelPerUnit;
        var heightPixel = widthPixel / camera.aspect;

        int widthInt = Mathf.RoundToInt(widthPixel);
        int heightInt = Mathf.RoundToInt(heightPixel);
        RenderTexture rt = new RenderTexture(widthInt, heightInt, 16);
        camera.targetTexture = rt;
        //一定要调用render函数
        //camera.Render();
        var shader = Shader.Find("Hidden/MiniMap");
        Debug.Log(shader.name);
        camera.Render();
        RenderTexture.active = rt;
        Texture2D texture = new Texture2D(widthInt, heightInt);
        texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture.Apply();

        var sceneName = SceneManager.GetActiveScene().name;

        string path = Path.Combine(Application.dataPath,"Visual","MiniMap",$"{sceneName}.png");
        var pngData = texture.EncodeToPNG();

        FileStream file = File.Create(path);

        if (!file.CanWrite)
        {
            Debug.LogError("Unable to capture editor screenshot, Failed to open file for writing");
            return ;
        }
        file.Write(pngData, 0, pngData.Length);

        file.Close();
        camera.targetTexture = null;

        AssetDatabase.Refresh();
        ///<see cref="ProjectBrowser"/>
    }

    [Obsolete]
    public bool CaptureEditorScreenshot(string _filename, bool _transparent)
    {
        SceneView sw = SceneView.lastActiveSceneView;

        if (sw == null)
        {
            Debug.LogError("Unable to capture editor screenshot, no scene view found");
            return false;
        }


        if (camera == null)
        {
            Debug.LogError("Unable to capture editor screenshot, no camera attached to current scene view");
            return false;
        }

        RenderTexture renderTexture = camera.targetTexture;

        if (renderTexture == null)
        {
            Debug.LogError("Unable to capture editor screenshot, camera has no render texture attached");
            return false;
        }


        camera.Render();

        int width = renderTexture.width;
        int height = renderTexture.height;

        var outputTexture = new Texture2D(width, height);

        RenderTexture.active = renderTexture;

        outputTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        outputTexture.Apply();

        byte[] pngData = outputTexture.EncodeToPNG();

        FileStream file = File.Create(_filename);

        if (!file.CanWrite)
        {
            Debug.LogError("Unable to capture editor screenshot, Failed to open file for writing");
            return false;
        }

        file.Write(pngData, 0, pngData.Length);

        file.Close();

        if (texture2D != null)
            DestroyImmediate(texture2D);
        texture2D = outputTexture;

        Debug.Log("Screenshot written to file " + _filename);

        return true;
    }
}
