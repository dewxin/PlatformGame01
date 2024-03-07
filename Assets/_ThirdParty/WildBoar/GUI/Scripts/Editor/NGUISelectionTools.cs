//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace WildBoar.GUIModule
{
	public class NGUISelectionTools
	{
		[MenuItem("GameObject/Selection/Force Delete")]
		static void ForceDelete()
		{
			Object[] gos = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);

			if (gos != null && gos.Length > 0)
			{
				for (int i = 0; i < gos.Length; ++i)
				{
					Object go = gos[i];
					NGUITools.DestroyImmediate(go);
				}
			}
		}

		[MenuItem("GameObject/Selection/Clear Local Transform")]
		static void ClearLocalTransform()
		{
			if (HasValidTransform())
			{
				Transform t = Selection.activeTransform;
				NGUIEditorTools.UndoRecordObject("Clear Local Transform", t);
				t.localPosition = Vector3.zero;
				t.localRotation = Quaternion.identity;
				t.localScale = Vector3.one;
			}
		}

		[MenuItem("GameObject/Selection/List Dependencies")]
		static void ListDependencies()
		{
			if (HasValidSelection())
			{
				Debug.Log("Selection depends on the following assets:\n\n" + GetDependencyText(Selection.objects, false));
			}
		}

		//========================================================================================================

		#region Helper Functions

		class AssetEntry
		{
			public string path;
			public List<System.Type> types = new List<System.Type>();
		}

		/// <summary>
		/// Helper function that checks to see if there are objects selected.
		/// </summary>

		static bool HasValidSelection()
		{
			if (Selection.objects == null || Selection.objects.Length == 0)
			{
				Debug.LogWarning("You must select an object first");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Helper function that checks to see if there is an object with a Transform component selected.
		/// </summary>

		static bool HasValidTransform()
		{
			if (Selection.activeTransform == null)
			{
				Debug.LogWarning("You must select an object first");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Helper function that checks to see if a prefab is currently selected.
		/// </summary>

		static bool PrefabCheck()
		{
			if (Selection.activeTransform != null)
			{
				// Check if the selected object is a prefab instance and display a warning
				if (NGUIEditorTools.IsPrefabInstance(Selection.activeGameObject))
				{
					return EditorUtility.DisplayDialog("Losing prefab",
						"This action will lose the prefab connection. Are you sure you wish to continue?",
						"Continue", "Cancel");
				}
			}
			return true;
		}

		// Function that collects a list of file dependencies from the specified list of objects.
		static List<AssetEntry> GetDependencyList(Object[] objects, bool reverse)
		{
			Object[] deps = reverse ? EditorUtility.CollectDeepHierarchy(objects) : EditorUtility.CollectDependencies(objects);

			List<AssetEntry> assetList = new List<AssetEntry>();

			foreach (Object obj in deps)
			{
				string path = AssetDatabase.GetAssetPath(obj);

				if (!string.IsNullOrEmpty(path))
				{
					var found = false;
					var type = obj.GetType();

					foreach (var assetEntry in assetList)
					{
						if (assetEntry.path.Equals(path))
						{
							if (!assetEntry.types.Contains(type))
								assetEntry.types.Add(type);

							found = true;
							break;
						}
					}

					if (!found)
					{
						var ent = new AssetEntry();
						ent.path = path;
						ent.types.Add(type);
						assetList.Add(ent);
					}
				}
			}

			deps = null;
			objects = null;
			return assetList;
		}

		/// <summary>
		/// Helper function that removes the Unity class prefix from the specified string.
		/// </summary>

		static string RemovePrefix(string text)
		{
			text = text.Replace("UnityEngine.", "");
			text = text.Replace("UnityEditor.", "");
			return text;
		}

		/// <summary>
		/// Helper function that gets the dependencies of specified objects and returns them in text format.
		/// </summary>

		static string GetDependencyText(Object[] objects, bool reverse)
		{
			List<AssetEntry> dependencies = GetDependencyList(objects, reverse);
			List<string> list = new List<string>();
			string text = "";

			foreach (AssetEntry ae in dependencies)
			{
				text = ae.path.Replace("Assets/", "");

				if (ae.types.Count > 1)
				{
					text += " (" + RemovePrefix(ae.types[0].ToString());

					for (int i = 1; i < ae.types.Count; ++i)
					{
						text += ", " + RemovePrefix(ae.types[i].ToString());
					}

					text += ")";
				}
				list.Add(text);
			}

			list.Sort();

			text = "";
			foreach (string s in list) text += s + "\n";
			list.Clear();
			list = null;

			dependencies.Clear();
			dependencies = null;
			return text;
		}
		#endregion
	}
}
