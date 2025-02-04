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
    public class MeshDecalHandler : MonoBehaviour, PGIExecutable
    {
        public float distortion = -1f;
        public float duration = 5f;

        [Tooltip("Distortion being applied over the normalized duration.")]
        public AnimationCurve distortionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public List<MeshDecalHandler> drops = new();
        
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private Mesh mesh;
        private Material _material;

        private static readonly int Distortion = Shader.PropertyToID(Constants.ShaderDistortion);

        private bool initialized;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;
            
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
        }

        public void Execute()
        {
            Initialize();

            PGScheduler.ScheduleFrames(this, 1, ExecuteInternal);
        }

        private void ExecuteInternal()
        {
            mesh = Instantiate(meshFilter.mesh);
            _material = Instantiate(meshRenderer.material);

            meshRenderer.material = _material;
            meshFilter.mesh = mesh;

            var tween = PGTween.Move(this, 0f, distortion, duration);
            tween.SetEase(distortionCurve);
            tween.OnUpdate(() => { _material.SetFloat(Distortion, (float) tween.currentValue); });

            for (int i = 0; i < drops.Count; i++)
            {
                drops[i].Execute();
            }
        }
    }
}