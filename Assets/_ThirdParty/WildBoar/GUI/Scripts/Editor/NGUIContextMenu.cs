using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This editor helper class makes it easy to create and show a context menu.
/// It ensures that it's possible to add multiple items with the same name.
/// </summary>

namespace WildBoar.GUIModule
{

	static public class NGUIContextMenu
	{
		public delegate UIDrawCallMaker AddFunc(GameObject go);

		static List<string> mEntries = new List<string>();
		static GenericMenu genericMenu;

        [MenuItem("CONTEXT/UIRectUser/Edit Script (Editor)")]
        private static void OpenUIRectUserEditor(MenuCommand command)
        {
            OpenEditorScript(command);
        }


        [MenuItem("CONTEXT/UIRect/Edit Script (Editor)")]
		private static void OpenUIRectEditor(MenuCommand command)
		{
			OpenEditorScript(command);
		}

		private static void OpenEditorScript(MenuCommand command)
		{
			var editor = Editor.CreateEditor(command.context);
			//var monoScript = MonoScript.FromScriptableObject(editor);
			var editorFileName = $"{editor.GetType().Name}";
			var guid = AssetDatabase.FindAssets(editorFileName).First();

			var assetPath = AssetDatabase.GUIDToAssetPath(guid);
			var assetObj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));

			AssetDatabase.OpenAsset(assetObj);

			Editor.DestroyImmediate(editor);

		}



		/// <summary>
		/// Clear the context menu list.
		/// </summary>

		static public void Clear()
		{
			mEntries.Clear();
			genericMenu = null;
		}

		/// <summary>
		/// Add a new context menu entry.
		/// </summary>

		static public void AddItem(string item, bool isChecked, GenericMenu.MenuFunction2 callback, object param)
		{
			if (callback != null)
			{
				if (genericMenu == null) genericMenu = new GenericMenu();
				int count = 0;

				for (int i = 0; i < mEntries.Count; ++i)
				{
					string str = mEntries[i];
					if (str == item) ++count;
				}
				mEntries.Add(item);

				if (count > 0) item += " [" + count + "]";
				genericMenu.AddItem(new GUIContent(item), isChecked, callback, param);
			}
			else AddDisabledItem(item);
		}

		/// <summary>
		/// Wrapper function called by the menu that in turn calls the correct callback.
		/// </summary>

		static public void AddChild(object obj)
		{
			AddFunc func = obj as AddFunc;
			UIDrawCallMaker widget = func(Selection.activeGameObject);
			if (widget != null) Selection.activeGameObject = widget.gameObject;
		}

		/// <summary>
		/// Add a new context menu entry.
		/// </summary>

		static public void AddChildWidget(string item, bool isChecked, AddFunc callback)
		{
			if (callback != null)
			{
				if (genericMenu == null) genericMenu = new GenericMenu();
				int count = 0;

				for (int i = 0; i < mEntries.Count; ++i)
				{
					string str = mEntries[i];
					if (str == item) ++count;
				}
				mEntries.Add(item);

				if (count > 0) item += " [" + count + "]";
				genericMenu.AddItem(new GUIContent(item), isChecked, AddChild, callback);
			}
			else AddDisabledItem(item);
		}

		/// <summary>
		/// Wrapper function called by the menu that in turn calls the correct callback.
		/// </summary>

		static public void AddSibling(object obj)
		{
			AddFunc func = obj as AddFunc;
			UIDrawCallMaker widget = func(Selection.activeTransform.parent.gameObject);
			if (widget != null) Selection.activeGameObject = widget.gameObject;
		}

		/// <summary>
		/// Add a new context menu entry.
		/// </summary>

		static public void AddSiblingWidget(string item, bool isChecked, AddFunc callback)
		{
			if (callback != null)
			{
				if (genericMenu == null) genericMenu = new GenericMenu();
				int count = 0;

				for (int i = 0; i < mEntries.Count; ++i)
				{
					string str = mEntries[i];
					if (str == item) ++count;
				}
				mEntries.Add(item);

				if (count > 0) item += " [" + count + "]";
				genericMenu.AddItem(new GUIContent(item), isChecked, AddSibling, callback);
			}
			else AddDisabledItem(item);
		}


		/// <summary>
		/// Helper function that adds the specified type to all selected game objects. Used with the menu options above.
		/// </summary>

		static void Attach(object typeParam)
		{
			if (Selection.activeGameObject == null) return;
			System.Type type = (System.Type)typeParam;

			for (int i = 0; i < Selection.gameObjects.Length; ++i)
			{
				GameObject go = Selection.gameObjects[i];
				if (go.GetComponent(type) != null) continue;
				Component cmp = go.AddComponent(type);
				Undo.RegisterCreatedObjectUndo(cmp, "Attach " + type);
			}
		}

		/// <summary>
		/// Helper function.
		/// </summary>

		static void AddMissingItem<T>(GameObject target, string name) where T : MonoBehaviour
		{
			if (target.GetComponent<T>() == null)
				AddItem(name, false, Attach, typeof(T));
		}

		static void OnDelete(object obj)
		{
			GameObject go = obj as GameObject;
			Selection.activeGameObject = go.transform.parent.gameObject;
			Undo.DestroyObjectImmediate(go);
		}

		/// <summary>
		/// Add a new disabled context menu entry.
		/// </summary>

		static public void AddDisabledItem(string item)
		{
			if (genericMenu == null) genericMenu = new GenericMenu();
			genericMenu.AddDisabledItem(new GUIContent(item));
		}

		/// <summary>
		/// Add a separator to the menu.
		/// </summary>

		static public void AddSeparator(string path)
		{
			if (genericMenu == null) genericMenu = new GenericMenu();

			// For some weird reason adding separators on OSX causes the entire menu to be disabled. Wtf?
			if (Application.platform != RuntimePlatform.OSXEditor)
				genericMenu.AddSeparator(path);
		}

		/// <summary>
		/// Show the context menu with all the added items.
		/// </summary>

		static public void Show()
		{
			if (genericMenu != null)
			{
				genericMenu.ShowAsContext();
				genericMenu = null;
				mEntries.Clear();
			}
		}
	}
}
