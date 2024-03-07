using Codice.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace WildBoar.GUIModule
{

    //TODO replace it with toolkit
    //https://docs.unity3d.com/Manual/UIE-simple-ui-toolkit-workflow.html
    public class SpriteSelectorWindow:EditorWindow
    {

        private const string STR_ATLAS = "Atlas";
        private const string STR_SPRITE = "Sprite";

        private int toolbarInt = 0;
        private string[] toolbarStrings = new string[] { STR_ATLAS, STR_SPRITE };

        private List<SpriteAtlas> atlasList = new List<SpriteAtlas>();

        private SpriteAtlas currentAtlas;
        public SpriteAtlas CurrentAtlas => currentAtlas;

        public AtlasDrawer AtlasDrawer { get; private set; }
        public Action<SpriteAtlas, SpriteData> OnSelectSprite = delegate { };

        // open the window from the menu item Example -> GUI Color
        [MenuItem("Tools/Windows/SpriteSelectorWindow")]
        static void Init()
        {
            EditorWindow window = GetWindow<SpriteSelectorWindow>();
            window.Show();
        }

        public static SpriteSelectorWindow CreateNew ()
        {
            SpriteSelectorWindow window = GetWindow<SpriteSelectorWindow>();
            return window;
        }


        public void SetAtlasAndShow(SpriteAtlas atlas)
        {
            if (atlas != null)
            {
                SetAtlas(atlas);
                toolbarInt = 1;
            }
            Show();

        }



        public void OnGUI()
        {
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, GUILayout.Height(40));
            if (toolbarStrings[toolbarInt].Equals(STR_ATLAS))
            {
                HandleAtlas();
            }
            else if(toolbarStrings[toolbarInt].Equals(STR_SPRITE))
            {
                if (CurrentAtlas == null)
                {
                    GUILayout.Label("No Atlas selected.", "LODLevelNotifyText");
                    return;
                }

                AtlasDrawer.DrawSpriteDatas();
            }
        }

        private void HandleAtlas()
        {
            if (atlasList.Count() == 0)
            {
                GUILayout.Label("No Atlas Found Yet...", "LODLevelNotifyText");
            }

            //FindObjectsOfTypeAll 只能加载已经在内存中的资源。
            foreach(var atlas in atlasList) 
            {
                GUIStyle style = GUI.skin.button;
                if (CurrentAtlas!=null && atlas.name == CurrentAtlas.name)
                    style = GUI.skin.textArea;
                if (GUILayout.Button(atlas.name, style))
                {
                    EditorGUIUtility.PingObject(atlas);
                    SetAtlas(atlas);
                }
            }


            GUILayout.Space(6f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool search = GUILayout.Button("Show All", "LargeButton", GUILayout.Width(120f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (search)
                SearchAtlasInAssetDatabase();
        }

        private void SetAtlas(SpriteAtlas spriteAtlas)
        {
            this.currentAtlas = spriteAtlas;
            AtlasDrawer = new AtlasDrawer(CurrentAtlas, OnSelectSprite);
        }
       

        void SearchAtlasInAssetDatabase()
        {

            var paths = AssetDatabase.GetAllAssetPaths();

            var assetPathList = new List<string>();

            foreach(var path in paths)
            {
                if(path.EndsWith("asset"))
                    assetPathList.Add(path);
            }


            var list = new List<SpriteAtlas>();


            for (int i = 0; i < assetPathList.Count; ++i)
            {
                var path = assetPathList[i];

                EditorUtility.DisplayProgressBar("Loading", "Searching assets, please wait...", (float)i / assetPathList.Count);
                //todo 这里感觉好像其实也有点问题。。。
                var obj = AssetDatabase.LoadMainAssetAtPath(path) as SpriteAtlas;
                if (obj == null || list.Contains(obj)) continue;

                    var t = obj.GetType();
                    if (t == typeof(SpriteAtlas)) 
                        list.Add(obj);
            }
            list.Sort((a1, a2) => a1.name.CompareTo(a2.name) );
            atlasList = list;
            EditorUtility.ClearProgressBar();
        }



    }
}
