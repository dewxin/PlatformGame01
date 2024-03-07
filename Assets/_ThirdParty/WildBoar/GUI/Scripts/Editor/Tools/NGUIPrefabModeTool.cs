using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WildBoar.GUIModule
{
    [InitializeOnLoad]
    class NGUIPrefabModeTool
    {
        static NGUIPrefabModeTool()
        {
            //PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        }

        static void OnPrefabStageOpened(PrefabStage prefabStage)
        {

            GenerateRootPanelInPrefabStage(prefabStage);

        }


        static public void GenerateRootPanelInPrefabStage(PrefabStage prefabStage)
        {

            var rootGO = prefabStage.prefabContentsRoot;
            var rectList = rootGO.GetComponentsInChildren<UIRect>();
            bool containNGUIComponent = rectList.Count() > 0;

            if (!containNGUIComponent)
                return;

            var uiRootComponent = rootGO.GetComponent<NUICanvas>();

            bool missingRoot = uiRootComponent == null;

            if (!missingRoot)
                return;

            // Since this function is called from Awake/OnEnable, utilities like PrefabStage.prefabContentsRoot
            // or Scene.GetRootGameObjects () aren't available at this point

            var instanceRoot = rootGO.transform;
            while (instanceRoot.parent != null)
                instanceRoot = instanceRoot.parent;

            var container = EditorUtility.CreateGameObjectWithHideFlags($"UIRoot ({nameof(NGUIPrefabModeTool)})", HideFlags.DontSave);
            container.layer = instanceRoot.gameObject.layer;
            container.AddComponent<NUICanvas>();
            //var panel = container.AddComponent<UIPanel>();

            //TODO move to Editor Assembly and get info from  GameView.cs

            //    [EditorWindowTitle(title = "Game", useTypeNameAsIconName = true)]
            //internal class GameView : PlayModeView, IHasCustomMenu, IGameViewSizeMenuUser

            //panel.size = new Vector2Int(1024, 768);

            SceneManager.MoveGameObjectToScene(container, prefabStage.scene);
            instanceRoot.SetParent(container.transform, false);
        }
    }

}
