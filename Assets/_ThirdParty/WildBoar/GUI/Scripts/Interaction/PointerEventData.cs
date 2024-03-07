using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace WildBoar.GUIModule
{
    public class PointerEventData : BaseEventData
    {
        // The object that received OnPointerDown
        public GameObject PointerDownGO { get; set; }

        /// <summary>
        /// The raw GameObject for the last press event. This means that it is the 'pressed' GameObject even if it can not receive the press event itself.
        /// </summary>
        public GameObject LastPointerDownGO { get; set; }

        /// <summary>
        /// The object that is receiving 'OnDrag'.
        /// </summary>
        public GameObject pointerDrag { get; set; }

        public GameObject pointerClick { get; set; }

        public Vector2 PointerCurrentPos { get; set; }

        public Vector2 PointerPosDelta { get; set; }

        public float LastClickTime { get; set; }

        public Vector2 MouseScrollDelta { get; set; }

        public MouseButton MouseButton { get; set; }

        /// <seealso cref="UnityEngine.UIElements.IPointerEvent" />

        public bool IsPointerMoving => PointerPosDelta.sqrMagnitude > 0;

        public bool IsScrolling => MouseScrollDelta.sqrMagnitude > 0;

        public PointerEventData(EventSystem eventSystem) : base(eventSystem)
        {
            PointerCurrentPos = Vector2.zero; // Current position of the mouse or touch event
            PointerPosDelta = Vector2.zero; // Delta since last update
            LastClickTime = 0.0f; // The last time a click event was sent out (used for double-clicks)

            MouseScrollDelta = Vector2.zero;
            MouseButton = MouseButton.LeftMouse;

        }



        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<b>Position</b>: " + PointerCurrentPos);
            sb.AppendLine("<b>delta</b>: " + PointerPosDelta);
            sb.AppendLine("<b>lastPointerPress</b>: " + LastPointerDownGO);
            sb.AppendLine("<b>pointerDrag</b>: " + pointerDrag);
            sb.AppendLine("<b>Current Raycast:</b>");
            sb.AppendLine("<b>Press Raycast:</b>");
            sb.AppendLine("<b>Display Index:</b>");
            return sb.ToString();
        }
    }
}
