using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CelestialBodyLightning : MonoBehaviour, ILightPreprocess
{
    public async IAsyncEnumerator<WaitForSeconds> GenerateCustomLighting(Light2D light, float minIntensity, float maxIntensity, SemaphoreSlim couroutineBlocker, float minInnnerRadius, float maxInnerRadius, float minOuterRadius, float maxOuterRadius, float delayBetweenExecution)
    {
        ActivateContinuousShimmer(light, Time.time, minIntensity, maxIntensity, minOuterRadius, maxOuterRadius, minInnnerRadius, maxInnerRadius);
        await Task.Delay(TimeSpan.FromMilliseconds(0));

        //release it here
        couroutineBlocker.Release();
        yield return new WaitForSeconds(0);
    }
    public void ActivateContinuousShimmer(Light2D light, float time, float minIntensity, float maxIntensity, float minOuterRadius, float maxOuterRadius, float minInnerRadius, float maxInnerRadius)
    {
        light.intensity = Mathf.PingPong(time, maxIntensity) + (minIntensity);
        light.pointLightOuterRadius = Mathf.PingPong(time * 2, maxOuterRadius) + minOuterRadius;
        light.pointLightInnerRadius = Mathf.PingPong(time * 2, maxInnerRadius) + minInnerRadius;
    }
}
