using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AdjustPlatformTool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AjustPlatformOrthogonal(float degDiffThreshold)
    {
        foreach (Transform line in transform)
        {
            System.Collections.Generic.List<Transform> pointPosList = new System.Collections.Generic.List<Transform>();
            foreach (Transform point in line)
                pointPosList.Add(point);

            for(int i = 0; i < pointPosList.Count-1; i++)
            {
                var point1 = pointPosList[i];
                var point2 = pointPosList[i+1];
                var vec = point2.position - point1.position;

                bool horitontal = Vector2.Angle(vec, Vector2.right) < degDiffThreshold;
                horitontal |= Vector2.Angle(vec, Vector2.left) < degDiffThreshold;
                //小于限制的度数，将其设置为水平面
                if (horitontal)
                {
                    point2.position = new Vector3(point2.position.x, point1.position.y,point2.position.z);
                }


                bool vertical = Vector2.Angle(vec, Vector2.up) < degDiffThreshold;
                vertical |= Vector2.Angle(vec, Vector2.down) < degDiffThreshold;

                if(vertical)
                {
                    point2.position = new Vector3(point1.position.x, point2.position.y,point2.position.z);
                }

            }

        }


    }
}
