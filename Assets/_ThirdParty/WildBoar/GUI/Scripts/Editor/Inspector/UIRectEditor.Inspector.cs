//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2023 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIRect), true)]
    public partial class UIRectEditor :NGUIEditor 
    {
        protected UIRect targetRect;

        protected override void OnEnable()
        {
            base.OnEnable();
            targetRect = target as UIRect;


            UnityEditorInternal.ComponentUtility.MoveComponentUp(targetRect);
        }

        protected virtual void OnDisable()
        {
        }

        public override void OnInspectorNGUI()
        {
            //EditorGUIUtility.labelWidth = 80f;
            EditorGUIUtility.wideMode = true;
            EditorGUILayout.Space();

            DrawParent();

            var newReceiveRaycast = EditorGUILayout.Toggle($"{nameof(targetRect.receiveRaycast)}",targetRect.receiveRaycast);
            if(newReceiveRaycast!=targetRect.receiveRaycast)
            {
                Undo.RecordObject(targetRect, "undo");
                targetRect.receiveRaycast = newReceiveRaycast;
            }

            DrawGroupDisabledIfRoot(() =>
            {
                EditorGUILayout.Space();
                DrawSize();
                DrawPivot();
                DrawSortingOrder();
                EditorGUILayout.Space();
                DrawRectAnchor();
            });

        }

        protected void DrawParent()
        {
            if (targetRect.transform.parent == null)
                return;

            EditorGUI.BeginDisabledGroup(true);

            var findParent = TryFindParentRect(out var parentRect);
            EditorGUILayout.ObjectField("Parent", parentRect, typeof(GameObject), false);

            EditorGUI.EndDisabledGroup();
        }

        private bool TryFindParentRect(out GameObject parentGameObj)
        {
            var uiRect = targetRect.transform.parent.GetComponentInParent<UIRect>();

            parentGameObj = null;
            if(uiRect != null)
                parentGameObj = uiRect.gameObject;

            return parentGameObj != null;
        }

        public void DrawRectAnchor()
        {
            var toggleStyle = GUI.skin.toggle;
            toggleStyle.richText = false;

            NGUIEditorTools.UsingHeaderIndicator = false;
            EditorGUI.BeginChangeCheck();
            bool useAnchor = NGUIEditorTools.DrawSectionHeader("Use Anchors", toggleStyle);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetRect, "RectChange");
                targetRect.useAnchor = useAnchor;
            }

            if (useAnchor)
            {
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;
                DrawMinAnchor();
                DrawMaxAnchor();
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
            NGUIEditorTools.UsingHeaderIndicator = true;
        }

        public void DrawSortingOrder()
        {
            using (NGUILayout.HorizontalScope())
            {
                EditorGUI.BeginChangeCheck();
                var order = EditorGUILayout.IntField("Sort Order", targetRect.SortingOrder);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(targetRect, "RectChange");
                    targetRect.SortingOrder = order;
                }


                if(GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    Undo.RecordObject(targetRect, "RectChange");
                    targetRect.SortingOrder -= 1;
                }
                if(GUILayout.Button("+", GUILayout.Width(20f)))
                {
                    Undo.RecordObject(targetRect, "RectChange");
                    targetRect.SortingOrder += 1;
                }

            }


        }

        public void DrawPivot()
        {
            EditorGUI.BeginChangeCheck();
            var pivot = EditorGUILayout.Vector2Field("Pivot", targetRect.RenderTargetPivot);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetRect, "RectChange");
                targetRect.SetPivotKeepRenderTargetStill(pivot);
            }
        }

        private void DrawSize()
        {
            EditorGUI.BeginDisabledGroup(targetRect.useAnchor);

            EditorGUI.BeginChangeCheck();
            var size = EditorGUILayout.Vector2IntField("Size", targetRect.Size);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetRect, "RectChange");
                targetRect.Size = size;
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawMinAnchor()
        {
            EditorGUI.BeginChangeCheck();
            var minAnchorRelative = EditorGUILayout.Vector2Field("Min(Relative)", targetRect.minAnchorRelative);
            var minAnchorAbsolute = EditorGUILayout.Vector2IntField("Min(Absolute)", targetRect.minAnchorAbsolute);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetRect, "RectChange");
                targetRect.minAnchorRelative = minAnchorRelative;
                targetRect.minAnchorAbsolute = minAnchorAbsolute;
            }
        }

        private void DrawMaxAnchor()
        {
            EditorGUI.BeginChangeCheck();
            var maxAnchorRelative = EditorGUILayout.Vector2Field("Max(Relative)", targetRect.maxAnchorRelative);
            var maxAnchorAbsolute = EditorGUILayout.Vector2IntField("Max(Absolute)", targetRect.maxAnchorAbsolute);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetRect, "RectChange");
                targetRect.maxAnchorRelative = maxAnchorRelative;
                targetRect.maxAnchorAbsolute = maxAnchorAbsolute;
            }
        }
    }
}