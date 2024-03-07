//-------------------------------------------------
//			  NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform), true)]
    public partial class NGUITransformInspector : NGUIEditor
    {
        private Transform targetTransform;
        private SerializedProperty serialPropertyLocalPosition;
        private SerializedProperty serialPropertyLocalRotation;
        private SerializedProperty serialPropertyLocalScale;

        public GUIContent positionContent = EditorGUIUtility.TrTextContent("Position", "The local position of this GameObject relative to the parent.");
        public GUIContent rotationContent = EditorGUIUtility.TrTextContent("Rotation", "The local rotation of this GameObject relative to the parent.");
        public GUIContent scaleContent = EditorGUIUtility.TrTextContent("Scale", "The local scaling of this GameObject relative to the parent.");

        public string floatingPointWarning = ("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");

        private Editor transformInspector;

        private UIRect uiRect;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetTransform = target as Transform;
            uiRect = targetTransform.GetComponent<UIRect>();

            serialPropertyLocalPosition = serializedObject.FindProperty("m_LocalPosition");
            serialPropertyLocalRotation = serializedObject.FindProperty("m_LocalRotation");
            serialPropertyLocalScale = serializedObject.FindProperty("m_LocalScale");
        }

        private void OnDisable()
        {
            DestroyImmediate(transformInspector);
        }

        public override void OnInspectorNGUI()
        {
            if (TryUsingTransformInspector())
                return;

            // Using wideMode, Vector3Field will not stretch to 2 lines
            if (!EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = true;
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212f;
            }

            DrawGroupDisabledIfRoot(() =>
            {
                DrawPosition();
                DrawRotation();
                DrawScale();
            });

        }



        ///<see cref="UnityEditor.TransformInspector"/>
        private bool TryUsingTransformInspector()
        {
            if (uiRect != null)
                return false;

            if (targetTransform.GetComponent<Transform>() == null)
                return false;

            if (transformInspector == null)
            {
                var defaultType = typeof(Editor).Assembly.GetType("UnityEditor.TransformInspector");
                transformInspector = Editor.CreateEditor(this.target, defaultType);
            }
            if (transformInspector != null)
                transformInspector.OnInspectorGUI();

            return true;
        }

        private void ShowAlertIfAny()
        {
            var position = targetTransform.position;
            if (Mathf.Abs(position.x) > 100000f || Mathf.Abs(position.y) > 100000f || Mathf.Abs(position.z) > 100000f)
            {
                EditorGUILayout.HelpBox(floatingPointWarning, MessageType.Warning);
            }
        }

        private void DrawPosition()
        {
            EditorGUI.BeginDisabledGroup(uiRect.useAnchor);
            using (NGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(serialPropertyLocalPosition, positionContent);
                var reset = GUILayout.Button("0", GUILayout.Width(20f));
                if (reset) serialPropertyLocalPosition.vector3Value = Vector3.zero;
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawScale()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serialPropertyLocalScale, scaleContent);
            var reset = GUILayout.Button("1", GUILayout.Width(20f));
            if (reset) serialPropertyLocalScale.vector3Value = Vector3.one;
            GUILayout.EndHorizontal();
        }

        private void DrawRotation()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serialPropertyLocalRotation, rotationContent);
            var reset = GUILayout.Button("0", GUILayout.Width(20f));
            if (reset) serialPropertyLocalRotation.quaternionValue = Quaternion.identity;
            GUILayout.EndHorizontal();
        }
    }
}