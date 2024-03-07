using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{

	[CustomEditor(typeof(UISpriteDecorator), true)]
	public class UISpriteDecoratorInspector : UIWidgetEditor
	{
        UISpriteDecorator targetSpriteDecorator;

        UISprite targetSprite;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetSpriteDecorator = target as UISpriteDecorator;
            targetSprite = targetSpriteDecorator.GetComponent<UISprite>();
        }


		protected override void DrawCustomProperties()
		{
			base.DrawCustomProperties();

			NGUIEditorTools.BeginContents();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Normal"))
            {
                var selectorWindow = SpriteSelectorWindow.CreateNew();
                selectorWindow.OnSelectSprite += 
                    (atlas, data) => { EditorUtility.SetDirty(targetSpriteDecorator); targetSpriteDecorator.normalSpriteName = data.Name; };
                selectorWindow.SetAtlasAndShow(targetSprite.SpriteAtlas);

            }
            EditorGUILayout.LabelField(targetSpriteDecorator.normalSpriteName);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Hover"))
            {
                var selectorWindow = SpriteSelectorWindow.CreateNew();
                selectorWindow.OnSelectSprite +=
                    (atlas, data) => { EditorUtility.SetDirty(targetSpriteDecorator); targetSpriteDecorator.hoverSpriteName = data.Name; };
                selectorWindow.SetAtlasAndShow(targetSprite.SpriteAtlas);
            }
            EditorGUILayout.LabelField(targetSpriteDecorator.hoverSpriteName);
            GUILayout.EndHorizontal();


            NGUIEditorTools.EndContents();

		}



    }
}
