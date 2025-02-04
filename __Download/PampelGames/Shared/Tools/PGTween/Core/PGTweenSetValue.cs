// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Sets the current value of a <see cref="PGTweenDescr" /> for all available types.
    /// </summary>
    internal static class PGTweenSetValue
    {
        public static void SetFloat(PGTweenDescr tween)
        {
            var newValue = (float) tween.startValue;
            var changeValueFloat = (float) tween.changeValue;
            var easeValue = tween.easeMethod(tween.currentTime, tween.duration, tween.amplitude, tween.animationCurve);
            tween.currentValue = newValue + changeValueFloat * easeValue;
        }

        public static void SetVector2(PGTweenDescr tween)
        {
            var newValue = (Vector2) tween.startValue;
            var changeValueVector2 = (Vector2) tween.changeValue;
            var easeValue = tween.easeMethod(tween.currentTime, tween.duration, tween.amplitude, tween.animationCurve);
            newValue.x += changeValueVector2.x * easeValue;
            newValue.y += changeValueVector2.y * easeValue;
            tween.currentValue = newValue;
        }

        public static void SetVector3(PGTweenDescr tween)
        {
            var newValue = (Vector3) tween.startValue;
            var changeValueVector3 = (Vector3) tween.changeValue;
            var easeValue = tween.easeMethod(tween.currentTime, tween.duration, tween.amplitude, tween.animationCurve);
            newValue.x += changeValueVector3.x * easeValue;
            newValue.y += changeValueVector3.y * easeValue;
            newValue.z += changeValueVector3.z * easeValue;
            tween.currentValue = newValue;
        }

        public static void SetVector4(PGTweenDescr tween)
        {
            var newValue = (Vector4) tween.startValue;
            var changeValueVector4 = (Vector4) tween.changeValue;
            var easeValue = tween.easeMethod(tween.currentTime, tween.duration, tween.amplitude, tween.animationCurve);
            newValue.x += changeValueVector4.x * easeValue;
            newValue.y += changeValueVector4.y * easeValue;
            newValue.z += changeValueVector4.z * easeValue;
            newValue.w += changeValueVector4.w * easeValue;
            tween.currentValue = newValue;
        }

        public static void SetColor(PGTweenDescr tween)
        {
            var newValue = (Color) tween.startValue;
            var changeValueColor = (Color) tween.changeValue;
            var easeValue = tween.easeMethod(tween.currentTime, tween.duration, tween.amplitude, tween.animationCurve);
            newValue.r += changeValueColor.r * easeValue;
            newValue.g += changeValueColor.g * easeValue;
            newValue.b += changeValueColor.b * easeValue;
            newValue.a += changeValueColor.a * easeValue;
            tween.currentValue = newValue;
        }
        
        /********************************************************************************************************************************/
        // Shake
        
        public static void SetFloatShake(PGTweenDescr tween)
        {
            float noiseValue = Mathf.PerlinNoise(tween.noiseCoordinate, 0) - 0.5f;
            float shakeValue = noiseValue * (tween.amplitude * tween.NormalizedFadeTime);
            shakeValue *= (float)tween.endValue; // EndValue is strength here
            tween.currentValue = (float)tween.startValue + shakeValue;
        }
        public static void SetVector2Shake(PGTweenDescr tween)
        {
            var noiseVector = new Vector2
            {
                x = Mathf.PerlinNoise(tween.noiseCoordinate, 0) - 0.5f,
                y = Mathf.PerlinNoise(0, tween.noiseCoordinate) - 0.5f,
            };

            var shakeValue = noiseVector * (tween.amplitude * tween.NormalizedFadeTime);
            shakeValue = Vector2.Scale(shakeValue, (Vector2) tween.endValue); // EndValue is strength here.
            tween.currentValue = (Vector2) tween.startValue + shakeValue;
        }
        public static void SetVector3Shake(PGTweenDescr tween)
        {
            var noiseVector = new Vector3
            {
                x = Mathf.PerlinNoise(tween.noiseCoordinate, 0) - 0.5f,
                y = Mathf.PerlinNoise(0, tween.noiseCoordinate) - 0.5f,
                z = Mathf.PerlinNoise(tween.noiseCoordinate, tween.noiseCoordinate) - 0.5f
            };

            var shakeValue = noiseVector * (tween.amplitude * tween.NormalizedFadeTime);
            shakeValue = Vector3.Scale(shakeValue, (Vector3) tween.endValue); // EndValue is strength here.
            tween.currentValue = (Vector3) tween.startValue + shakeValue;
        }
        public static void SetVector4Shake(PGTweenDescr tween)
        {
            var noiseVector = new Vector4
            {
                x = Mathf.PerlinNoise(tween.noiseCoordinate, 0) - 0.5f,
                y = Mathf.PerlinNoise(0, tween.noiseCoordinate) - 0.5f,
                z = Mathf.PerlinNoise(tween.noiseCoordinate, tween.noiseCoordinate) - 0.5f,
                w = Mathf.PerlinNoise(tween.noiseCoordinate, tween.noiseCoordinate) - 0.5f,
            };

            var shakeValue = noiseVector * (tween.amplitude * tween.NormalizedFadeTime);
            shakeValue = Vector4.Scale(shakeValue, (Vector4) tween.endValue); // EndValue is strength here.
            tween.currentValue = (Vector4) tween.startValue + shakeValue;
        }
        
        public static void SetColorShake(PGTweenDescr tween)
        {
            var noiseVector = new Vector3
            {
                x = Mathf.PerlinNoise(tween.noiseCoordinate, 0) - 0.5f,
                y = Mathf.PerlinNoise(0, tween.noiseCoordinate) - 0.5f,
                z = Mathf.PerlinNoise(tween.noiseCoordinate, tween.noiseCoordinate) - 0.5f
            };

            var shakeValue = noiseVector * (tween.amplitude * tween.NormalizedFadeTime);

            var colorEndValue = (Color)tween.endValue;
            shakeValue = Vector3.Scale(shakeValue, new Vector3(colorEndValue.r, colorEndValue.g, colorEndValue.b)); // EndValue is strength here.

            var colorStartValue = (Color)tween.startValue;
            var colorShake = new Color(shakeValue.x, shakeValue.y, shakeValue.z);
            tween.currentValue = colorStartValue + colorShake;
        }
    }
}