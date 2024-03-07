using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Overlays;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

class EditorWindowOverlayExample : EditorWindow, ISupportsOverlays
{
    [MenuItem("Tools/Example/Overlay Supported Window Example")]
    static void Init() => GetWindow<EditorWindowOverlayExample>();

    void OnGUI()
    {
        GUILayout.Label("Here is some text");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Plus some more text, but on the bottom of the screen.");
    }
}

[Overlay(typeof(EditorWindowOverlayExample), "Is Mouse Hovering Me?", true)]
class IsMouseHoveringMe : Overlay
{
    Label m_MouseLabel;

    public override VisualElement CreatePanelContent()
    {
        m_MouseLabel = new Label();
        m_MouseLabel.style.minHeight = 40;
        m_MouseLabel.RegisterCallback<MouseEnterEvent>(evt => m_MouseLabel.text = "Mouse is hovering this Overlay content!");
        m_MouseLabel.RegisterCallback<MouseLeaveEvent>(evt => m_MouseLabel.text = "Mouse is not hovering this Overlay content.");
        return m_MouseLabel;
    }
}