using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderUser : MonoBehaviour
{
    public ComputeShader computeShader;

    public Material material;

    private int kernelIndex;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        RenderTexture mRenderTexture = new RenderTexture(256, 256, 0);
        mRenderTexture.enableRandomWrite = true;
        mRenderTexture.Create();


        material.mainTexture = mRenderTexture;
        kernelIndex = computeShader.FindKernel("CSMain");
        computeShader.SetTexture(kernelIndex, "Result", mRenderTexture);
    }

    // Update is called once per frame
    void Update()
    {
        computeShader.Dispatch(kernelIndex, 256 / 8, 256 / 8, 1);
    }
}
