using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
class GrayscaleRenderPass : ScriptableRenderPass
{
    private Material material;
    private GrayscaleSettings settings;
    private RenderTargetIdentifier source;
    private RenderTargetIdentifier mainTex;
    private string profilerTag;

    public void Setup(ScriptableRenderer renderer, string profilerTag)
    {
        this.profilerTag = profilerTag;
        source = renderer.cameraColorTarget;
        VolumeStack stack = VolumeManager.instance.stack;
        settings = stack.GetComponent<GrayscaleSettings>();
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        if (settings != null && settings.IsActive())
        {
            renderer.EnqueuePass(this);
            material = new Material(Shader.Find("Examples/ImageEffects/Grayscale"));
        }
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor
cameraTextureDescriptor)
    {
        if (settings == null) return;
        int id = Shader.PropertyToID("_MainTex");
        mainTex = new RenderTargetIdentifier(id);
        cmd.GetTemporaryRT(id, cameraTextureDescriptor);
        base.Configure(cmd, cameraTextureDescriptor);
    }

    public override void Execute(ScriptableRenderContext context, ref
RenderingData renderingData)
    {
        if (!settings.IsActive())
        {
            return;
        }
        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.Blit(source, mainTex);
        material.SetFloat("_Strength", settings.strength.value);
        cmd.Blit(mainTex, source, material);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(Shader.PropertyToID("_MainTex"));
    }
}