using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.Sprites;

namespace WildBoar.GUIModule
{
    internal class NEditorGUI
    {
        private static int s_ObjectFieldHash = $"{nameof(s_ObjectFieldHash)}".GetHashCode();

        static NEditorGUI()
        {
        }

        public static UnityEngine.Object ObjectField(UnityEngine.Object obj, Type objType, params GUILayoutOption[] options)
        {
            return ObjectField(EditorGUILayout.GetControlRect(hasLabel: false, 18f, options), obj, objType );
        }

        public static UnityEngine.Object ObjectField(Rect position, UnityEngine.Object obj, Type objType )
        {
            int controlID = GUIUtility.GetControlID(s_ObjectFieldHash, FocusType.Keyboard, position);
            return DoObjectField(EditorGUI.IndentedRect(position), controlID, obj,  objType );
        }

        internal static UnityEngine.Object DoObjectField(Rect position,  int id, UnityEngine.Object obj,  Type objType)
        {
            return DoObjectField(position, id, obj, objType, EditorStyles.objectField, GUI.skin.GetStyle("ObjectFieldButton"));
        }

        private static UnityEngine.Object DoObjectField(Rect position,  int id, UnityEngine.Object obj,  Type objType,  GUIStyle style, GUIStyle buttonStyle, Action<UnityEngine.Object> onObjectSelectorClosed = null, Action<UnityEngine.Object> onObjectSelectedUpdated = null)
        {

            Event current = Event.current;
            EventType eventType = current.type;

            bool flag = EditorGUIUtility.HasObjectThumbnail(objType);

            Vector2 iconSize = EditorGUIUtility.GetIconSize();
            EditorGUIUtility.SetIconSize(new Vector2(12f, 12f));

            if (((eventType == EventType.MouseDown && Event.current.button == 1) || (eventType == EventType.ContextClick) && position.Contains(Event.current.mousePosition)))
            {
                UnityEngine.Object actualObject =  obj;
                GenericMenu genericMenu = new GenericMenu();

                genericMenu.AddItem(new GUIContent("Properties..."), on: false, delegate
                {
                    Debug.Log("no thing");
                });
                genericMenu.DropDown(position);
                Event.current.Use();
            }

            switch (eventType)
            {
                case EventType.MouseDown:
                    {
                        if (!position.Contains(Event.current.mousePosition) || Event.current.button != 0)
                        {
                            break;
                        }

                        Rect buttonRect = GetButtonRect(position);
                        EditorGUIUtility.editingTextField = false;
                        if (buttonRect.Contains(Event.current.mousePosition))
                        {
                            if (GUI.enabled)
                            {
                                GUIUtility.keyboardControl = id;
                                Type[] requiredTypes = new Type[1] { objType };

                                //NObjectSelector.get.Show(obj, requiredTypes, objBeingEdited, allowSceneObjects, null, onObjectSelectorClosed, onObjectSelectedUpdated);
                                Debug.Log("Open Button Clicked");

                                current.Use();
                                GUIUtility.ExitGUI();
                            }

                            break;
                        }

                        UnityEngine.Object @object = obj;
                        Component component = @object as Component;
                        if ((bool)component)
                        {
                            @object = component.gameObject;
                        }

           
                        if (Event.current.clickCount == 1)
                        {
                            GUIUtility.keyboardControl = id;
                            EditorGUIUtility.PingObject(@object);

                            current.Use();
                        }
                        else if (Event.current.clickCount == 2 && (bool)@object)
                        {
                            AssetDatabase.OpenAsset(@object);
                            current.Use();
                            GUIUtility.ExitGUI();
                        }

                        break;
                    }



                case EventType.DragExited:
                    if (GUI.enabled)
                    {
                        HandleUtility.Repaint();
                    }

                    break;
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    {
                        if (eventType == EventType.DragPerform && !ValidDroppedObject(DragAndDrop.objectReferences, objType, out var errorString))
                        {
                            UnityEngine.Object object2 = DragAndDrop.objectReferences[0];
                            EditorUtility.DisplayDialog("Can't assign script", errorString, "OK");
                        }
                        else
                        {
                            if (!position.Contains(Event.current.mousePosition) || !GUI.enabled)
                            {
                                break;
                            }

                            UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;

                            UnityEngine.Object object3 = ValidateObjectFieldAssignment(objectReferences, objType);
                            if (object3 != null && !EditorUtility.IsPersistent(object3))
                            {
                                object3 = null;
                            }

                            if (!(object3 != null))
                            {
                                break;
                            }

                            if (DragAndDrop.visualMode == DragAndDropVisualMode.None)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                            }

                            if (eventType == EventType.DragPerform)
                            {
                                obj = object3;

                                GUI.changed = true;
                                DragAndDrop.AcceptDrag();
                                DragAndDrop.activeControlID = 0;
                            }
                            else
                            {
                                DragAndDrop.activeControlID = id;
                            }

                            Event.current.Use();
                        }

                        break;
                    }

                case EventType.Repaint:
                    {
                        GUIContent content = EditorGUIUtility.ObjectContent(obj, objType) ;
                        {
                            style.Draw(position, content, id, DragAndDrop.activeControlID == id, position.Contains(Event.current.mousePosition));
                            Rect position2 = buttonStyle.margin.Remove(GetButtonRect( position));
                            buttonStyle.Draw(position2, GUIContent.none, id, DragAndDrop.activeControlID == id, position2.Contains(Event.current.mousePosition));
                        }
                        break;
                    }
            }

            EditorGUIUtility.SetIconSize(iconSize);
            return obj;
        }

        private static bool HasValidScript(UnityEngine.Object obj)
        {
            MonoScript monoScript = MonoScript.FromScriptableObject(obj as ScriptableObject);
            if (monoScript == null)
            {
                return false;
            }

            Type @class = monoScript.GetClass();
            if (@class == null)
            {
                return false;
            }

            return true;
        }

        private static bool ValidDroppedObject(UnityEngine.Object[] references, Type objType, out string errorString)
        {
            errorString = "";
            if (references == null || references.Length == 0)
            {
                return true;
            }

            UnityEngine.Object @object = references[0];
            UnityEngine.Object object2 = EditorUtility.InstanceIDToObject(@object.GetInstanceID());
            if ((object2 is MonoBehaviour || object2 is ScriptableObject) && !HasValidScript(object2))
            {
                errorString = $"Type cannot be found: {@object.GetType()}. Containing file and class name must match.";
                return false;
            }

            return true;
        }

        internal static UnityEngine.Object ValidateObjectFieldAssignment(UnityEngine.Object[] references, Type objType)
        {
            if (references.Length != 0)
            {
                {
                    if (references[0] != null && references[0] is GameObject && typeof(Component).IsAssignableFrom(objType))
                    {
                        GameObject gameObject2 = (GameObject)references[0];
                        UnityEngine.Object[] components = gameObject2.GetComponents(typeof(Component));
                        references = components;
                    }

                    UnityEngine.Object[] array2 = references;
                    foreach (UnityEngine.Object object2 in array2)
                    {
                        if (object2 != null && objType.IsAssignableFrom(object2.GetType()))
                        {
                            return object2;
                        }
                    }
                }
            }

            return null;
        }

        private static Rect GetButtonRect(Rect position)
        {
            return
                 new Rect(position.xMax - 19f, position.y, 19f, position.height);
        }
    }
}
