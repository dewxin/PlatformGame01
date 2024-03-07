//-------------------------------------------------
//			  NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace WildBoar.GUIModule
{
	/// <summary>
	/// Tools for the editor
	/// </summary>

	static public class NGUIEditorTools
	{
		private static Texture2D mBackdropTex;
		private static Texture2D mGridTex;
		private static Texture2D mContrastTex;
		private static Texture2D mGradientTex;
		private static Object mPreviousSelection;

		/// <summary>
		/// Returns a blank usable 1x1 white texture.
		/// </summary>

		static public Texture2D blankTexture
		{
			get
			{
				return EditorGUIUtility.whiteTexture;
			}
		}

		/// <summary>
		/// Returns a usable texture that looks like a dark checker board.
		/// </summary>

		static public Texture2D backdropTexture
		{
			get
			{
				if (mBackdropTex == null) mBackdropTex = CreateCheckerTex(
					new Color(0.1f, 0.1f, 0.1f, 0.5f),
					new Color(0.2f, 0.2f, 0.2f, 0.5f));
				return mBackdropTex;
			}
		}

        static public Texture2D GridTexture
        {
            get
            {
                if (mGridTex == null) mGridTex = CreateCheckerTex(
                    new Color(1f, 1f, 1f, 1f),
                    new Color(0.7f, 0.7f, 0.7f, 1f));
                return mGridTex;
            }
        }

        /// <summary>
        /// Returns a usable texture that looks like a high-contrast checker board.
        /// </summary>

        static public Texture2D contrastTexture
		{
			get
			{
				if (mContrastTex == null) mContrastTex = CreateCheckerTex(
					new Color(0f, 0f, 0f, 0.5f),
					new Color(1f, 1f, 1f, 0.5f));
				return mContrastTex;
			}
		}

		/// <summary>
		/// Gradient texture is used for title bars / headers.
		/// </summary>

		static public Texture2D gradientTexture
		{
			get
			{
				if (mGradientTex == null) mGradientTex = CreateGradientTex();
				return mGradientTex;
			}
		}

		/// <summary>
		/// Create a white dummy texture.
		/// </summary>

		private static Texture2D CreateDummyTex()
		{
			var tex = new Texture2D(1, 1);
			tex.name = "[Generated] Dummy Texture";
			tex.hideFlags = HideFlags.DontSave;
			tex.filterMode = FilterMode.Point;
			tex.SetPixel(0, 0, Color.white);
			tex.Apply();
			return tex;
		}

		/// <summary>
		/// Create a checker-background texture
		/// </summary>

		private static Texture2D CreateCheckerTex(Color c0, Color c1)
		{
			var tex = new Texture2D(128, 128);
			tex.name = "[Generated] Checker Texture";
			tex.hideFlags = HideFlags.DontSave;

			for (int iy = 0; iy < 8; ++iy)
			{
				var oy = iy * 16;

				for (int ix = 0; ix < 8; ++ix)
				{
					var ox = ix * 16;

					for (int y = 0; y < 8; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(ox + x, oy + y, c1);
					for (int y = 8; y < 16; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(ox + x, oy + y, c0);
					for (int y = 0; y < 8; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(ox + x, oy + y, c0);
					for (int y = 8; y < 16; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(ox + x, oy + y, c1);
				}
			}

			tex.Apply();
			tex.filterMode = FilterMode.Point;
			return tex;
		}

		/// <summary>
		/// Create a gradient texture
		/// </summary>

		private static Texture2D CreateGradientTex()
		{
			Texture2D tex = new Texture2D(1, 16);
			tex.name = "[Generated] Gradient Texture";
			tex.hideFlags = HideFlags.DontSave;

			Color c0 = new Color(1f, 1f, 1f, 0f);
			Color c1 = new Color(1f, 1f, 1f, 0.4f);

			for (int i = 0; i < 16; ++i)
			{
				float f = Mathf.Abs((i / 15f) * 2f - 1f);
				f *= f;
				tex.SetPixel(0, i, Color.Lerp(c0, c1, f));
			}

			tex.Apply();
			tex.filterMode = FilterMode.Bilinear;
			return tex;
		}

		/// <summary>
		/// Draws the tiled texture. Like GUI.DrawTexture() but tiled instead of stretched.
		/// </summary>

		static public void DrawTiledTexture(Rect rect, Texture tex)
		{
			int width = Mathf.RoundToInt(rect.width);
			int height = Mathf.RoundToInt(rect.height);
			var tw = tex.width;
			var th = tex.height;

			if (width <= tw && height <= th)
			{
				var tc = new Rect(0f, 0f, (float)width / tw, (float)height / th);
				GUI.DrawTextureWithTexCoords(rect, tex, tc, true);
			}
			else
			{
				GUI.BeginGroup(rect);
				{
					for (int y = 0; y < height; y += th)
					{
						for (int x = 0; x < width; x += tw)
						{
							GUI.DrawTexture(new Rect(x, y, tw, th), tex);
						}
					}
				}
				GUI.EndGroup();
			}
		}

		/// <summary>
		/// Draw a single-pixel outline around the specified rectangle.
		/// </summary>

		static public void DrawOutline(Rect rect)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Texture2D tex = contrastTexture;
				GUI.color = Color.white;
				DrawTiledTexture(new Rect(rect.xMin, rect.yMax, 1f, -rect.height), tex);
				DrawTiledTexture(new Rect(rect.xMax, rect.yMax, 1f, -rect.height), tex);
				DrawTiledTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
				DrawTiledTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
			}
		}

		/// <summary>
		/// Draw a single-pixel outline around the specified rectangle.
		/// </summary>

		static public void DrawOutline(Rect rect, Color color)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Texture2D tex = blankTexture;
				GUI.color = color;
				GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, 1f, rect.height), tex);
				GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, 1f, rect.height), tex);
				GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
				GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
				GUI.color = Color.white;
			}
		}

		/// <summary>
		/// Draw a selection outline around the specified rectangle.
		/// </summary>

		static public void DrawOutline(Rect rect, Rect relative, Color color)
		{
			if (Event.current.type == EventType.Repaint)
			{
				// Calculate where the outer rectangle would be
				float x = rect.xMin + rect.width * relative.xMin;
				float y = rect.yMax - rect.height * relative.yMin;
				float width = rect.width * relative.width;
				float height = -rect.height * relative.height;
				relative = new Rect(x, y, width, height);

				// Draw the selection
				DrawOutline(relative, color);
			}
		}

		/// <summary>
		/// Draw a selection outline around the specified rectangle.
		/// </summary>

		static public void DrawOutline(Rect rect, Rect relative)
		{
			if (Event.current.type == EventType.Repaint)
			{
				// Calculate where the outer rectangle would be
				float x = rect.xMin + rect.width * relative.xMin;
				float y = rect.yMax - rect.height * relative.yMin;
				float width = rect.width * relative.width;
				float height = -rect.height * relative.height;
				relative = new Rect(x, y, width, height);

				// Draw the selection
				DrawOutline(relative);
			}
		}

		/// <summary>
		/// Draw a 9-sliced outline.
		/// </summary>

		static public void DrawOutline(Rect rect, Rect outer, Rect inner)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Color green = new Color(0.4f, 1f, 0f, 1f);

				DrawOutline(rect, new Rect(outer.x, inner.y, outer.width, inner.height));
				DrawOutline(rect, new Rect(inner.x, outer.y, inner.width, outer.height));
				DrawOutline(rect, outer, green);
			}
		}

		/// <summary>
		/// Draw a checkered background for the specified texture.
		/// </summary>

		static public Rect DrawBackground(Texture2D tex, float ratio)
		{
			Rect rect = GUILayoutUtility.GetRect(0f, 0f);
			rect.width = Screen.width - rect.xMin;
			rect.height = rect.width * ratio;
			GUILayout.Space(rect.height);

			if (Event.current.type == EventType.Repaint)
			{
				Texture2D blank = blankTexture;
				Texture2D check = backdropTexture;

				// Lines above and below the texture rectangle
				GUI.color = new Color(0f, 0f, 0f, 0.2f);
				GUI.DrawTexture(new Rect(rect.xMin, rect.yMin - 1, rect.width, 1f), blank);
				GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), blank);
				GUI.color = Color.white;

				// Checker background
				DrawTiledTexture(rect, check);
			}
			return rect;
		}

		/// <summary>
		/// Draw a visible separator in addition to adding some padding.
		/// </summary>

		static public void DrawSeparator()
		{
			GUILayout.Space(12f);

			if (Event.current.type == EventType.Repaint)
			{
				var tex = blankTexture;
				var rect = GUILayoutUtility.GetLastRect();
				GUI.color = new Color(0f, 0f, 0f, 0.25f);
				GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
				GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
				GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
				GUI.color = Color.white;
			}
		}

		/// <summary>
		/// Draw a visible thin separator in addition to adding some padding.
		/// </summary>

		static public void DrawThinSeparator()
		{
			GUILayout.Space(4f);

			if (Event.current.type == EventType.Repaint)
			{
				var tex = blankTexture;
				var rect = GUILayoutUtility.GetLastRect();
				GUI.color = new Color(0f, 0f, 0f, 0.25f);
				GUI.DrawTexture(new Rect(0f, rect.yMin, Screen.width, 2f), tex);
				GUI.color = Color.white;
			}
		}

		/// <summary>
		/// Convenience function that displays a list of sprites and returns the selected value.
		/// </summary>

		static public string DrawList(string field, string[] list, string selection, params GUILayoutOption[] options)
		{
			if (list != null && list.Length > 0)
			{
				int index = 0;
				if (string.IsNullOrEmpty(selection)) selection = list[0];

				// We need to find the sprite in order to have it selected
				if (!string.IsNullOrEmpty(selection))
				{
					for (int i = 0; i < list.Length; ++i)
					{
						if (selection.Equals(list[i], System.StringComparison.OrdinalIgnoreCase))
						{
							index = i;
							break;
						}
					}
				}

				// Draw the sprite selection popup
				index = string.IsNullOrEmpty(field) ?
					EditorGUILayout.Popup(index, list, options) :
					EditorGUILayout.Popup(field, index, list, options);

				return list[index];
			}
			return null;
		}

		/// <summary>
		/// Convenience function that displays a list of sprites and returns the selected value.
		/// </summary>

		static public string DrawAdvancedList(string field, string[] list, string selection, params GUILayoutOption[] options)
		{
			if (list != null && list.Length > 0)
			{
				int index = 0;
				if (string.IsNullOrEmpty(selection)) selection = list[0];

				// We need to find the sprite in order to have it selected
				if (!string.IsNullOrEmpty(selection))
				{
					for (int i = 0; i < list.Length; ++i)
					{
						if (selection.Equals(list[i], System.StringComparison.OrdinalIgnoreCase))
						{
							index = i;
							break;
						}
					}
				}

				// Draw the sprite selection popup
				index = string.IsNullOrEmpty(field) ?
					DrawPrefixList(index, list, options) :
					DrawPrefixList(field, index, list, options);

				return list[index];
			}
			return null;
		}




		/// <summary>
		/// Returns 'true' if the specified object is a prefab.
		/// </summary>

		static public bool IsPrefab(GameObject go)
		{
#if UNITY_2018_3_OR_NEWER
			return go != null && PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular;
#else
		return go != null && PrefabUtility.GetPrefabType(go) == PrefabType.Prefab;
#endif
		}

		/// <summary>
		/// Returns 'true' if the specified object is a prefab instance.
		/// </summary>

		static public bool IsPrefabInstance(GameObject go)
		{
#if UNITY_2018_3_OR_NEWER
			return go != null && PrefabUtility.GetPrefabInstanceStatus(go) == PrefabInstanceStatus.Connected;
#else
		return go != null && PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance;
#endif
		}

		/// <summary>
		/// Given a game object, return its prefab (or itself if it's a prefab).
		/// </summary>

		static public GameObject GetPrefab(GameObject go)
		{
			if (go == null) return null;

#if UNITY_2018_3_OR_NEWER
			go = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
			if (go == null) return null;
			return IsPrefab(go) ? go : PrefabUtility.GetCorrespondingObjectFromSource(go);
#else
		go = PrefabUtility.FindPrefabRoot(go);
		if (go == null) return null;
		return PrefabUtility.GetPrefabParent(go) as GameObject;
#endif
		}

		/// <summary>
		/// Helper function that checks to see if this action would break the prefab connection.
		/// </summary>

		static public bool WillLosePrefab(GameObject root)
		{
			if (root == null) return false;

			if (root.transform != null)
			{
				if (IsPrefabInstance(root))
				{
					return EditorUtility.DisplayDialog("Losing prefab",
						"This action will lose the prefab connection. Are you sure you wish to continue?",
						"Continue", "Cancel");
				}
			}
			return true;
		}

		/// <summary>
		/// Change the import settings of the specified texture asset, making it readable.
		/// </summary>

		static public bool MakeTextureReadable(string path, bool force)
		{
			if (string.IsNullOrEmpty(path)) return false;
			var ti = AssetImporter.GetAtPath(path) as TextureImporter;
			if (ti == null) return false;

			var settings = new TextureImporterSettings();
			ti.ReadTextureSettings(settings);

			if (force || !settings.readable || settings.npotScale != TextureImporterNPOTScale.None || ti.textureCompression != TextureImporterCompression.Uncompressed)
			{
				settings.readable = true;

					var platform = ti.GetDefaultPlatformTextureSettings();
					platform.format = TextureImporterFormat.RGBA32;

				settings.npotScale = TextureImporterNPOTScale.None;
				ti.textureCompression = TextureImporterCompression.Uncompressed;
				ti.SetTextureSettings(settings);

#if UNITY_5_6
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
#else
				ti.SaveAndReimport();
#endif
			}
			return true;
		}

		/// <summary>
		/// Change the import settings of the specified texture asset, making it suitable to be used as a texture atlas.
		/// </summary>

		private static bool MakeTextureAnAtlas(string path, bool force)
		{
			if (string.IsNullOrEmpty(path)) return false;
			var ti = AssetImporter.GetAtPath(path) as TextureImporter;
			if (ti == null) return false;

			var settings = new TextureImporterSettings();
			ti.ReadTextureSettings(settings);

			if (force || settings.readable ||
				ti.maxTextureSize < 4096 ||
				(ti.textureCompression != TextureImporterCompression.Uncompressed) ||
				settings.wrapMode != TextureWrapMode.Clamp ||
				settings.npotScale != TextureImporterNPOTScale.ToNearest)
			{
				settings.readable = false;
				ti.maxTextureSize = 4096;
				settings.wrapMode = TextureWrapMode.Clamp;
				settings.npotScale = TextureImporterNPOTScale.ToNearest;

					ti.textureCompression = TextureImporterCompression.Uncompressed;
					settings.filterMode = FilterMode.Trilinear;
			
				settings.aniso = 4;
				settings.alphaIsTransparency = true;
				ti.SetTextureSettings(settings);

#if UNITY_5_6
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
#else
				ti.SaveAndReimport();
#endif
			}
			return true;
		}

		/// <summary>
		/// Fix the import settings for the specified texture, re-importing it if necessary.
		/// </summary>

		static public Texture2D ImportTexture(string path, bool forInput, bool force)
		{
			if (!string.IsNullOrEmpty(path))
			{
				if (forInput) { if (!MakeTextureReadable(path, force)) return null; }
				else if (!MakeTextureAnAtlas(path, force)) return null;
				//return AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;

				var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
				AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
				return tex;
			}
			return null;
		}

		/// <summary>
		/// Fix the import settings for the specified texture, re-importing it if necessary.
		/// </summary>

		static public Texture2D ImportTexture(Texture tex, bool forInput, bool force)
		{
			if (tex != null)
			{
				var path = AssetDatabase.GetAssetPath(tex.GetInstanceID());
				return ImportTexture(path, forInput, force);
			}
			return null;
		}


		/// <summary>
		/// Helper function that returns the folder where the current selection resides.
		/// </summary>

		static public string GetSelectionFolder()
		{
			if (Selection.activeObject != null)
			{
				string path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());

				if (!string.IsNullOrEmpty(path))
				{
					int dot = path.LastIndexOf('.');
					int slash = Mathf.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
					if (slash > 0) return (dot > slash) ? path.Substring(0, slash + 1) : path + "/";
				}
			}
			return "Assets/";
		}

		/// <summary>
		/// Struct type for the integer vector field below.
		/// </summary>

		public struct IntVector
		{
			public int x;
			public int y;
		}

		/// <summary>
		/// Integer vector field.
		/// </summary>

		static public IntVector IntPair(string prefix, string leftCaption, string rightCaption, int x, int y)
		{
			GUILayout.BeginHorizontal();

			if (string.IsNullOrEmpty(prefix))
			{
				GUILayout.Space(82f);
			}
			else
			{
				GUILayout.Label(prefix, GUILayout.Width(74f));
			}

			NGUIEditorTools.SetLabelWidth(48f);

			IntVector retVal;
			retVal.x = EditorGUILayout.IntField(leftCaption, x, GUILayout.MinWidth(30f));
			retVal.y = EditorGUILayout.IntField(rightCaption, y, GUILayout.MinWidth(30f));

			NGUIEditorTools.SetLabelWidth(80f);

			GUILayout.EndHorizontal();
			return retVal;
		}

		/// <summary>
		/// Integer rectangle field.
		/// </summary>

		static public Rect IntRect(string prefix, Rect rect)
		{
			int left = Mathf.RoundToInt(rect.xMin);
			int top = Mathf.RoundToInt(rect.yMin);
			int width = Mathf.RoundToInt(rect.width);
			int height = Mathf.RoundToInt(rect.height);

			NGUIEditorTools.IntVector a = NGUIEditorTools.IntPair(prefix, "Left", "Top", left, top);
			NGUIEditorTools.IntVector b = NGUIEditorTools.IntPair(null, "Width", "Height", width, height);

			return new Rect(a.x, a.y, b.x, b.y);
		}

		/// <summary>
		/// Integer vector field.
		/// </summary>

		static public Vector4 IntPadding(string prefix, Vector4 v)
		{
			int left = Mathf.RoundToInt(v.x);
			int top = Mathf.RoundToInt(v.y);
			int right = Mathf.RoundToInt(v.z);
			int bottom = Mathf.RoundToInt(v.w);

			NGUIEditorTools.IntVector a = NGUIEditorTools.IntPair(prefix, "Left", "Top", left, top);
			NGUIEditorTools.IntVector b = NGUIEditorTools.IntPair(null, "Right", "Bottom", right, bottom);

			return new Vector4(a.x, a.y, b.x, b.y);
		}

		/// <summary>
		/// Find all scene components, active or inactive.
		/// </summary>

		static public System.Collections.Generic.List<T> FindAll<T>() where T : Component
		{
			T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

			System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();

			foreach (T comp in comps)
			{
				if (comp.gameObject.hideFlags == 0)
				{
					string path = AssetDatabase.GetAssetPath(comp.gameObject);
					if (string.IsNullOrEmpty(path)) list.Add(comp);
				}
			}
			return list;
		}

		static public bool DrawPrefixButton(string text)
		{
			return GUILayout.Button(text, "DropDown", GUILayout.Width(76f));
		}

		static public bool DrawPrefixButton(string text, params GUILayoutOption[] options)
		{
			return GUILayout.Button(text, "DropDown", options);
		}

		static public int DrawPrefixList(int index, string[] list, params GUILayoutOption[] options)
		{
			return EditorGUILayout.Popup(index, list, "DropDown", options);
		}

		static public int DrawPrefixList(string text, int index, string[] list, params GUILayoutOption[] options)
		{
			return EditorGUILayout.Popup(text, index, list, "DropDown", options);
		}


		/// Helper function that checks to see if the scale is uniform.
		static public bool IsUniform(Vector3 scale)
		{
			return Mathf.Approximately(scale.x, scale.y) && Mathf.Approximately(scale.x, scale.z);
		}

		/// <summary>
		/// Check to see if the specified game object has a uniform scale.
		/// </summary>

		static public bool IsUniform(GameObject go)
		{
			if (go == null) return true;

			if (go.GetComponent<UIDrawCallMaker>() != null)
			{
				Transform parent = go.transform.parent;
				return parent == null || IsUniform(parent.gameObject);
			}
			return IsUniform(go.transform.lossyScale);
		}

		// Fix uniform scaling of the specified object.

		static public void FixUniform(GameObject go)
		{
			Transform t = go.transform;

			while (t != null && t.gameObject.GetComponent<NUICanvas>() == null)
			{
				if (!NGUIEditorTools.IsUniform(t.localScale))
				{
					NGUIEditorTools.UndoRecordObject("Uniform scaling fix", t);
					t.localScale = Vector3.one;
					NGUITools.EditorSetDirty(t);
				}
				t = t.parent;
			}
		}


		public static bool UsingHeaderIndicator = true;

		private static string GetSectionHeaderIndicator(bool isExpandHeader)
		{
			if (isExpandHeader)
				return "\u25BC ";
			else
				return "\u25BA ";
		}

		public static bool DrawSectionHeader(string headerName, GUIStyle style, bool isExpandHeader)
		{
			if (!isExpandHeader) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);

			if (style.richText)
				headerName = "<b><size=11>" + headerName + "</size></b>";

			if (UsingHeaderIndicator)
				headerName = GetSectionHeaderIndicator(isExpandHeader) + headerName;

			isExpandHeader = GUILayout.Toggle(isExpandHeader, headerName, style, GUILayout.MinWidth(20f));


			GUI.backgroundColor = Color.white;
			return isExpandHeader;
		}


		public static bool DrawSectionHeader(string headerName)
		{
			GUIStyle style = "button";
			{
				style.richText = true;
				style.alignment = TextAnchor.MiddleLeft;
			}

			return DrawSectionHeader(headerName, style);
		}
		public static bool DrawSectionHeader(string headerName, bool isExpandHeader)
		{
			GUIStyle style = "button";
			{
				style.richText = true;
				style.alignment = TextAnchor.MiddleLeft;
			}

			isExpandHeader = DrawSectionHeader(headerName, style, isExpandHeader);

			return isExpandHeader;
		}
		public static bool DrawSectionHeader(string headerName, GUIStyle style)
		{
			string expandHeaderStr = $"IsExpandHeader{headerName}";
			bool oldIsExpandHeader = EditorPrefs.GetBool(expandHeaderStr);

			var newIsExpandHeader = DrawSectionHeader(headerName, style, oldIsExpandHeader);

			if (newIsExpandHeader != oldIsExpandHeader)
				EditorPrefs.SetBool(expandHeaderStr, newIsExpandHeader);

			return newIsExpandHeader;
		}

		// Begin drawing the content area.
		static public void BeginContents()
		{
			GUILayout.BeginVertical(GUI.skin.textArea);
		}

		// End drawing the content area.
		static public void EndContents()
		{
			GUILayout.EndVertical();

			GUILayout.Space(3f);
		}


		// Helper function that draws a serialized property.
		static public SerializedProperty DrawProperty(this SerializedObject serializedObject, string property, params GUILayoutOption[] options)
		{
			return DrawProperty(null, serializedObject, property, false, options);
		}

		// Helper function that draws a serialized property.
		static public SerializedProperty DrawProperty(this SerializedObject serializedObject, string property, string label, params GUILayoutOption[] options)
		{
			return DrawProperty(label, serializedObject, property, false, options);
		}

		// Helper function that draws a serialized property.
		static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
		{
			return DrawProperty(label, serializedObject, property, false, options);
		}

		// Helper function that draws a serialized property.
		static public SerializedProperty DrawPaddedProperty(this SerializedObject serializedObject, string property, params GUILayoutOption[] options)
		{
			return DrawProperty(null, serializedObject, property, true, options);
		}

		// Helper function that draws a serialized property.
		static public SerializedProperty DrawPaddedProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
		{
			return DrawProperty(label, serializedObject, property, true, options);
		}

		// Helper function that draws a serialized property.
		static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
		{
			SerializedProperty sp = serializedObject.FindProperty(property);

			if (sp != null)
			{
				if (padding) EditorGUILayout.BeginHorizontal();

				if (sp.isArray && sp.type != "string") DrawArray(serializedObject, property, label ?? property);
				else if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
				else EditorGUILayout.PropertyField(sp, options);

				if (padding)
				{
					NGUIEditorTools.DrawPadding();
					EditorGUILayout.EndHorizontal();
				}
			}
			else Debug.LogWarning("Unable to find property " + property);
			return sp;
		}

		// Helper function that draws an array property.
		static public void DrawArray(this SerializedObject obj, string property, string title)
		{
			SerializedProperty sp = obj.FindProperty(property + ".Array.size");

			if (sp != null && NGUIEditorTools.DrawSectionHeader(title))
			{
				NGUIEditorTools.BeginContents();
				int size = sp.intValue;
				int newSize = EditorGUILayout.IntField("Size", size);
				if (newSize != size) obj.FindProperty(property + ".Array.size").intValue = newSize;

				EditorGUI.indentLevel = 1;

				for (int i = 0; i < newSize; i++)
				{
					SerializedProperty p = obj.FindProperty(string.Format("{0}.Array.data[{1}]", property, i));
					if (p != null) EditorGUILayout.PropertyField(p);
				}
				EditorGUI.indentLevel = 0;
				NGUIEditorTools.EndContents();
			}
		}

		// Helper function that draws a serialized property.
		static public void DrawProperty(string label, SerializedProperty sp, params GUILayoutOption[] options)
		{
			DrawProperty(label, sp, true, options);
		}

		// Helper function that draws a serialized property.
		static public void DrawProperty(string label, SerializedProperty sp, bool padding, params GUILayoutOption[] options)
		{
			if (sp != null)
			{
				if (padding) EditorGUILayout.BeginHorizontal();

				if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
				else EditorGUILayout.PropertyField(sp, options);

				if (padding)
				{
					NGUIEditorTools.DrawPadding();
					EditorGUILayout.EndHorizontal();
				}
			}
		}

		// Helper function that draws a compact Vector4.
		static public void DrawBorderProperty(string name, SerializedObject serializedObject, string field)
		{
			if (serializedObject.FindProperty(field) != null)
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(name, GUILayout.Width(75f));

					NGUIEditorTools.SetLabelWidth(50f);
					GUILayout.BeginVertical();
					NGUIEditorTools.DrawProperty("Left", serializedObject, field + ".x", GUILayout.MinWidth(80f));
					NGUIEditorTools.DrawProperty("Bottom", serializedObject, field + ".y", GUILayout.MinWidth(80f));
					GUILayout.EndVertical();

					GUILayout.BeginVertical();
					NGUIEditorTools.DrawProperty("Right", serializedObject, field + ".z", GUILayout.MinWidth(80f));
					NGUIEditorTools.DrawProperty("Top", serializedObject, field + ".w", GUILayout.MinWidth(80f));
					GUILayout.EndVertical();

					NGUIEditorTools.SetLabelWidth(80f);
				}
				GUILayout.EndHorizontal();
			}
		}

		// Helper function that draws a compact Rect.
		static public void DrawRectProperty(string name, SerializedObject serializedObject, string field)
		{
			DrawRectProperty(name, serializedObject, field, 56f, 18f);
		}

		// Helper function that draws a compact Rect.
		static public void DrawRectProperty(string name, SerializedObject serializedObject, string field, float labelWidth, float spacing)
		{
			if (serializedObject.FindProperty(field) != null)
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(name, GUILayout.Width(labelWidth));

					NGUIEditorTools.SetLabelWidth(20f);
					GUILayout.BeginVertical();
					NGUIEditorTools.DrawProperty("X", serializedObject, field + ".x", GUILayout.MinWidth(50f));
					NGUIEditorTools.DrawProperty("Y", serializedObject, field + ".y", GUILayout.MinWidth(50f));
					GUILayout.EndVertical();

					NGUIEditorTools.SetLabelWidth(50f);
					GUILayout.BeginVertical();
					NGUIEditorTools.DrawProperty("Width", serializedObject, field + ".width", GUILayout.MinWidth(80f));
					NGUIEditorTools.DrawProperty("Height", serializedObject, field + ".height", GUILayout.MinWidth(80f));
					GUILayout.EndVertical();

					NGUIEditorTools.SetLabelWidth(80f);
					if (spacing != 0f) GUILayout.Space(spacing);
				}
				GUILayout.EndHorizontal();
			}
		}



		static public void SetLabelWidth(float width)
		{
			EditorGUIUtility.labelWidth = width;
		}

		static public void UndoRecordObject(string name, Object obj)
		{
			if (obj != null)
				UnityEditor.Undo.RecordObject(obj, name);
		}

		static public void UndoRecordObject(string name, params Object[] objects)
		{
			if (objects != null && objects.Length > 0)
				UnityEditor.Undo.RecordObjects(objects, name);
		}


		/// <summary>
		/// Gets the internal class ID of the specified type.
		/// </summary>

		static public int GetClassID(System.Type type)
		{
			var go = EditorUtility.CreateGameObjectWithHideFlags("Temp", HideFlags.HideAndDontSave);
			var uiSprite = go.AddComponent(type);
			var ob = new SerializedObject(uiSprite);
			var classID = ob.FindProperty("m_Script").objectReferenceInstanceIDValue;
			NGUITools.DestroyImmediate(go);
			return classID;
		}

		/// <summary>
		/// Gets the internal class ID of the specified type.
		/// </summary>

		static public int GetClassID<T>() where T : MonoBehaviour
		{
			return GetClassID(typeof(T));
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified type.
		/// </summary>

		static public SerializedObject ReplaceClass(MonoBehaviour mb, System.Type type)
		{
			var id = GetClassID(type);
			var ob = new SerializedObject(mb);
			ob.Update();
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = id;
			ob.ApplyModifiedProperties();
			ob.Update();
			return ob;
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
		/// </summary>

		static public SerializedObject ReplaceClass(MonoBehaviour mb, int classID)
		{
			var ob = new SerializedObject(mb);
			ob.Update();
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = classID;
			ob.ApplyModifiedProperties();
			ob.Update();
			return ob;
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
		/// </summary>

		static public void ReplaceClass(SerializedObject ob, int classID)
		{
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = classID;
			ob.ApplyModifiedProperties();
			ob.Update();
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified class ID.
		/// </summary>

		static public void ReplaceClass(SerializedObject ob, System.Type type)
		{
			ob.FindProperty("m_Script").objectReferenceInstanceIDValue = GetClassID(type);
			ob.ApplyModifiedProperties();
			ob.Update();
		}

		/// <summary>
		/// Convenience function that replaces the specified MonoBehaviour with one of specified type.
		/// </summary>

		static public T ReplaceClass<T>(MonoBehaviour mb) where T : MonoBehaviour
		{
			return ReplaceClass(mb, typeof(T)).targetObject as T;
		}

		private class MenuEntry
		{
			public string name;
			public GameObject go;

			public MenuEntry(string name, GameObject go)
			{
				this.name = name; this.go = go;
			}
		}


		static public Object LoadAsset(string path)
		{
			if (string.IsNullOrEmpty(path)) return null;
			return AssetDatabase.LoadMainAssetAtPath(path);
		}

		/// <summary>
		/// Convenience function to load an asset of specified type, given the full path to it.
		/// </summary>

		static public T LoadAsset<T>(string path) where T : Object
		{
			Object obj = LoadAsset(path);
			if (obj == null) return null;

			T val = obj as T;
			if (val != null) return val;

			if (typeof(T).IsSubclassOf(typeof(Component)))
			{
				if (obj.GetType() == typeof(GameObject))
				{
					GameObject go = obj as GameObject;
					return go.GetComponent(typeof(T)) as T;
				}
			}
			return null;
		}

		/// <summary>
		/// Get the specified object's GUID.
		/// </summary>

		static public string ObjectToGUID(Object obj)
		{
			string path = AssetDatabase.GetAssetPath(obj);
			return (!string.IsNullOrEmpty(path)) ? AssetDatabase.AssetPathToGUID(path) : null;
		}

		private static MethodInfo s_GetInstanceIDFromGUID;

		/// <summary>
		/// Convert the specified GUID to an object reference.
		/// </summary>

		static public Object GUIDToObject(string guid)
		{
			if (string.IsNullOrEmpty(guid)) return null;

			if (s_GetInstanceIDFromGUID == null)
			{
				var type = typeof(AssetDatabase);

				// Unity 3, 4, 5 and 2017
				s_GetInstanceIDFromGUID = type.GetMethod("GetInstanceIDFromGUID", BindingFlags.Static | BindingFlags.NonPublic);

				// Unity 2018+
				if (s_GetInstanceIDFromGUID == null) s_GetInstanceIDFromGUID = type.GetMethod("GetMainAssetInstanceID", BindingFlags.Static | BindingFlags.NonPublic);
				if (s_GetInstanceIDFromGUID == null) return null;
			}

			int id = (int)s_GetInstanceIDFromGUID.Invoke(null, new object[] { guid });
			if (id != 0) return EditorUtility.InstanceIDToObject(id);
			string path = AssetDatabase.GUIDToAssetPath(guid);
			if (string.IsNullOrEmpty(path)) return null;
			return AssetDatabase.LoadAssetAtPath(path, typeof(Object));
		}

		/// <summary>
		/// Convert the specified GUID to an object reference of specified type.
		/// </summary>

		static public T GUIDToObject<T>(string guid) where T : Object
		{
			Object obj = GUIDToObject(guid);
			if (obj == null) return null;

			System.Type objType = obj.GetType();
			if (objType == typeof(T) || objType.IsSubclassOf(typeof(T))) return obj as T;

			if (objType == typeof(GameObject) && typeof(T).IsSubclassOf(typeof(Component)))
			{
				GameObject go = obj as GameObject;
				return go.GetComponent(typeof(T)) as T;
			}
			return null;
		}

		/// <summary>
		/// Add a border around the specified color buffer with the width and height of a single pixel all around.
		/// The returned color buffer will have its width and height increased by 2.
		/// </summary>

		static public Color32[] AddBorder(Color32[] colors, int width, int height)
		{
			int w2 = width + 2;
			int h2 = height + 2;

			Color32[] c2 = new Color32[w2 * h2];

			for (int y2 = 0; y2 < h2; ++y2)
			{
				int y1 = NGUIMath.ClampIndex(y2 - 1, height);

				for (int x2 = 0; x2 < w2; ++x2)
				{
					int x1 = NGUIMath.ClampIndex(x2 - 1, width);
					int i2 = x2 + y2 * w2;
					c2[i2] = colors[x1 + y1 * width];

					if (x2 == 0 || x2 + 1 == w2 || y2 == 0 || y2 + 1 == h2)
						c2[i2].a = 0;
				}
			}
			return c2;
		}


		/// <summary>
		/// Draw 18 pixel padding on the right-hand side. Used to align fields.
		/// </summary>

		static public void DrawPadding()
		{
			GUILayout.Space(18f);
		}

		private static System.Collections.Generic.Dictionary<string, TextureImporterType> mOriginal = new Dictionary<string, TextureImporterType>();

		/// <summary>
		/// Force the texture to be readable. Returns the asset database path to the texture.
		/// </summary>

		static public string MakeReadable(this Texture2D tex, bool readable = true)
		{
			var path = AssetDatabase.GetAssetPath(tex);
#if UNITY_5_6
		if (!string.IsNullOrEmpty(path))
#else
			if (!string.IsNullOrEmpty(path) && !tex.isReadable)
#endif
			{
				var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

				if (textureImporter != null && textureImporter.isReadable != readable)
				{
					textureImporter.isReadable = readable;

					if (readable)
					{
						mOriginal[path] = textureImporter.textureType;
#if UNITY_5_5_OR_NEWER
						textureImporter.textureType = TextureImporterType.Default;
#else
					textureImporter.textureType = TextureImporterType.Image;
#endif
					}
					else
					{
						TextureImporterType type;

						if (mOriginal.TryGetValue(path, out type))
						{
							textureImporter.textureType = type;
							mOriginal.Remove(path);
						}
					}
					AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
				}
			}
			return path;
		}
	}
}