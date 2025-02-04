// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Can be attached to pooled particle systems to despawn them automatically when all particles have died.
    /// </summary>
    public class PGPoolableParticles : MonoBehaviour
    {
        public ParticleSystem _particleSystem;

        public bool poolActive = true;
        public bool autoDespawn = true;

        private void Reset()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Initialize(bool _poolActive, bool startPlay, bool _autoDespawn = true)
        {
            Initialize(GetComponentInChildren<ParticleSystem>(), _poolActive, startPlay, _autoDespawn);
        }
        public void Initialize(ParticleSystem particles, bool _poolActive, bool startPlay, bool _autoDespawn = true)
        {
            _particleSystem = particles;
            poolActive = _poolActive;
            autoDespawn = _autoDespawn;
            if(startPlay) particles.Play();
        }

        private void Start()
        {
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            if(autoDespawn)
            {
                if (poolActive) PGPool.Release(gameObject);
                else Destroy(gameObject);
            }
        }
    }
}