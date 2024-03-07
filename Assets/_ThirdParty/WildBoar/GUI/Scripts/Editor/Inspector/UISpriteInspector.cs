using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{

	[CanEditMultipleObjects]
	[CustomEditor(typeof(UISprite), true)]
	public class UISpriteInspector : UIWidgetEditor
	{
		UISprite targetSprite;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetSprite = target as UISprite;
        }


		protected override void DrawCustomProperties()
		{
			base.DrawCustomProperties();

			if (NGUIEditorTools.DrawSectionHeader("Sprite"))
			{
				NGUIEditorTools.BeginContents();

                var newMeshType = (UISpriteMeshType)EditorGUILayout.EnumPopup("Mesh Type", targetSprite.SpriteMeshType);
                if(newMeshType != targetSprite.SpriteMeshType)
                {
                    Undo.RecordObject(targetSprite, "undo");
                    targetSprite.SpriteMeshType = newMeshType;
                }

                if(targetSprite.SpriteMeshType == UISpriteMeshType.Filled)
                {
                    var fillMethod = (UISpriteFillMethod)EditorGUILayout.EnumPopup("Fill Method", targetSprite.fillMethod);
                    if(fillMethod!=targetSprite.fillMethod)
                    {
                        Undo.RecordObject(targetSprite, "undo");
                        targetSprite.fillMethod = fillMethod;
                    }

                    var fillAmount = EditorGUILayout.FloatField("Fill Amount", targetSprite.FillAmount);
                    if (fillAmount != targetSprite.FillAmount)
                    {
                        Undo.RecordObject(targetSprite, "undo");
                        targetSprite.SetFillAmount( fillAmount);
                    }

                }


                var newColor = EditorGUILayout.ColorField(new GUIContent("Color"), targetSprite.Color, false, true, true);
                //var newColor = EditorGUILayout.ColorField("Color", targetSprite.Color);
                if (newColor != targetSprite.Color)
                {
                    Undo.RecordObject(targetSprite, "undo");
                    targetSprite.SetColor(newColor);
                }


                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Sprite"))
                {
                    var selectorWindow = SpriteSelectorWindow.CreateNew();
                    selectorWindow.OnSelectSprite += SetAtlasAndSprite;
                    selectorWindow.SetAtlasAndShow(targetSprite.SpriteAtlas);
                }


                var spriteAtlas = EditorGUILayout.ObjectField(targetSprite.SpriteAtlas, typeof(SpriteAtlas), false) as SpriteAtlas;
                if (spriteAtlas != targetSprite.SpriteAtlas)
                {
                    targetSprite.SetAtlas(spriteAtlas);
                }

                GUILayout.EndHorizontal();

                //NGUIEditorTools.DrawProperty("Material", serializedObject, nameof(UIWidget.mMat));

                NGUIEditorTools.EndContents();
			}

		}

        private void SetAtlasAndSprite(SpriteAtlas spriteAtlas, SpriteData spriteData)
        {
            targetSprite.SetAtlas(spriteAtlas);
            targetSprite.SetSprite(spriteData.Name);
        }


		public override bool HasPreviewGUI()
		{
			return targetSprite.SpriteAtlas != null;
		}

		public override void OnPreviewGUI(Rect rect, GUIStyle background)
		{
            var uv = GetUVs(targetSprite);
            DrawTexture(targetSprite.atlasTexture, rect, uv);
        }


        Rect GetUVs(UISprite uiSprite)
        {
            if (uiSprite.SpriteData == null)
                uiSprite.TryGetSpriteDataByName();

            var sprite = uiSprite.SpriteData;
            Rect UVs = sprite.Rect;
            UVs.x /= uiSprite.atlasTexture.width;
            UVs.width /= uiSprite.atlasTexture.width;
            UVs.y /= uiSprite.atlasTexture.height;
            UVs.height /= uiSprite.atlasTexture.height;
            return UVs;
        }


        public void DrawTexture(Texture2D tex, Rect rect, Rect uv)
        {
            int w = Mathf.RoundToInt(tex.width * uv.width);
            int h = Mathf.RoundToInt(tex.height * uv.height);

            var outerRect = GetResizedSpriteRect(tex, rect, uv);

            // Draw the background
            NGUIEditorTools.DrawTiledTexture(outerRect, NGUIEditorTools.backdropTexture);

            // Draw the sprite
            {
                //这里uv 和 ui渲染的uv是一个意思，就是textureCoord
                GUI.DrawTextureWithTexCoords(outerRect, tex, uv, true);
            }

            GUI.color = Color.white;

            // Draw the lines around the sprite
            Handles.color = Color.black;
            Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMin, outerRect.yMax));
            Handles.DrawLine(new Vector3(outerRect.xMax, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMax));
            Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMin));
            Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMax), new Vector3(outerRect.xMax, outerRect.yMax));

            // Sprite size label
            string text = string.Format("Texture Size: {0}x{1}", w, h);
            EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);
        }

        private Rect GetResizedSpriteRect(Texture2D texture, Rect canvasRect, Rect uv)
        {
            int w = Mathf.RoundToInt(texture.width * uv.width);
            int h = Mathf.RoundToInt(texture.height * uv.height);

            // Create the texture rectangle that is centered inside rect.
            Rect outerRect = canvasRect;
            outerRect.width = w;
            outerRect.height = h;

            if (outerRect.width > 0f)
            {
                float f = canvasRect.width / outerRect.width;
                outerRect.width *= f;
                outerRect.height *= f;
            }

            if (canvasRect.height > outerRect.height)
            {
                outerRect.y += (canvasRect.height - outerRect.height) * 0.5f;
            }
            else if (outerRect.height > canvasRect.height)
            {
                float f = canvasRect.height / outerRect.height;
                outerRect.width *= f;
                outerRect.height *= f;
            }

            if (canvasRect.width > outerRect.width) outerRect.x += (canvasRect.width - outerRect.width) * 0.5f;

            return outerRect;
        }
    }
}
