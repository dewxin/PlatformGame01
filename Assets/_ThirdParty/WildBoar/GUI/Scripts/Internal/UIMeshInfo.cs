using UnityEngine;
using System.Collections.Generic;

namespace WildBoar.GUIModule
{

    public class UIMeshInfo
    {
        public List<Vector3> vertexList = new List<Vector3>();
        public List<int> indexList = new List<int>();

        public List<Vector2> uvList = new List<Vector2>();

        public List<Color> colorList = new List<Color>();

        public static ColorSpace mColorSpace = ColorSpace.Uninitialized;
        public void ConvertColorSpace()
        {
            if (mColorSpace == ColorSpace.Uninitialized)
                mColorSpace = QualitySettings.activeColorSpace;

            if (mColorSpace == ColorSpace.Linear)
            {
                for (int i = 0; i < colorList.Count; ++i)
                    colorList[i] = colorList[i].GammaToLinearSpace();
            }
        }

    }
}
