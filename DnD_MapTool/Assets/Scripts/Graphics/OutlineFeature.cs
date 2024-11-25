// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

using System.CodeDom;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature: ScriptableRendererFeature
{
    class OutlinePass: ScriptableRenderPass
    {
        private RenderTargetIdentifier source { get; set; }
        private RTHandle destination { get; set; }
        public Material outlineMaterial = null;
        public RTHandle temporaryColorTexture;

        public void Setup(RenderTargetIdentifier source, RTHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public OutlinePass(Material outlineMaterial)
        {
            this.outlineMaterial = outlineMaterial;
        }



        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            // Create Temp Texture to hold Camera Color
            temporaryColorTexture = RTHandles.Alloc("_TemporaryColorTexture");

            // (doesn't need depthBits so setting this to 0, so the RT might use less memory I guess?)
            RenderTextureDescriptor tempRTD = cameraTextureDescriptor;
            tempRTD.depthBufferBits = 0;

            cmd.GetTemporaryRT(Shader.PropertyToID(temporaryColorTexture.name), tempRTD, FilterMode.Point);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("_OutlinePass");

            RenderTextureDescriptor opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDescriptor.depthBufferBits = 0;

            if(destination == ScriptableRenderPass.k_CameraTarget)
            {
                cmd.GetTemporaryRT(Shader.PropertyToID(temporaryColorTexture.name), opaqueDescriptor, FilterMode.Point);
                cmd.Blit(source, temporaryColorTexture, outlineMaterial, 0);
                // cmd.Blit(temporaryColorTexture, source);

            }
            else
                cmd.Blit(source, destination, outlineMaterial, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {

            if(destination == ScriptableRenderPass.k_CameraTarget)
                cmd.ReleaseTemporaryRT(Shader.PropertyToID(temporaryColorTexture.name));
        }
    }

    [System.Serializable]
    public class OutlineSettings
    {
        public Material outlineMaterial = null;
    }

    public OutlineSettings settings = new OutlineSettings();
    OutlinePass outlinePass;
    RTHandle outlineTexture;

    public override void Create()
    {
        outlinePass = new OutlinePass(settings.outlineMaterial);
        outlinePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        outlineTexture = RTHandles.Alloc("_OutlineTexture");
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(settings.outlineMaterial == null)
        {
            Debug.LogWarningFormat("Missing Outline Material");
            return;
        }
        outlinePass.Setup(renderer.cameraColorTargetHandle, ScriptableRenderPass.k_CameraTarget);
        renderer.EnqueuePass(outlinePass);
    }
}