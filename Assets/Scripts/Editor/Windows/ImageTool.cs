using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using Color = System.Drawing.Color;
using System.Security.Policy;

public class ImageTool : EditorWindow
{
    private string ImagePath => Path.Combine(Application.dataPath, "Sprites");

    private bool deleteBmp;

    [MenuItem("Tools/Windows/Image Tool")]
    private static void CreateWindow()
    {
        GetWindow<ImageTool>();
    }

    private void OnGUI()
    {
        GUIStyle toggleStyle = GUI.skin.toggle;

        GUIStyle buttonStyle = GUI.skin.button;
        {
            buttonStyle.richText = true;
            buttonStyle.alignment = TextAnchor.MiddleLeft;

            buttonStyle.hover = toggleStyle.hover;
            buttonStyle.onActive= toggleStyle.onActive;

        }
        deleteBmp = GUILayout.Toggle(deleteBmp, "Delete Bmp", buttonStyle, GUILayout.MinWidth(20f));

        if (GUILayout.Button("Convert Bmp to png"))
        {
            var path = EditorUtility.OpenFolderPanel("Select Folder", ImagePath, "");

            ConvertBmp2Png(path);
            if(deleteBmp)
                DeleteBmp(path);


            AssetDatabase.Refresh();

        }



    }

    private void DeleteBmp(string path)
    {
        var fileNameList = Directory.GetFiles(path).ToList();
        var bmpFileList = fileNameList.Where(fileName => fileName.EndsWith(".bmp"));
        foreach ( var fileName in bmpFileList)
        {
            File.Delete(fileName);
        }
    }


    private void ConvertBmp2Png(string path)
    {
        var fileNameList = Directory.GetFiles(path).ToList();
        var bmpFileList = fileNameList.Where(fileName => fileName.EndsWith(".bmp"));

        foreach (var bmpFileName in bmpFileList)
        {
            int width = -1;
            int height = -1;
            using (Bitmap bmp1 = new Bitmap(bmpFileName))
            {
                width = bmp1.Width;
                height = bmp1.Height;
            }


            using (FileStream fileStream = new FileStream(bmpFileName, FileMode.Open, FileAccess.Read))
            {

                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                Bitmap bitmap = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color color;
                        if (y * width + x + 57 <= buffer.Length - 1) color = Color.FromArgb(Convert.ToInt32(buffer[(y * width + x) * 4 + 57]), Convert.ToInt32(buffer[(y * width + x) * 4 + 56]), Convert.ToInt32(buffer[(y * width + x) * 4 + 55]), Convert.ToInt32(buffer[(y * width + x) * 4 + 54]));
                        else color = Color.FromArgb(0, 0, 0, 0);
                        bitmap.SetPixel(x, y, color);
                    }
                }

                bitmap.Save(bmpFileName.Replace(".bmp", ".png"), ImageFormat.Png);

            }
        }

    }

}
