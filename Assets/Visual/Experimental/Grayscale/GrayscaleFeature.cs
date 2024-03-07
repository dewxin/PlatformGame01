using UnityEngine.Rendering.Universal;

public class GrayscaleFeature : ScriptableRendererFeature
{
    GrayscaleRenderPass pass;
    public override void Create()
    {
        name = "Grayscale";
        pass = new GrayscaleRenderPass();
    }
    public override void AddRenderPasses(ScriptableRenderer renderer,
    ref RenderingData renderingData)
    {
        pass.Setup(renderer, "Grayscale Post Process");
    }
}