using System;
using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{
    public abstract class NGUIEditor : Editor
    {

        public bool IsRoot { get; set; }

        private Component targetComponent;

        protected virtual void OnEnable()
        {
            targetComponent = target as Component;
            IsRoot = targetComponent.GetComponent<NUICanvas>() != null;
        }


        protected void DrawGroupDisabledIfRoot(Action drawAction)
        {
            EditorGUI.BeginDisabledGroup(IsRoot);
            drawAction();
            EditorGUI.EndDisabledGroup();
            if (IsRoot)
            {
                EditorGUILayout.HelpBox($"Data is driven by {nameof(NUICanvas)}", MessageType.None);
            }
        }

        public override sealed void OnInspectorGUI()
        {
            using (new SerializedObjHelper(this.serializedObject))
            {
                OnInspectorNGUI();
            }
        }

        public abstract void OnInspectorNGUI();


        // Raycast into the screen.
        public static bool RaycastMousePlane(Vector3[] corners, out Vector3 hit)
        {
            Plane plane = new Plane(corners[0], corners[1], corners[2]);
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            float dist = 0f;
            bool isHit = plane.Raycast(ray, out dist);
            hit = isHit ? ray.GetPoint(dist) : Vector3.zero;
            return isHit;
        }


        internal class SerializedObjHelper : IDisposable
        {
            private SerializedObject serializedObject;

            public SerializedObjHelper(SerializedObject serializedObject)
            {
                this.serializedObject = serializedObject;
                serializedObject.Update();
            }

            public void Dispose()
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}