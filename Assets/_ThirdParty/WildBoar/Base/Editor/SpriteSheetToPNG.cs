//#if UNITY_EDITOR

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEditor;
//using UnityEngine;


//public class SpriteSheet
//{
//    public List<Sprite> spriteList;
//    public string filePath;
//}

//internal class SpriteSheetToPNG
//{

//    public static void Save(SpriteSheet spriteSheet)
//    {
//        var folderPath = Path.GetDirectoryName(spriteSheet.filePath);
//        if (!Directory.Exists(folderPath))
//            Directory.CreateDirectory(folderPath);

//        var rectList = SaveAndGetRect(spriteSheet);
//        SetSheet(spriteSheet, rectList);

//    }

//    private static List<Rect> SaveAndGetRect(SpriteSheet spriteSheet)
//    {
//        var textureList = new List<Texture2D>();
//        foreach (var sprite in spriteSheet.spriteList)
//            textureList.Add(sprite.texture);

//        Texture2D atlas = new Texture2D(2048, 2048);
//        var rects = atlas.PackTextures(textureList.ToArray(), 2, 2048);

//        var rectList = new List<Rect>();
//        foreach(var rect in rects)
//        {
//            // PackTextures函数返回的值是相对百分比，需要转化为像素单位
//            var xMin = rect.xMin * atlas.width;
//            var xMax = rect.xMax * atlas.width;
//            var yMin = rect.yMin * atlas.height;
//            var yMax = rect.yMax * atlas.height;

//            var result = new Rect
//            {
//                xMin = xMin,
//                xMax = xMax,
//                yMin = yMin,
//                yMax = yMax
//            };
//            rectList.Add(result);
//        }

//        byte[] bytes = atlas.EncodeToPNG();
//        File.WriteAllBytes(spriteSheet.filePath, bytes);
//        var filePath = Path.GetRelativePath(System.Environment.CurrentDirectory, spriteSheet.filePath);
//        AssetDatabase.ImportAsset(filePath);
//        return rectList;

//    }

//    private static void SetSheet(SpriteSheet spriteSheet, List<Rect> rectList)
//    {
//        var filePath = Path.GetRelativePath(System.Environment.CurrentDirectory, spriteSheet.filePath);

//        TextureImporter textureImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;
//        textureImporter.spriteImportMode = SpriteImportMode.Multiple;
//        textureImporter.SaveAndReimport();


//        var obj = AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D));

//        var factory = new SpriteDataProviderFactories();
//        factory.Init();
//        var dataProvider = factory.GetSpriteEditorDataProviderFromObject(obj);
//        dataProvider.InitSpriteEditorDataProvider();

//        /* Use the data provider */
//        List<SpriteRect> spriteRectList = new List<SpriteRect>();

//        for (int i = 0; i < rectList.Count; ++i)
//        {
//            var rect = rectList[i];
//            var sprite = spriteSheet.spriteList[i];

//            SpriteRect spriteRect = new SpriteRect();
//            spriteRect.rect = rect;
//            spriteRect.spriteID = GUID.Generate();
//            //sprite.pivot返回的是像素单位，需要转为百分比
//            spriteRect.pivot = new Vector2(sprite.pivot.x/ sprite.texture.width, sprite.pivot.y/sprite.texture.height);
//            spriteRect.alignment = SpriteAlignment.Custom;
//            spriteRect.name = sprite.name;

//            spriteRectList.Add(spriteRect);

//        }

//        dataProvider.SetSpriteRects(spriteRectList.ToArray());
//        dataProvider.Apply();

//        // Reimport the asset to have the changes applied
//        var assetImporter = dataProvider.targetObject as AssetImporter;
//        assetImporter.SaveAndReimport();

//    }


//}

//#endif
