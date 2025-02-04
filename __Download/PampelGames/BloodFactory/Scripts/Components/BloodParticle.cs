// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using PampelGames.Shared.Tools;
using PampelGames.Shared.Utility;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PampelGames.BloodFactory
{
    [AddComponentMenu("Pampel Games/Blood Factory/Blood Particle")]
    [RequireComponent(typeof(ParticleSystem))]
    public class BloodParticle : MonoBehaviour
    {
        private void Reset()
        {
            particle = GetComponent<ParticleSystem>();
        }
        
        [PGHide]
        public ParticleSystem particle;

        public float delay;
        
        [PGHeader("Shader Animation", HeaderType.Small)]
        public bool animationActive = true;
        [PGClamp]
        public float duration = 1f;
        
        [Tooltip("Shader alpha value over the duration.")]
        public AnimationCurve alphaCurve = AnimationCurve.Linear(0f, 3f, 1f, 0.75f);

        [Tooltip("Shader alpha noise over the duration.")]
        public AnimationCurve noisePowerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Tooltip("Fades out gradually in UV y diration.")]
        public bool fadeOutUp;
        
        [Tooltip("Fade Out Up value over the duration.")]
        public AnimationCurve fadeOutUpCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        [PGHeader("Collision Spawn", HeaderType.Small)] 
        public bool spawnActive = true;
        public List<SpawnEffect> spawnEffects = new();
        
        
        private ParticleSystemRenderer particleSystemRenderer;
        private List<ParticleCollisionEvent> collisionEvents;
    
        private Material _material;
        private static readonly int AlphaStrength = Shader.PropertyToID(Constants.ShaderAlphaStrength);
        private static readonly int NoisePower = Shader.PropertyToID(Constants.ShaderNoisePower);
        private static readonly int Fade = Shader.PropertyToID(Constants.ShaderFade);
        
        
        private bool initialized;
        private float startTime;
        private Coroutine coroutine;
        
        /********************************************************************************************************************************/
        
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;
            
            particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
            collisionEvents = new List<ParticleCollisionEvent>();

            _material = Instantiate(particleSystemRenderer.material);
            particleSystemRenderer.material = _material;
            
            var main = particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
        
        /********************************************************************************************************************************/

        public void Execute(LayerMask collisionLayer)
        {
            Initialize();
            
            if(delay <= 0f) ExecuteInternal(collisionLayer);
            else
            {
                PGScheduler.ScheduleTime(this, delay, () =>
                {
                    ExecuteInternal(collisionLayer);
                });
            }
        }

        private void ExecuteInternal(LayerMask collisionLayer)
        {
            startTime = Time.time;
            
            var collisionModule = particle.collision;
            collisionModule.collidesWith = collisionLayer;

            if(coroutine != null) StopCoroutine(coroutine);
            if(animationActive) coroutine = StartCoroutine(UpdateBloodParticle());
            particle.Stop();
            particle.Play();
        }
        
        private IEnumerator UpdateBloodParticle()
        {
            for (;;)
            {
                var evaluationTime = CurveEvaluationTime();
                var alphaStrenght = alphaCurve.Evaluate(evaluationTime);
                var noisePower = noisePowerCurve.Evaluate(evaluationTime);

                _material.SetFloat(AlphaStrength, alphaStrenght);
                _material.SetFloat(NoisePower, noisePower);

                if (fadeOutUp)
                {
                    var fade = fadeOutUpCurve.Evaluate(evaluationTime);
                    _material.SetFloat(Fade, fade);
                }
                
                yield return null;
            }
        }

        private float CurveEvaluationTime()
        {
            var timeValue = (Time.time - startTime) / duration;
            timeValue = Mathf.Clamp(timeValue, 0f, 1f);
            return timeValue;
        }
        
        private void OnParticleSystemStopped()
        {
            if(coroutine != null) StopCoroutine(coroutine);
        }
        
        /********************************************************************************************************************************/
        
        private void OnParticleCollision(GameObject other)
        {
            particle.GetCollisionEvents(other, collisionEvents);
            
            for (int i = 0; i < collisionEvents.Count; i++)
            {
                SpawnEffects(collisionEvents[i]);
            }
        }
        
        private void SpawnEffects(ParticleCollisionEvent collisionEvent)
        {
            if (!spawnActive) return;
            
            var position = collisionEvent.intersection;
            var normal = collisionEvent.normal;

            if (position == Vector3.zero || normal == Vector3.zero) return;
            
            if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z) 
                || float.IsNaN(normal.x) || float.IsNaN(normal.y) || float.IsNaN(normal.z))
                return;
            
            var upwards = math.up();
            if (math.abs(normal.y) > math.abs(normal.x) && math.abs(normal.y) > math.abs(normal.z)) upwards = math.right();
            
            for (var i = 0; i < spawnEffects.Count; i++)
            {
                var spawnEffect = spawnEffects[i];
                if (Random.value > spawnEffect.chance) continue;
                
                var spawnedObj = Instantiate(spawnEffect.obj);

                spawnedObj.transform.SetPositionAndRotation(position, quaternion.LookRotationSafe(normal, upwards));

                ApplySpawnOffsets(spawnEffect, spawnedObj, normal, upwards);
                
                if (spawnedObj.TryGetComponent<PGIExecutable>(out var pgiExecutable)) pgiExecutable.Execute();
                
                var spawnInfo = spawnedObj.AddComponent<SpawnEffectInfo>();
                PGScheduler.ScheduleTime(spawnInfo, spawnEffect.despawnDelay, () => Destroy(spawnedObj));
            }
        } 

        private void ApplySpawnOffsets(SpawnEffect spawnEffect, GameObject spawnedObj, Vector3 normal, float3 upwards)
        {
            spawnedObj.transform.position += normal * spawnEffect.positionOffset;
            if(spawnEffect.flipRotation) spawnedObj.transform.rotation = quaternion.LookRotationSafe(-normal, upwards);
        }
    }
}