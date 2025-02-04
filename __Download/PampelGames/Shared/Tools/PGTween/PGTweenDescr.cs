// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Tween object that is being animated.
    /// </summary>
    public class PGTweenDescr
    {
        internal event Action<PGTweenDescr> SetValueAction;

        internal readonly InternalEvents internalEvents = new();

        internal class InternalEvents
        {
            public Action onUpdate;
            public Action onComplete;
            public Action onPause;
            public Action onResume;
            public Action onStop;
        }

        internal float duration;
        internal bool isFrameTween;
        internal float amplitude = 1.70158f;
        internal AnimationCurve animationCurve;
        internal bool completed;
        internal bool stopped;
        public bool active;
        public object startValue;
        public object endValue;
        public object changeValue;
        public object currentValue;
        public float currentTime;
        public Coroutine coroutine;

        // Shake
        internal float noiseCoordinate;
        internal float fadeInDuration;
        internal float fadeOutDuration;
        internal float frequency;
        internal float NormalizedFadeTime;
        internal float TotalDuration => fadeInDuration + duration + fadeOutDuration;


        internal PGTweenEase.EaseMethod easeMethod;

        public void SetValue()
        {
            SetValueAction(this);
        }

        public void Reset()
        {
            SetValueAction = delegate { };
            internalEvents.onUpdate = null;
            internalEvents.onComplete = null;
            internalEvents.onPause = null;
            internalEvents.onResume = null;
            internalEvents.onStop = null;
            duration = 0;
            amplitude = 1.70158f;
            animationCurve = null;
            currentTime = 0;
            noiseCoordinate = 0f;
            active = false;
            completed = false;
            easeMethod = null;
            startValue = null;
            endValue = null;
            changeValue = null;
            currentValue = null;
            coroutine = null;
        }
    }
}