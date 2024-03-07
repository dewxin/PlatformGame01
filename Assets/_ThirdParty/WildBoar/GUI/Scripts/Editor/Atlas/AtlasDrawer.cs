using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WildBoar.GUIModule
{

    //TODO
    //2d bin packing algorithm
    //https://www.dei.unipd.it/%7Efisch/ricop/tesi/tesi_dottorato_Lodi_1999.pdf

    public class AtlasDrawer
    {
        public SpriteAtlas CurrentAtlas { get; set; }
        public SpriteData CurrentSpriteData { get; set; }
        public string SearchText { get; set; } = string.Empty;

        private List<SpriteData> displaySpriteList = new List<SpriteData>();
        private Vector2 scrollPos;

        public Action<SpriteAtlas, SpriteData> OnSelectSprite = delegate { };

        public Action<SpriteData> OnSpriteRightClick;
        public Action<List<Texture2D>> OnAcceptTextures;

        public AtlasDrawer(SpriteAtlas Atlas, Action<SpriteAtlas,SpriteData> OnSelectSprite)
        {
            CurrentAtlas = Atlas;

            if(OnSelectSprite!= null)
                this.OnSelectSprite += OnSelectSprite;

            OnSpriteRightClick = ShowRightClickMenu;
        }

        public void DrawSpriteDatas()
        {
            NGUIEditorTools.SetLabelWidth(80f);

            GUILayout.Label(CurrentAtlas.name + " Sprites", "LODLevelNotifyText");
            NGUIEditorTools.DrawSeparator();

            GUILayout.BeginHorizontal();
            GUILayout.Space(84f);

            var changed = false;
            var newSearch = EditorGUILayout.TextField("", SearchText, "SearchTextField");

            if (newSearch != SearchText)
            {
                SearchText = newSearch;
                changed = true;
            }


            HanleDragEvent();

            GUILayout.Space(84f);
            GUILayout.EndHorizontal();

            var tex = CurrentAtlas.Texture as Texture2D;

            if (tex == null)
            {
                GUILayout.Label("The atlas doesn't have a texture to work with");
                return;
            }


            if (displaySpriteList.Count == 0 || changed)
            {
                displaySpriteList = CurrentAtlas.GetSpriteListSearch(SearchText);
            }

            if (displaySpriteList.Count == 0)
            {
                GUILayout.Label("No sprites found");
                return;
            }

            float size = 80f;
            float padded = size + 10f;

            int screenWidth = (int)EditorGUIUtility.currentViewWidth;

            int columns = Mathf.FloorToInt(screenWidth / padded);
            if (columns < 1) columns = 1;

            Rect rect = new Rect(10f, 0, size, size);

            var ev = Event.current;
            var et = ev.type;
            //var sw = (et == EventType.Repaint) ? System.Diagnostics.Stopwatch.StartNew() : null;

            GUILayout.Space(10f);
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            int rows = 1;

            int offset = 0;
            while (offset < displaySpriteList.Count)
            {
                GUILayout.BeginHorizontal();
                {
                    int col = 0;
                    rect.x = 10f;

                    for (; offset < displaySpriteList.Count; ++offset)
                    {
                        var spriteData = CurrentAtlas.GetData(offset);
                        if (spriteData == null) continue;

                        // Button comes first
                        if (et != EventType.Repaint && GUI.Button(rect, ""))
                        {
                            if (ev.button == 0)
                            {
                                CurrentSpriteData = spriteData;
                                OnSelectSprite(CurrentAtlas, CurrentSpriteData);
                            }
                            else
                            {
                                OnSpriteRightClick(spriteData);
                            }
                        }

                        if (et == EventType.Repaint)
                        {
                            // On top of the button we have a checkboard grid
                            NGUIEditorTools.DrawTiledTexture(rect, NGUIEditorTools.backdropTexture);
                            var uv = NGUIMath.ConvertToTexCoords(spriteData.Rect, tex.width, tex.height);

                            // Calculate the texture's scale that's needed to display the sprite in the clipped area
                            var scaleX = rect.width / uv.width;
                            var scaleY = rect.height / uv.height;

                            // Stretch the sprite so that it will appear proper
                            var aspect = (scaleY / scaleX) / ((float)tex.height / tex.width);
                            var clipRect = rect;

                            if (aspect != 1f)
                            {
                                if (aspect < 1f)
                                {
                                    // The sprite is taller than it is wider
                                    float padding = size * (1f - aspect) * 0.5f;
                                    clipRect.xMin += padding;
                                    clipRect.xMax -= padding;
                                }
                                else
                                {
                                    // The sprite is wider than it is taller
                                    float padding = size * (1f - 1f / aspect) * 0.5f;
                                    clipRect.yMin += padding;
                                    clipRect.yMax -= padding;
                                }
                            }

                            GUI.DrawTextureWithTexCoords(clipRect, tex, uv);

                            // Draw the selection
                            if (CurrentSpriteData == spriteData)
                                NGUIEditorTools.DrawOutline(rect, new Color(0.4f, 1f, 0f, 1f));
                        }

                        if (et == EventType.Repaint)
                        {
                            GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
                            GUI.contentColor = new Color(1f, 1f, 1f, 0.7f);
                            GUI.Label(new Rect(rect.x, rect.y + rect.height, rect.width, 32f), spriteData.Name, "ProgressBarBack");
                            GUI.contentColor = Color.white;
                            GUI.backgroundColor = Color.white;
                        }

                        if (++col >= columns)
                        {
                            ++offset;
                            break;
                        }
                        rect.x += padded;
                    }
                }


                GUILayout.EndHorizontal();

                rect.y += padded + 26;
                ++rows;
            }
            GUILayout.Space(rows * (26 + padded));

            GUILayout.EndScrollView();


        }

        private void HanleDragEvent()
        {
            if (OnAcceptTextures == null)
                return;

            var eventType = Event.current.type;

            if (eventType == EventType.DragExited)
            {
                if (GUI.enabled)
                {
                    HandleUtility.Repaint();
                }

            }

            if(eventType == EventType.DragUpdated)
            {
                if (DragAndDrop.visualMode == DragAndDropVisualMode.None)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }
            }

            if (eventType == EventType.DragPerform)
            {

                List<Texture2D> list = new List<Texture2D>();
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    if (obj is not Texture2D)
                    {
                        Debug.Log($"{obj} not texture2d");
                        continue;
                    }
                    list.Add(obj as Texture2D);

                }

                GUI.changed = true;
                if(OnAcceptTextures != null)
                    OnAcceptTextures(list);
                DragAndDrop.AcceptDrag();
                DragAndDrop.activeControlID = 0;
                Event.current.Use();
            }
        }


        private void ShowRightClickMenu(SpriteData spriteData)
        {
            NGUIContextMenu.AddItem("Log", false, ContextMenuLogSprite, spriteData);
            NGUIContextMenu.Show();
        }

        void ContextMenuLogSprite(object obj)
        {
            if (this == null) return;
            var spriteData = obj as SpriteData;
            Debug.Log(spriteData.Name);
        }
    }
}
