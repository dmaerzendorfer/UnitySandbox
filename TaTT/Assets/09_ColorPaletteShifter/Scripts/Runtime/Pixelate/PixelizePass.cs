using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _09_ColorPaletteShifter.Scripts.Runtime.Pixelate
{
    //reference: https://github.com/whateep/unity-simple-URP-pixelation
    public class PixelizePass : ScriptableRenderPass
    {
        private PixelizeFeature.CustomPassSettings settings;

        private RenderTargetIdentifier colorBuffer, pixelBuffer;
        private int pixelBufferID = Shader.PropertyToID("_PixelBuffer");

        private Material material;
        private int pixelScreenHeight, pixelScreenWidth;

        public PixelizePass(PixelizeFeature.CustomPassSettings settings)
        {
            this.settings = settings;
            this.renderPassEvent = settings.renderPassEvent;
            if (material == null) material = CoreUtils.CreateEngineMaterial("Hidden/Pixelize");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, new ProfilingSampler("Pixelize Pass")))
            {
                // no-shader variant
                //Blit(cmd, colorBuffer, pointBuffer);
                //Blit(cmd, pointBuffer, pixelBuffer);
                //Blit(cmd, pixelBuffer, colorBuffer);

                Blit(cmd, colorBuffer, pixelBuffer, material);
                Blit(cmd, pixelBuffer, colorBuffer);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

            pixelScreenHeight = settings.screenHeight;
            pixelScreenWidth =
                (int)(pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f); //+0.5 to round up

            material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
            material.SetVector("_BlockSize", new Vector2(1f / pixelScreenWidth, 1f / pixelScreenHeight));
            material.SetVector("_HalfBlockSize", new Vector2(.5f / pixelScreenWidth, .5f / pixelScreenHeight));

            descriptor.height = pixelScreenHeight;
            descriptor.width = pixelScreenWidth;

            cmd.GetTemporaryRT(pixelBufferID, descriptor, FilterMode.Point);
            pixelBuffer = new RenderTargetIdentifier(pixelBufferID);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new System.ArgumentException("cmd is null");
            cmd.ReleaseTemporaryRT(pixelBufferID);
        }
    }
}