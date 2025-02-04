// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using PampelGames.Shared.Tools;
using PampelGames.Shared.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PampelGames.BloodFactory
{
    /// <summary>
    ///     Using reflection to cover both URP and HDRP Decal Projector.
    /// </summary>
    public class DecalHandler : MonoBehaviour, PGIExecutable
    {
        public bool active = true;
        public Component decalComponent;
        public float lifetime = 5f;
        public float fadeIn = 0.5f;
        public float fadeOut = 1f;
        public Vector2 size = new(0.75f, 1.25f);
        public AnimationCurve sizeOverLifeTime = AnimationCurve.Constant(0f, 1f, 1f);
        public List<Material> materials = new();

        private PropertyInfo materialPropertyInfo;
        private PropertyInfo sizePropertyInfo;
        private PropertyInfo fadeFactorPropertyInfo;
        
        private PGTweenDescr sizeTween;
        private bool initialized;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;
            
            var decalType = decalComponent.GetType();
            fadeFactorPropertyInfo = decalType.GetProperty("fadeFactor");
            materialPropertyInfo = decalType.GetProperty("material");
            sizePropertyInfo = decalType.GetProperty("size");
            fadeFactorPropertyInfo!.SetValue(decalComponent, 0f, null);
        }

        public void Execute()
        {
            Initialize();
            
            var randomAngle = Random.Range(0f, 360f);
            transform.Rotate(0f, 0f, randomAngle, Space.Self);

            var randomIndex = Random.Range(0, materials.Count);
            materialPropertyInfo!.SetValue(decalComponent, materials[randomIndex], null);

            var randomSize = Random.Range(size.x, size.y);
            sizePropertyInfo!.SetValue(decalComponent, Vector3.one * randomSize);
            

            var currentSize = (Vector3) sizePropertyInfo.GetValue(decalComponent);
            sizeTween.Stop();
            sizeTween = PGTween.Move(this, 0f, 1f, lifetime);
            sizeTween.SetEase(sizeOverLifeTime);
            
            sizeTween.OnUpdate(() =>
            {
                var curveEvaluation = sizeOverLifeTime.Evaluate(sizeTween.currentTime / lifetime);
                sizePropertyInfo!.SetValue(decalComponent, currentSize * curveEvaluation);
            });
            
            
            var fadeTween = PGTween.Move(this, 0f, 1f, fadeIn);
            fadeTween.OnUpdate(() =>
            {
                fadeFactorPropertyInfo.SetValue(decalComponent, fadeTween.currentValue, null);
            });

            PGScheduler.ScheduleTime(this, lifetime - fadeOut, () =>
            {
                var fadeOutTween = PGTween.Move(this, 1f, 0f, fadeOut);
                fadeOutTween.OnUpdate(() =>
                {
                    fadeFactorPropertyInfo.SetValue(decalComponent, fadeOutTween.currentValue, null);    
                });
            });
            
        }
    }
}