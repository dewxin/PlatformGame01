
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace WildBoar.GUIModule
{

    public class UIDrawCallMaterialManager:Singleton<UIDrawCallMaterialManager>
    {

        private Dictionary<Texture2D, Material> texture2MaterialDict = new Dictionary<Texture2D, Material>();

        //TODO 这里还是会残留之前存储的Material（但是因为被销毁，会返回null）
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
