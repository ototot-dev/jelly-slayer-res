using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Reflection;
using System;

namespace G1UP
{
    [ExecuteInEditMode]
    public class DepthCamera_Ground : MonoBehaviour
    {
        public Transform targetTransform;
        Transform prevTargetTranform = null;

        public bool renderChildren;
        public int shadowIndex = 0;
        [Range(0, 1)]
        public float shadowOffset = 1.0f;

        public bool followTarget;
        private Vector3 posOffset = Vector3.zero;
        private Vector3 prevPosition = Vector3.zero;

        public LayerMask layerMask;
        private LayerMask prevMask;
        private Camera cam = null;

        private Dictionary<string, int> layerDict = new Dictionary<string, int>();

        public enum TexSize : int
        {
            s1024 = 1024,
            s512 = 512,
            s256 = 256,
            s128 = 128,
        }

        public TexSize texWidth = TexSize.s128;
        private int prevWidth;
        public TexSize texHeight = TexSize.s128;
        private int prevHeight;

        void InitTargetObject()
        {
            prevMask = layerMask;
            prevWidth = (int)texWidth;
            prevHeight = (int)texHeight;

            cam = this.GetComponent<Camera>();
            if (cam == null) return;
            cam.targetTexture = new RenderTexture(
                (int)texWidth, (int)texHeight,
                16, RenderTextureFormat.Depth);
            cam.clearStencilAfterLightingPass = true;
            // cam.clearFlags = CameraClearFlags.Skybox;

            if (targetTransform == null) return;
            prevPosition = targetTransform.position;
            posOffset = targetTransform.position - prevPosition;
            UpdateCameraRT();
        }

        void OnEnable()
        {
            InitTargetObject();
            // #if UNITY_EDITOR
            //         var egu = typeof(EditorGUIUtility);
            //         var icon = EditorGUIUtility.IconContent("sv_label_0").image as Texture2D;
            //         // icon = Texture2D 
            //         icon = Resources.Load("UnlitShadowIcon/UnlitShadowIconForGround") as Texture2D;
            //         var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            //         var args = new object[] { this.gameObject, icon };
            //         var setIcon = egu.GetMethod("SetIconForObject", flags, null,
            //             new Type[] { typeof(UnityEngine.GameObject), typeof(Texture2D) }, null);
            //         setIcon.Invoke(null, args);
            // #endif
        }

        void OnDisable()
        {
            // if (cbuffer == null) return;
            if (cam == null) return;
            if (targetTransform == null) return;
            var targetRenderer = targetTransform.GetComponent<Renderer>();
            ResetProj(targetRenderer);

            foreach (Transform child in targetTransform)
            {
                var renderers = child.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                    ResetProj(renderer);
            }
            // #if UNITY_EDITOR
            //         var egu = typeof(EditorGUIUtility);
            //         var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            //         var args = new object[] { this.gameObject, null };
            //         var setIcon = egu.GetMethod("SetIconForObject", flags, null,
            //             new Type[] { typeof(UnityEngine.GameObject), typeof(Texture2D) }, null);
            //         setIcon.Invoke(null, args);
            // #endif
        }

        void OnStart()
        {
            //Debug.Log("UpdateBuffer");

        }

        void ResetProj(Renderer renderer)
        {
            if (renderer == null || renderer.sharedMaterial == null) return;
            renderer.sharedMaterial.SetFloat("_ShadowOffset" + shadowIndex.ToString(), 0.0f);
            renderer.sharedMaterial.SetMatrix("_ShadowProj" + shadowIndex.ToString(), Matrix4x4.identity);
            renderer.sharedMaterial.SetTexture("_ShadowTex" + shadowIndex.ToString(), null);
            // Debug.Log("ResetShadowRT");
        }

        void UpdateProj(Renderer renderer)
        {
            if (renderer == null ||
                renderer.sharedMaterial == null ||
                !renderer.isVisible) return;
            var layer = 1 << renderer.gameObject.layer;
            if (layer != (layer & layerMask))
            {
                ResetProj(renderer);
                return;
            }
            Matrix4x4 proj =
                GL.GetGPUProjectionMatrix(cam.projectionMatrix, false);
            Matrix4x4 lightProj = proj * cam.worldToCameraMatrix;
            renderer.sharedMaterial.SetFloat("_ShadowOffset" + shadowIndex.ToString(), shadowOffset);
            renderer.sharedMaterial.SetMatrix("_ShadowProj" + shadowIndex.ToString(), lightProj);
            renderer.sharedMaterial.SetTexture("_ShadowTex" + shadowIndex.ToString(), cam.targetTexture);
            // renderer.sharedMaterial.SetTexture("_ShadowTex" + shadowIndex.ToString(), shadowRT);
            // Debug.Log("UpdateShadowRT" + shadowIndex);
        }

        void UpdateCameraRT()
        {
            if (targetTransform == null) return;
            if (followTarget)
            {
                posOffset = targetTransform.position - prevPosition;
                transform.position += posOffset;
                prevPosition = targetTransform.position;
            }

            if (prevTargetTranform != targetTransform)
            {
                if (prevTargetTranform != null)
                {
                    var prevRenderer = prevTargetTranform.GetComponent<Renderer>();
                    ResetProj(prevRenderer);

                    foreach (Transform child in prevTargetTranform)
                    {
                        var renderers = child.GetComponentsInChildren<Renderer>();
                        foreach (Renderer renderer in renderers)
                            ResetProj(renderer);
                    }
                }
                prevTargetTranform = targetTransform;
            }

            if (prevMask.value != layerMask.value)
            {
                prevMask = layerMask;
                cam.cullingMask = layerMask;
            }

            bool updateTexsize = false;
            if (prevWidth != (int)texWidth)
            {
                prevWidth = (int)texWidth;
                updateTexsize = true;
            }

            if (prevHeight != (int)texHeight)
            {
                prevHeight = (int)texHeight;
                updateTexsize = true;
            }

            if (updateTexsize)
            {
                cam.targetTexture = new RenderTexture(
                    (int)texWidth, (int)texHeight,
                    16, RenderTextureFormat.Depth);
            }

            var targetRenderer = targetTransform.GetComponent<Renderer>();
            UpdateProj(targetRenderer);

            foreach (Transform child in targetTransform)
            {
                var renderers = child.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                    UpdateProj(renderer);
            }
        }

        // void Update()
        void Update()
        {
            UpdateCameraRT();
        }
    }
}