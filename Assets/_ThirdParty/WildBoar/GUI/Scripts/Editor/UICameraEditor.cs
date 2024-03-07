//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using UnityEditor;

namespace WildBoar.GUIModule
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EventSystem))]
	public class UICameraEditor : Editor
	{
		enum EventsGo
		{
			Colliders,
			Rigidbodies,
		}

		public override void OnInspectorGUI()
		{
			EventSystem cam = target as EventSystem;
			GUILayout.Space(3f);

			serializedObject.Update();


			//if (UICamera.eventHandler != cam)
			//{
			//	EditorGUILayout.PropertyField(serializedObject.FindProperty("eventReceiverMask"), new GUIContent("Event Mask"));
			//	serializedObject.ApplyModifiedProperties();

			//	EditorGUILayout.HelpBox("All other settings are inherited from the First Camera.", MessageType.Info);

			//	if (GUILayout.Button("Select the First Camera"))
			//	{
			//		Selection.activeGameObject = UICamera.eventHandler.gameObject;
			//	}
			//}
			//else
			{


				NGUIEditorTools.SetLabelWidth(80f);


				if ( NGUIEditorTools.DrawSectionHeader("Thresholds"))
				{
					NGUIEditorTools.BeginContents();
					{
						GUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(serializedObject.FindProperty("mouseDragThreshold"), new GUIContent("Mouse Drag"), GUILayout.Width(120f));
						GUILayout.Label("pixels");
						GUILayout.EndHorizontal();

						GUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(serializedObject.FindProperty("mouseClickThreshold"), new GUIContent("Mouse Click"), GUILayout.Width(120f));
						GUILayout.Label("pixels");
						GUILayout.EndHorizontal();

					}
					NGUIEditorTools.EndContents();
				}

				if (NGUIEditorTools.DrawSectionHeader("Axes and Keys"))
				{
					NGUIEditorTools.BeginContents();
					{
						EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalAxisName"), new GUIContent("Navigate X"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalAxisName"), new GUIContent("Navigate Y"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalPanAxisName"), new GUIContent("Pan X"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalPanAxisName"), new GUIContent("Pan Y"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("scrollAxisName"), new GUIContent("Scroll"));
					}
					NGUIEditorTools.EndContents();
				}
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
