using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    // Start is called before the first frame update
    public string initScene;


    void Start()
    {
        //LoadCommonPrefab.Work();
        SceneInfo.LoadCommonOnce();
        MapManager.Instance.LoadScene(initScene);

    }

    // Update is called once per frame
    void Update()
    {

        
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class LoadCommonPrefab
{
    static LoadCommonPrefab()
    {
#if UNITY_EDITOR
        SceneManager.sceneLoaded += OnSceneLoad;
#endif
    }

    public static void OnSceneLoad(Scene scene, LoadSceneMode loadMode)
    {
        TryInvokeSceneInfoFunc(scene);

        Debug.Log("On Scene Loaded");
    }

    public static void TryInvokeSceneInfoFunc(Scene scene)
    {
        foreach(var root in scene.GetRootGameObjects())
        {
            var sceneInfoList = root.GetComponentsInChildren<SceneInfo>();
            foreach(var info in sceneInfoList)
            {
                info.InvokeWhenSceneLoadEditor();
            }
        }

    }

}
