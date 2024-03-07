using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SceneInfo:MonoBehaviour
{
    public List<GameObject> LoadTheseOnceInEditor = new List<GameObject>();

    public bool LoadCommon = true;

    public void InvokeWhenSceneLoadEditor()
    {
        foreach(var prefab in LoadTheseOnceInEditor)
        {
            MonoBehaviour.Instantiate(prefab);
        }


        if(LoadCommon)
            LoadCommonOnce();
    }


    public static void LoadCommonOnce()
    {
        if (DataCache.Instance.sceneLoadOnceInvoked)
            return;

        DataCache.Instance.sceneLoadOnceInvoked = true;

        var playePrefab = Resources.Load("Prefabs/Common/Player");
        var player = MonoBehaviour.Instantiate(playePrefab);

        var cameraPrefab = Resources.Load("Prefabs/Common/Camera");
        var camera = MonoBehaviour.Instantiate(cameraPrefab);

        var gameManagerPrefab = Resources.Load("Prefabs/Common/GameManager");
        var gameManager = MonoBehaviour.Instantiate(gameManagerPrefab);

    }

    public class DataCache : Singleton<DataCache>
    {
        public  bool sceneLoadOnceInvoked = false;
    }

}



