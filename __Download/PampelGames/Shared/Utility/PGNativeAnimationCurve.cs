// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    /// <summary>
    ///     AnimationCurves for IJobs.
    /// </summary>
    public struct PGNativeAnimationCurve : System.IDisposable
    {
        private NativeArray<float> nativeCurve;
            
        public PGNativeAnimationCurve(AnimationCurve curve, int samples)
        {
            nativeCurve = new NativeArray<float>(samples, Allocator.Persistent);
            float timeStart = curve.keys[0].time;
            float timeEnd = curve.keys[^1].time;
            float timeStep = (timeEnd - timeStart) / (samples - 1);
            for (int i = 0; i < samples; i++) nativeCurve[i] = curve.Evaluate(timeStart + i * timeStep);
        }
 
        /// <summary>
        ///     Evaluates the curve.
        /// </summary>
        /// <param name="time">Clamped from 0 to 1.</param>
        public float Evaluate(float time)
        {
            int curveLength = nativeCurve.Length - 1;
            float clamp01 = time < 0 ? 0 : time > 1 ? 1 : time;
            float floatIndex = clamp01 * curveLength;
            int floorIndex = (int)math.floor(floatIndex);
            if (floorIndex == curveLength) return nativeCurve[curveLength];
            float lowerValue = nativeCurve[floorIndex];
            float higherValue = nativeCurve[floorIndex + 1];
            return math.lerp(lowerValue, higherValue, math.frac(floatIndex));
        }
        
        public void Dispose()
        {
            if(nativeCurve.IsCreated) nativeCurve.Dispose();
        }
    }
    
    
    /* Example
        public void Execute()
        {
            var sampledCurve = new PGNativeAnimationCurve(transitionCurve, terrainData.heightmapResolution);

            ContrastHeightJob job = new ContrastHeightJob
            {
                moduleFlatArraySum = moduleFlatArraySum,
                nativeCurve = sampledCurve
            };

            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            sampledCurve.Dispose();
        }

        [BurstCompile]
        private struct ContrastHeightJob : IJob
        {
            public NativeArray<float> moduleFlatArraySum;
            public PGNativeAnimationCurve nativeCurve;

            public void Execute()
            {
                for (int i = 0; i < moduleFlatArraySum.Length; i++)
                {
                    float curveValue = nativeCurve.Evaluate(moduleFlatArraySum[i]);
                }
            }
        }

     */
}