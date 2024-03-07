using System;
using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{
    public partial class UIWidgetEditor
    {
        //UI position
        public Vector2 DragStartPos { get; set; }

        public Vector2 WidgetPos { get; set; }
        public Vector2 WidgetSize { get; set; }

        private int ControlID { get; set; }

        private static int s_Hash = "WidgetHash".GetHashCode();

        //TODO merge widget's OnSceneGUI and Panel's OnSceneGUI
        // Draw the on-screen selection, knobs, and handle all interaction logic.
        protected virtual void OnSceneGUI()
        {
            if (Tools.current != UnityEditor.Tool.Rect) return;

            ControlID = GUIUtility.GetControlID(s_Hash, FocusType.Passive);

        }

        private void SelectDrawCall()
        {
            bool selectionContainsDrawCall = false;
            foreach (var go in Selection.objects)
            {
                if (go == targetWidget.DrawCall.gameObject)
                    selectionContainsDrawCall = true;
            }
            if (selectionContainsDrawCall)
                return;

            var count = Selection.objects.Length;
            GameObject[] newSelection = new GameObject[count + 1];
            Array.Copy(Selection.objects, newSelection, count);

            newSelection[count] = targetWidget.DrawCall.gameObject;

            Selection.objects = newSelection;
        }

    }
}