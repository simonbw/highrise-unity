#ifndef COMBINED_SHAPE_LIGHT_PASS
#define COMBINED_SHAPE_LIGHT_PASS

#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

half _HDREmulationScale;
half _UseSceneLighting;

half4 CombinedShapeVisionShared(in SurfaceData2D surfaceData, in InputData2D inputData)
{
    #if defined(DEBUG_DISPLAY)
    half4 debugColor = 0;

    if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
    {
        return debugColor;
    }
    #endif

    half alpha = surfaceData.alpha;
    half4 color = half4(surfaceData.albedo, alpha);
    const half4 mask = surfaceData.mask;
    const half2 lightingUV = inputData.lightingUV;

    // if (alpha == 0.0)
    //     discard;

// VISION!
#if USE_SHAPE_LIGHT_TYPE_3
    half4 visionMask = SAMPLE_TEXTURE2D(_ShapeLightTexture3, sampler_ShapeLightTexture3, lightingUV);

    if (any(_ShapeLightMaskFilter3))
    {
        float4 processedMask = (1 - _ShapeLightInvertedFilter3) * mask + _ShapeLightInvertedFilter3 * (1 - mask);
        visionMask *= dot(processedMask, _ShapeLightMaskFilter3);
    }
    
    visionMask = min(visionMask, 1);
#else
    half4 visionMask = 1;
#endif

    half4 finalOutput;
    finalOutput = color;

    finalOutput.a = color.a * visionMask;
    
    finalOutput = finalOutput * _UseSceneLighting + (1 - _UseSceneLighting) * color;
    
    return max(0, finalOutput);
}
#endif
