// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.BloodFactory
{
    [Serializable]
    public class SpawnEffect
    {
        [Tooltip("The object to spawn upon collision.\n\n" +
                 "Components that inherit from 'PGIExecutable' will be automatically executed.")]
        public GameObject obj;
        [Tooltip("Chance of a spawn on collision, normalized from 0 to 1.")]
        [Min(0)]
        public float chance = 1f;
        [Tooltip("Offset in collision normal direction.")]
        public float positionOffset;
        [Tooltip("The spawned object will be destroyed after the delay.")]
        [Min(0)] public float despawnDelay = 5f;

        public bool flipRotation;
    }
}