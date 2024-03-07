
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace WildBoar.GUIModule
{

    public class UIDrawCallMaterialManager:Singleton<UIDrawCallMaterialManager>
    {

        private Dictionary<Texture2D, Material> texture2MaterialDict = new Dictionary<Texture2D, Material>();

        //TODO ���ﻹ�ǻ����֮ǰ�洢��Material��������Ϊ�����٣��᷵��null��
        public Material GetMaterialBySprite(Sprite sprite)
        {
            if(texture2MaterialDict.TryGetValue(sprite.texture, out var material)) 
                return material;

            var shader = Shader.Find("Unlit/Transparent Colored");
            Debug.Assert(shader!= null);
            var newMaterial = new Material(shader);

            newMaterial.SetTexture("_MainTex", sprite.texture);
            texture2MaterialDict[sprite.texture] = newMaterial;
            return newMaterial;
        }


    }
}
