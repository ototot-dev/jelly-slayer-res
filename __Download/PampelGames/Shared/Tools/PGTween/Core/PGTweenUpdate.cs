// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Updates <see cref="PGTweenDescr" />s.
    /// </summary>
    internal static class PGTweenUpdate
    {
        
        internal static IEnumerator _TweenUpdate(PGTweenDescr tween)
        {
            for (;;)
            {
                if (tween.stopped) yield break;

                if (!tween.active)
                {
                    yield return null;
                    continue;
                }
                if (tween.isFrameTween)
                {
                    tween.currentTime += 1;
                    if (tween.currentTime > tween.duration)
                    {
                        tween.currentTime = tween.duration;
                        tween.completed = true;
                    }   
                }
                else
                {
                    tween.currentTime += Time.deltaTime;
                    if (tween.currentTime >= tween.duration)
                    {
                        tween.currentTime = tween.duration;
                        tween.completed = true;
                    }    
                }

                if (tween.active)
                    tween.active = !tween.completed;

                tween.SetValue();
                tween.internalEvents.onUpdate?.Invoke();

                if (!tween.completed) yield return null;
                else
                {
                    tween.internalEvents.onComplete?.Invoke();
                    yield break;
                }
            }
        }
        
        internal static IEnumerator _TweenUpdateShake(PGTweenDescr tween)
        {
            for (;;)
            {
                if (tween.stopped) yield break;

                if (!tween.active)
                {
                    yield return null;
                    continue;
                }

                tween.currentTime += Time.deltaTime;
                if (tween.currentTime >= tween.TotalDuration)
                {
                    tween.currentTime = tween.TotalDuration;
                    tween.completed = true;
                }
                else if (tween.currentTime < tween.fadeInDuration)
                {
                    tween.NormalizedFadeTime = tween.currentTime / tween.fadeInDuration;
                }
                else if (tween.currentTime < tween.fadeInDuration + tween.duration)
                {
                    tween.NormalizedFadeTime = 1;
                }
                else
                {
                    tween.NormalizedFadeTime = 1 - (tween.currentTime - tween.fadeInDuration - tween.duration) / tween.fadeOutDuration;
                }

                tween.noiseCoordinate += Time.deltaTime * tween.frequency * tween.NormalizedFadeTime;
                

                if (tween.active)
                    tween.active = !tween.completed;

                tween.SetValue();
                tween.internalEvents.onUpdate?.Invoke();

                if (!tween.completed) yield return null;
                else
                {
                    tween.internalEvents.onComplete?.Invoke();
                    yield break;
                }
            }
        }
        
    }
}