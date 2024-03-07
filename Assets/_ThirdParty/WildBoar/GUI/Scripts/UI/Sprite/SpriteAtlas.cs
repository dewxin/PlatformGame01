using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace WildBoar.GUIModule
{
    [CreateAssetMenu(fileName = "SpriteAtlas", menuName = "WildBoarGUI/SpriteAtlas", order = 1)]
    public class SpriteAtlas:ScriptableObject
    {
        [HideInInspector] 
        [SerializeField]
        private Material material;
        public Material Material { get=>material; set => material = value; }

        [SerializeField]
        private Texture2D texture;
        public Texture2D Texture { get => texture; set => texture = value; }

        [SerializeField]
        private List<SpriteData> spriteDataList = new List<SpriteData>();

        public void Remove(SpriteData data)
        {
            spriteDataList.Remove(data);
        }

        public void AddData(SpriteData data)
        {
            spriteDataList.Add(data);
        }

        public SpriteData GetData(int index)
        {
            return spriteDataList[index];
        }

        public List<SpriteData> GetSpriteList()
        {
            return spriteDataList;
        }

        public List<SpriteData> GetSpriteListSearch(string spriteName)
        {
            if(string.IsNullOrEmpty(spriteName))
            {
                return spriteDataList;
            }

            var list = new List<SpriteData>();
            foreach(var sprite in spriteDataList) { 
                
                if(sprite.Name.Contains(spriteName)) 
                    list.Add(sprite);

            }

            return list;

        }

        public bool TryGetData(string name, out SpriteData spriteData)
        {

            foreach (SpriteData data in spriteDataList)
            {
                if (data.Name.Equals(name))
                {
                    spriteData= data;
                    return true;
                }
            }
            spriteData = null;
            return false;
        }

        public void AddSpritesAndRebuild(List<SpriteData> newTexture2DList)
        {
            var spriteDataList = GetSpriteDataWithStandaloneTextureList();

            spriteDataList.AddRange(newTexture2DList);

            RebuildAtlas(spriteDataList);

        }

#if UNITY_EDITOR

        public new  void SetDirty()
        {
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(material);
            UnityEditor.EditorUtility.SetDirty(texture);

        }

#endif

        private List<SpriteData> RebuildAtlas(List<SpriteData> spriteDataList)
        {
            Texture2D[] texture2DArray = new Texture2D[spriteDataList.Count];
            for(int i = 0; i < texture2DArray.Length; i++)
            {
                texture2DArray[i] = spriteDataList[i].Texture;
            }

            //返回的是百分比
            var uvArray = texture.PackTextures(texture2DArray, 2);
            material.SetTexture("_MainTex", texture);

            for (int i =0; i < uvArray.Length; ++i)
            {
                var spriteData = spriteDataList[i];

                var uv = uvArray[i];
                spriteData.Rect = new Rect();
                {
                    var size = new Vector2(texture.width,texture.height);
                    spriteData.Rect.position = uv.position * size;
                    spriteData.Rect.size = uv.size * size;
                };


                spriteData.Texture= texture;

            }


            this.spriteDataList = spriteDataList;

            return spriteDataList;
        }


        public List<SpriteData> GetSpriteDataWithStandaloneTextureList()
        {
            var list = new List<SpriteData>();

            foreach(var spriteData in spriteDataList)
            {
                var texture = new Texture2D((int)spriteData.Rect.width, (int)spriteData.Rect.height, TextureFormat.ARGB32, false);
                Color[] pixels = Texture.GetPixels((int)spriteData.Rect.x,(int)spriteData.Rect.y,
                                    (int)spriteData.Rect.width,(int)spriteData.Rect.height);

                texture.SetPixels(pixels);

                var newSpriteData = new SpriteData()
                {
                    Name = spriteData.Name,
                    Rect = new Rect(0, 0, spriteData.Rect.width, spriteData.Rect.height),
                    Slice9Padding = spriteData.Slice9Padding,
                    Pivot = spriteData.Pivot,
                    Texture = texture
                };

                list.Add(newSpriteData);

            }
            return list;
        }


    }

    [Serializable]
    public class SpriteData
    {
        public string Name;

        //position + size
        public Rect Rect;

        //x← y↑ z→ w↓
		// Sliced sprites generally have a border. X = left, Y = bottom, Z = right, W = top.
        public Vector4 Slice9Padding;

        public Vector2 Pivot = Vector2.one * 0.5f;

        //
        public Texture2D Texture { get; set; }

    }
}
