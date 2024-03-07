using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WildBoar.GUIModule
{
    public class UIRectResizeHandles
    {
        public const float SideLength = 14f;

        public RectCornerEnum CornerEnum { get; set; }
        public Rect RectRegion { get; set; }
        public MouseCursor Cursor { get; set; }

        public static Dictionary<RectCornerEnum, UIRectResizeHandles> GetResizeHandle(UIRect uiRect)
        {
            var rectCorners = uiRect.RenderTargetWorldCorners;

            var result = new Dictionary<RectCornerEnum, UIRectResizeHandles>();

            for (RectCornerEnum cornerEnum = RectCornerEnum.BottomLeft; cornerEnum < RectCornerEnum.EnumCount; ++cornerEnum)
            {
                var corner = rectCorners[cornerEnum];

                var guiPos = HandleUtility.WorldToGUIPoint(corner);
                Rect boundingRect = new Rect(guiPos.x - SideLength / 2, guiPos.y - SideLength / 2, SideLength, SideLength);

                UIRectResizeHandles handle = new UIRectResizeHandles();
                handle.CornerEnum = cornerEnum;
                handle.RectRegion = boundingRect;
                handle.Cursor = GetCursorByCornerEnum(cornerEnum);

                result.Add(handle.CornerEnum, handle);
            }

            return result;
        }

        private static MouseCursor GetCursorByCornerEnum(RectCornerEnum cornerEnum)
        {
            if (cornerEnum == RectCornerEnum.BottomLeft)
                return MouseCursor.ResizeUpRight;
            if (cornerEnum == RectCornerEnum.TopRight)
                return MouseCursor.ResizeUpRight;

            if (cornerEnum == RectCornerEnum.TopLeft)
                return MouseCursor.ResizeUpLeft;
            if (cornerEnum == RectCornerEnum.BottomRight)
                return MouseCursor.ResizeUpLeft;

            return MouseCursor.Arrow;
        }
    }
}