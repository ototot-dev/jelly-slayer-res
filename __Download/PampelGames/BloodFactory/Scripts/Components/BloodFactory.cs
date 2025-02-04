// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using PampelGames.Shared.Tools;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.BloodFactory
{
    [AddComponentMenu("Pampel Games/Blood Factory/Blood Factory")]
    public class BloodFactory : MonoBehaviour, PGIExecutable
    {
        private void Reset()
        {
            collisionLayer |= 1 << LayerMask.NameToLayer("Default");
        }

        [Tooltip("Overrides collision layers of the referenced blood particles and particles.")]
        public LayerMask collisionLayer;

        public List<BloodParticle> bloodParticles = new();
        public List<ParticleSystem> particles = new();
        

        /********************************************************************************************************************************/

        public void Execute()
        {
            for (var i = 0; i < bloodParticles.Count; i++) bloodParticles[i].Execute(collisionLayer);

            for (var i = 0; i < particles.Count; i++)
            {
                var particle = Instantiate(particles[i]);
                particle.transform.SetPositionAndRotation(transform.position, transform.rotation);
                var collisionModule = particle.collision;
                collisionModule.collidesWith = collisionLayer;
                var pgParticle = particle.gameObject.AddComponent<PGPoolableParticles>();
                pgParticle.Initialize(particle, false, true);
            }
        }
    }
}