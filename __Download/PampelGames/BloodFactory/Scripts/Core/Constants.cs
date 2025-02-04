// ----------------------------------------------------
// Blood Factory
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.BloodFactory
{
    public static class Constants
    {
        public const string GlobalSettings = "GlobalSettings";
        public const string DefaultReferences = "BloodFactoryDefaultReferences";

#if UNITY_EDITOR
        public const string DocumentationURL = "https://docs.google.com/document/d/1pjHgMf_qalES1EFhu4fYAMC6Nwuzj5Opd4is38URmGc/edit?usp=sharing";
#endif
        
        // BloodParticle
        public const string ShaderAlphaStrength = "_AlphaStrength";
        public const string ShaderNoisePower = "_NoisePower";
        public const string ShaderFade = "_Fade";
        public const string ShaderDistortion = "_Distortion";
        
    }
}
