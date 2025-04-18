﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace G1UP
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    // [ImageEffectAllowedInSceneView]
    public class SceneViewCamera : MonoBehaviour
    {
        // private new Camera camera = null;

        // // Start is called before the first frame update
        // void Start()
        // {
        //     camera = this.GetComponent<Camera>();
        //     camera.depthTextureMode = DepthTextureMode.Depth;
        // }

        // Update is called once per frame
        // void OnDrawGizmos()
        // {
        //     if (camera != null && UnityEditor.SceneView.lastActiveSceneView.camera != null)
        //     {
        //         camera.transform.SetPositionAndRotation(
        //             SceneView.lastActiveSceneView.camera.transform.position,
        //             SceneView.lastActiveSceneView.camera.transform.rotation);
        //     }
        // }

        public bool EnableInEditor = true;

        void OnEnable()
        {
            var camera = this.GetComponent<Camera>();
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
#if UNITY_EDITOR
            if (EnableInEditor)
            {
                if (UnityEditor.SceneView.currentDrawingSceneView != null &&
                    UnityEditor.SceneView.currentDrawingSceneView.camera != null)
                    UnityEditor.SceneView.currentDrawingSceneView.camera.depthTextureMode = DepthTextureMode.Depth;
                if (UnityEditor.SceneView.lastActiveSceneView != null &&
                    UnityEditor.SceneView.lastActiveSceneView.camera != null)
                    UnityEditor.SceneView.lastActiveSceneView.camera.depthTextureMode = DepthTextureMode.Depth;
            }
#endif
        }
        void OnDisable()
        {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
#if UNITY_EDITOR
            if (UnityEditor.SceneView.currentDrawingSceneView != null &&
                UnityEditor.SceneView.currentDrawingSceneView.camera != null)
                UnityEditor.SceneView.currentDrawingSceneView.camera.depthTextureMode = DepthTextureMode.None;
#endif
        }

        void Update()
        {
#if UNITY_EDITOR
            if (EnableInEditor)
            {
                if (UnityEditor.SceneView.currentDrawingSceneView != null &&
                    UnityEditor.SceneView.currentDrawingSceneView.camera != null)
                    UnityEditor.SceneView.currentDrawingSceneView.camera.depthTextureMode = DepthTextureMode.Depth;
                if (UnityEditor.SceneView.lastActiveSceneView != null &&
                    UnityEditor.SceneView.lastActiveSceneView.camera != null)
                    UnityEditor.SceneView.lastActiveSceneView.camera.depthTextureMode = DepthTextureMode.Depth;
                var view1 = UnityEditor.SceneView.lastActiveSceneView;
                var view2 = UnityEditor.SceneView.sceneViews;
            }
            else
            {
                if (UnityEditor.SceneView.currentDrawingSceneView != null &&
                    UnityEditor.SceneView.currentDrawingSceneView.camera != null)
                    UnityEditor.SceneView.currentDrawingSceneView.camera.depthTextureMode = DepthTextureMode.None;
                if (UnityEditor.SceneView.lastActiveSceneView != null &&
                    UnityEditor.SceneView.lastActiveSceneView.camera != null)
                    UnityEditor.SceneView.lastActiveSceneView.camera.depthTextureMode = DepthTextureMode.None;
            }
#endif

        }
    }
}