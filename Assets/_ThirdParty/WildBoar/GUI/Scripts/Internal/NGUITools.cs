//-------------------------------------------------
//			  NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class DoNotObfuscateNGUI : Attribute { }

namespace WildBoar.GUIModule
{

	/// <summary>
	/// Helper class containing generic functions used throughout the UI library.
	/// </summary>

	static public class NGUITools
	{

		static public void UndoRecordObject(UnityEngine.Object obj, string name)
		{
#if UNITY_EDITOR
			UnityEditor.Undo.RecordObject(obj, name);
#endif
		}


		static public void EditorSetDirty(UnityEngine.Object obj, string undoName = "last change")
		{
#if UNITY_EDITOR
			if (obj == null)
				return;
			UnityEditor.EditorUtility.SetDirty(obj);

#if UNITY_2018_3_OR_NEWER
			if (!UnityEditor.AssetDatabase.Contains(obj) && !Application.isPlaying)
			{
				if (obj is Component)
				{
					var component = (Component)obj;
					UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
				}
				else if (!(obj is UnityEditor.EditorWindow || obj is ScriptableObject))
				{
					UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
				}
			}
#endif

#endif
		}


		/// Destroy the specified object, immediately if in edit mode.
		static public void Destroy(UnityEngine.Object obj)
		{
			if (obj)
			{
				if (obj is Transform)
				{
					var t = (obj as Transform);
					var go = t.gameObject;

					if (Application.isPlaying)
					{
						go.SetActive(false);
						t.parent = null;
						UnityEngine.Object.Destroy(go);
					}
					else UnityEngine.Object.DestroyImmediate(go);
				}
				else if (obj is GameObject)
				{
					var go = obj as GameObject;
					var t = go.transform;

					if (Application.isPlaying)
					{
						go.SetActive(false);
						t.parent = null;
						UnityEngine.Object.Destroy(go);
					}
					else UnityEngine.Object.DestroyImmediate(go);
				}
				else if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
				else UnityEngine.Object.DestroyImmediate(obj);
			}
		}


		/// <summary>
		/// Destroy the specified object immediately, unless not in the editor, in which case the regular Destroy is used instead.
		/// </summary>
		static public void DestroyImmediate(UnityEngine.Object obj)
		{
			if (obj != null)
			{
				if (Application.isPlaying)
					UnityEngine.Object.Destroy(obj);
				else
					UnityEngine.Object.DestroyImmediate(obj);
			}
		}

	
		/// <summary>
		/// Helper function that returns whether the specified MonoBehaviour is active.
		/// </summary>

		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		static public bool IsActiveAndNotNull(Behaviour mb)
		{
			return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
		}

		/// <summary>
		/// Unity4 has changed GameObject.active to GameObject.activeself.
		/// </summary>

		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		static public bool GetActive(GameObject go)
		{
			return go.activeInHierarchy;
		}

		/// <summary>
		/// Unity4 has changed GameObject.active to GameObject.SetActive.
		/// </summary>

		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		static public void SetActiveSelf(GameObject go, bool state)
		{
			go.SetActive(state);
		}

		/// <summary>
		/// Recursively set the game object's layer.
		/// </summary>

		static public void SetLayer(GameObject go, int layer)
		{
			go.layer = layer;

			Transform t = go.transform;

			for (int i = 0, imax = t.childCount; i < imax; ++i)
			{
				Transform child = t.GetChild(i);
				SetLayer(child.gameObject, layer);
			}
		}


		/// <summary>
		/// Access to the clipboard via undocumented APIs.
		/// </summary>

		static public string clipboard
		{
			get
			{
				var te = new TextEditor();
				te.Paste();
#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			return te.content.text;
#else
				return te.text;
#endif
			}
			set
			{
				TextEditor te = new TextEditor();
#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			te.content = new GUIContent(value);
#else
				te.text = value;
#endif
				te.OnFocus();
				te.Copy();
			}
		}


		/// <summary>
		/// Extension for the game object that checks to see if the component already exists before adding a new one.
		/// If the component is already present it will be returned instead.
		/// </summary>

		static public T AddMissingComponent<T>(this GameObject go) where T : Component
		{
			T comp = go.GetComponent<T>();
			if (comp == null)
			{
#if UNITY_EDITOR
				if (!Application.isPlaying)
					UndoRecordObject(go, "Add " + typeof(T));
#endif
				comp = go.AddComponent<T>();
			}
			return comp;
		}

#if UNITY_EDITOR
		static Func<Vector2> s_GetSizeOfMainGameView;
		[System.NonSerialized] static bool mCheckedMainViewFunc = false;

		// Size of the game view cannot be retrieved from Screen.width and Screen.height when the game view is hidden.
		static public Vector2Int screenSize
		{
			get
			{
				Vector2 screenSize = Vector2.one;


				if (s_GetSizeOfMainGameView == null && !mCheckedMainViewFunc)
				{
					mCheckedMainViewFunc = true;
					System.Type gameViewType = System.Type.GetType("UnityEditor.GameView,UnityEditor");

					var methodInfo = gameViewType.GetMethod("GetSizeOfMainGameView",
						System.Reflection.BindingFlags.Public |
						System.Reflection.BindingFlags.NonPublic |
						System.Reflection.BindingFlags.Static);

					// Create the delegate
					if (methodInfo != null)
						s_GetSizeOfMainGameView = (Func<Vector2>)Delegate.CreateDelegate(typeof(Func<Vector2>), methodInfo);
					else
						Debug.LogWarning("Unable to get the main game view size function");
				}

				if (s_GetSizeOfMainGameView != null)
				{
					screenSize = s_GetSizeOfMainGameView();
				}
				else
				{
					screenSize = new Vector2(Screen.width, Screen.height);
				}
				return new Vector2Int((int)screenSize.x, (int)screenSize.y);
			}
		}

#else
	/// <summary>
	/// Size of the game view cannot be retrieved from Screen.width and Screen.height when the game view is hidden.
	/// </summary>

	static public Vector2Int screenSize { get { return new Vector2Int(Screen.width, Screen.height); } }
#endif


		/// <summary>
		/// Transforms this color from gamma to linear space, but only if the active color space is actually set to linear.
		/// </summary>

		static public Color GammaToLinearSpace(this Color c)
		{
			if (mColorSpace == ColorSpace.Uninitialized)
				mColorSpace = QualitySettings.activeColorSpace;

			if (mColorSpace == ColorSpace.Linear)
			{
				return new Color(
					Mathf.GammaToLinearSpace(c.r),
					Mathf.GammaToLinearSpace(c.g),
					Mathf.GammaToLinearSpace(c.b),
					c.a);
			}
			return c;
		}

		static public Color LinearToGammaSpace(this Color c)
		{
			if (mColorSpace == ColorSpace.Uninitialized)
				mColorSpace = QualitySettings.activeColorSpace;

			if (mColorSpace == ColorSpace.Linear)
			{
				return new Color(
					Mathf.LinearToGammaSpace(c.r),
					Mathf.LinearToGammaSpace(c.g),
					Mathf.LinearToGammaSpace(c.b),
					c.a);
			}
			return c;
		}

		static ColorSpace mColorSpace = ColorSpace.Uninitialized;


	}
}
