// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.BloodFactory
{
    public static class Enums
    {
        public enum SpawnType
        {
            ParticleSystem,
            Gameobject
        }

        public enum SpawnPositionFountain
        {
            AttachStart,
            AttachEnd,
            AttachCollision,
            SpawnCollision,
            MeshRenderer
        }
        
        public enum SpawnPositionSplash
        {
            Spawn,
            Collision
        }

        public enum Shape
        {
            Cone,
            Sphere,
            Mesh
        }
        
    }
    

}