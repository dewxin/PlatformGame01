using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WildBoar.GUIModule
{
    public partial class UIRectEditor
    {
        public const float SideLength = 14f;

        public RectCornerEnum DragedCorner { get; protected set; } = RectCornerEnum.None;
        public Vector3 DraggedCornerWorldPos => targetRect.RenderTargetWorldCorners[DragedCorner];
        public Vector2 DraggedCornerGUIPos => HandleUtility.WorldToGUIPoint(DraggedCornerWorldPos);

        public Vector2 DragStartPos { get; set; }
        public Vector2 WidgetPos { get; set; }
        public Vector2 WidgetSize { get; set; }

        private int ControlID { get; set; }

        private static int s_Hash = "UIRect".GetHashCode();

        protected virtual void OnSceneGUI()
        {

            ControlID = GUIUtility.GetControlID(s_Hash, FocusType.Passive);

            var cornerArray = targetRect.RenderTargetWorldCorners;

            NGUIHandles.DrawCornersLines(cornerArray);

            if (Tools.current == UnityEditor.Tool.Rect)
            {
                if (!IsRoot)
                {
                    OnGUIRepaint();
                    OnGUIMouseDown();
                    OnGUIMouseDrag();
                    OnGUIMouseUp();
                }
            }


        }

        public bool ExistCornerUnderMouse(Vector2 mousePosGUISpace, out RectCornerEnum rectCorner)
        {
            var handleDict = UIRectResizeHandles.GetResizeHandle(targetRect);
            foreach (var handle in handleDict.Values)
            {
                var boundingRect = handle.RectRegion;
                if (boundingRect.Contains(mousePosGUISpace))
                {
                    rectCorner = handle.CornerEnum;
                    return true;
                }
            }

            rectCorner = RectCornerEnum.None;
            return false;
        }



        public void SetCursorRect(Rect rect, MouseCursor cursor)
        {
            //https://github.com/halak/unity-editor-icons


            //TODO 好像还有点小问题
            //需要减去toolbar的高度，这里的数值是试出来的
            //minus sceneview  toolbar height,
            rect.min -= Vector2.down * 70;
            rect.max -= Vector2.down * 70;

            EditorGUIUtility.AddCursorRect(rect, cursor);
        }

        private void OnGUIRepaint()
        {
            if (Event.current.type != EventType.Repaint)
                return;

            //Vector3[] cornerArray = targetWidget.RenderTargetWorldCorners;
            //bool showDetails = NGUISettings.drawGuides;
            //if (showDetails) NGUIHandles.DrawSizeLabel(cornerArray, targetWidget.Width, targetWidget.Height);

            DrawCornerKnobs();

            DrawPivotGizmos();
        }

        public void DrawCornerKnobs()
        {
            Handles.BeginGUI();

            var handleDict = UIRectResizeHandles.GetResizeHandle(targetRect);
            foreach (var handle in handleDict.Values)
            {
                GUIStyle style = new GUIStyle("U2D.dragDot");
                style.fixedHeight = handle.RectRegion.height;
                style.fixedWidth = handle.RectRegion.width;

                SetCursorRect(handle.RectRegion, handle.Cursor);
                //EditorGUI.DrawRect(handle.RectRegion, Color.white);

                bool hover = DragedCorner == handle.CornerEnum;

                style.Draw(handle.RectRegion, hover, hover, hover, false);
            }
            Handles.EndGUI();
        }

        public void DrawPivotGizmos()
        {
            GUIStyle style = new GUIStyle("U2D.dragDotDimmed");
            Handles.BeginGUI();

            Vector2 pivotPos = HandleUtility.WorldToGUIPoint(targetRect.transform.position);

            float size = 14;
            Rect rect = new Rect(pivotPos.x - size / 2, pivotPos.y - size / 2, size, size);
            style.fixedHeight = rect.height;
            style.fixedWidth = rect.width;
            style.Draw(rect, false, true, true, false);

            Handles.EndGUI();
        }

        private void OnGUIMouseUp()
        {
            var currentEvent = Event.current;
            if (currentEvent.type != EventType.MouseUp)
                return;

            if (currentEvent.button == (int)MouseButton.MiddleMouse)
                return;

            GUIUtility.hotControl = 0;
            DragedCorner = RectCornerEnum.None;

            currentEvent.Use();
            if (currentEvent.button == (int)MouseButton.RightMouse)
            {
                // Right-click: Open a context menu listing all widgets underneath
                //NGUIEditorTools.ShowSpriteSelectionMenu(currentEvent.mousePosition);
            }

            //Selection.activeGameObject = widget.gameObject;
        }

        private void OnGUIMouseDrag()
        {
            var currentEvent = Event.current;

            var controlEventType = currentEvent.GetTypeForControl(ControlID);
            //When button is pressed, mouseDrag event will replace EventType.MouseMove
            if (currentEvent.type != EventType.MouseDrag)
            {
                return;
            }

            if (currentEvent.button != (int)MouseButton.LeftMouse)
            {
                return;
            }

            if (DragedCorner == RectCornerEnum.None)
            {
                return;
            }
                                    //TODO 这里应该根据Pivot改，而不是center
            var fromCenterToCornerVector = currentEvent.mousePosition - WidgetPos;
            var initVector = DragStartPos - WidgetPos;

            var scale = fromCenterToCornerVector / initVector;

            int scaleX = (int)(scale.x * WidgetSize.x);
            int scaleY = (int)(scale.y * WidgetSize.y);

            NGUIEditorTools.UndoRecordObject("UIRect Change", target);
            targetRect.Size = new Vector2Int(scaleX, scaleY);
        }

        private void OnGUIMouseDown()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type != EventType.MouseDown)
                return;

            if (currentEvent.button == (int)MouseButton.MiddleMouse)
            {
                GUIUtility.hotControl = 0;
                return;
            }

            if (!ExistCornerUnderMouse(currentEvent.mousePosition, out var rectCorner))
                return;

            GUIUtility.hotControl = ControlID;

            DragedCorner = rectCorner;
            DragStartPos = DraggedCornerGUIPos;
            WidgetSize = new Vector2(targetRect.Width, targetRect.Height);
            WidgetPos = HandleUtility.WorldToGUIPoint(targetRect.transform.position);

            currentEvent.Use();
        }
    }
}