//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace WildBoar.GUIModule
{
	// This script adds the NGUI menu options to the Unity Editor.
	static public class NGUIMenu
	{
		#region Selection


		[MenuItem("NGUI/Selection/Bring To Front &#=", true)]
		static public bool BringForward2Validation() { return (Selection.activeGameObject != null); }

		[MenuItem("NGUI/Selection/Adjust Depth By -1 %-", true)]
		static public bool PushBackValidation() { return (Selection.activeGameObject != null); }

		[MenuItem("NGUI/Selection/Make Pixel Perfect &#p", true)]
		static bool PixelPerfectSelectionValidation() { return (Selection.activeTransform != null); }

		[MenuItem("NGUI/Selection/Check for issues", true)]
		static bool CheckForIssuesValidation() { return (Selection.activeTransform != null); }

		#endregion
		#region Create



		[MenuItem("NGUI/Create/", false, 6)]
		static void AddBreaker123() { }



		#endregion
		#region Attach

		static void AddIfMissing<T>() where T : Component
		{
			if (Selection.activeGameObject != null)
			{
				for (int i = 0; i < Selection.gameObjects.Length; ++i)
					Selection.gameObjects[i].AddMissingComponent<T>();
			}
			else Debug.Log("You must select a game object first.");
		}

		static bool Exists<T>() where T : Component
		{
			GameObject go = Selection.activeGameObject;
			if (go != null) return go.GetComponent<T>() != null;
			return false;
		}


		#endregion
		#region Open


		[MenuItem("NGUI/Open/Camera Tool", false, 9)]
		static public void OpenCameraWizard()
		{
			EditorWindow.GetWindow<UICameraTool>(false, "Camera Tool", true).Show();
		}


		#endregion
	



	}
}
