using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{
    [InitializeOnLoad]
    internal class ProjectWindowThumbnail
    {
        static ProjectWindowThumbnail()
        {
            //EditorApplication.projectWindowItemOnGUI += ProjectWindowItemCallback;
            EditorApplication.projectWindowItemInstanceOnGUI += ProjectWindowItemInstanceCallback;
        }

        public static void ProjectWindowItemCallback(string guid, Rect selectionRect) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.EndsWith("asset"))
                return;

            var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
            if(atlas== null)
                return;
            
            GUI.DrawTexture(selectionRect, atlas.Texture);

        }

        public static void ProjectWindowItemInstanceCallback(int instanceID, Rect selectionRect)
        {
            var item = EditorUtility.InstanceIDToObject(instanceID);
            if (item is not SpriteAtlas)
                return;
            if (selectionRect.height < 20)
                return;

            var atlas = (SpriteAtlas)item;
            if (atlas == null)
                return;

            var picRect = selectionRect;
            picRect.height *= 0.8f;
            picRect.width *= 0.9f;
            NGUIEditorTools.DrawTiledTexture(picRect, NGUIEditorTools.GridTexture);
            if(atlas.Texture != null)
                GUI.DrawTexture(picRect, atlas.Texture);

        }
    }
}
