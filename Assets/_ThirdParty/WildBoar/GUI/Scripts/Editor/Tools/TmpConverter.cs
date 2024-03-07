using MacFsWatcher;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SearchService;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace WildBoar.GUIModule
{
    internal class TmpConverter : EditorWindow
    {

        Texture2D oldTexture2D;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/TmpConverter")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            TmpConverter window = (TmpConverter)EditorWindow.GetWindow(typeof(TmpConverter));
            window.Show();
        }

        void OnGUI()
        {
            oldTexture2D = EditorGUILayout.ObjectField("Atlas", oldTexture2D, typeof(Texture2D), false) as Texture2D;
            if(oldTexture2D!=null && !EditorUtility.IsPersistent(oldTexture2D))
            {
                EditorGUILayout.LabelField($"Atlas {oldTexture2D.name} is not persistent");
                return;
            }

            if (GUILayout.Button("Generate"))
            {
                var factory = new SpriteDataProviderFactories();
                factory.Init();
                var dataProvider = factory.GetSpriteEditorDataProviderFromObject(oldTexture2D);
                dataProvider.InitSpriteEditorDataProvider();
                var spriteRectArray = dataProvider.GetSpriteRects();

                var assetPath = AssetDatabase.GetAssetPath(oldTexture2D);
                var atlasPath = assetPath.Replace("png", "asset");
                SpriteAtlas spriteAtlas = ScriptableObject.CreateInstance<SpriteAtlas>();

                var shader = Shader.Find("Unlit/Transparent Colored");
                var material = new Material(shader);
                var newTexture = DuplicateTexture(oldTexture2D);

                material.SetTexture("_MainTex", newTexture);
                spriteAtlas.Material = material;
                foreach (var spriteRect in spriteRectArray )
                {
                    var spriteData = Convert1(spriteRect);
                    spriteAtlas.AddData(spriteData);
                }

                AssetDatabase.CreateAsset(spriteAtlas, atlasPath);



                AssetDatabase.AddObjectToAsset(newTexture, atlasPath);
                AssetDatabase.AddObjectToAsset(material, atlasPath);

                AssetDatabase.SaveAssets();
            }
        }


        private SpriteData Convert1(SpriteRect spriteRect)
        {

            var spriteData = new SpriteData()
            {
                Name = spriteRect.name,
                Rect = spriteRect.rect,
                Slice9Padding = spriteRect.border,
                Pivot = spriteRect.pivot,

            };
            return spriteData;
        }


        private Texture2D DuplicateTexture(Texture2D inTex)
        {
            int width = inTex.width;
            int height = inTex.height;

            var outTex = new Texture2D(width, height, inTex.format, false);
            outTex.name = "Texture";
            outTex.alphaIsTransparency = true;
            outTex.filterMode = FilterMode.Point;
            Graphics.CopyTexture(inTex, outTex);
            return outTex;

        }
    }
    
}
