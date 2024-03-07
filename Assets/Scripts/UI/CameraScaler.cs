using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    private float pixelPerUnit = 768 / 3.84f;
    // Start is called before the first frame update
    void Awake()
    {
        ScaleCamera();
    }

    // Update is called once per frame
    void Update()
    {
        ScaleCamera();
    }

    private void ScaleCamera()
    {
        Camera camera = GetComponent<Camera>();

        //vertical Size
        camera.orthographicSize = Screen.height / pixelPerUnit;

    }

}
