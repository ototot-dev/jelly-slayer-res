// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace PampelGames.Shared.Utility
{
    public static class PGInformationUtility
    {

        public static string GetTimeString()
        {
            var time = DateTime.Now.TimeOfDay;
            return time.ToString(@"hh\:mm\:ss\.fff");
        }
        
        /// <summary>
        ///     Checks for the render pipeline that is used in the project.
        /// </summary>
        public static PGEnums.RenderPipelineEnum GetRenderPipeline()
        {
            var currentPipeline = GraphicsSettings.defaultRenderPipeline;
            if (currentPipeline == null)
                return PGEnums.RenderPipelineEnum.BuiltIn;
            if (currentPipeline.GetType().Name.Contains("UniversalRenderPipelineAsset"))
                return PGEnums.RenderPipelineEnum.URP;
            if (currentPipeline.GetType().Name.Contains("HighDefinitionRenderPipelineAsset") || 
                currentPipeline.GetType().Name.Contains("HDRenderPipelineAsset"))
                return PGEnums.RenderPipelineEnum.HDRP;
            return PGEnums.RenderPipelineEnum.BuiltIn;
        }
        
        /// <summary>
        ///     Get the year of the Unity version being used. 
        /// </summary>
        public static string GetUnityVersionYear()
        {
            var unityVersion = CutStringAfter(Application.unityVersion,".", true).Trim();
            return unityVersion;
        }
        private static string CutStringAfter(string value, string cutString, bool removeCutstring)
        {
            var index = value.IndexOf(cutString, StringComparison.Ordinal);
            if (index == -1) return value;
            if(!removeCutstring) return value.Substring(0, index + cutString.Length);
            return value.Substring(0, index);

        }

        /// <summary>
        ///     Create a primitive sphere to get visual information about a position.
        /// </summary>
        public static GameObject CreateSphere(Vector3 position, float scaleMultiplier = 0.1f)
        {
            string sphereName = "Sphere";
            if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z))
            {
                sphereName = "Position IsNAN";
                position = Vector3.zero;
            }
            if (float.IsInfinity(position.x) || float.IsInfinity(position.y) || float.IsInfinity(position.z))
            {
                sphereName = "Position IsInfinity";
                position = Vector3.zero;
            }
            GameObject parent = GameObject.Find("SphereParent");
            if (parent == null) parent = new GameObject("SphereParent");
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = position;
            sphere.transform.localScale *= scaleMultiplier;
            sphere.transform.parent = parent.transform;
            if (sphere.TryGetComponent<Collider>(out var collider))
                collider.enabled = false;
            sphere.name = sphereName;
            return sphere;
        }
        public static GameObject CreateSphere(Vector3 position, string name)
        {
            var sphere = CreateSphere(position);
            sphere.name = name;
            return sphere;
        }
        
        public static List<GameObject> CreateSphereBounds(Bounds bounds, string name = "")
        {
            List<GameObject> spheres = new List<GameObject>();
            
            Vector3[] corners = new Vector3[8];
            corners[0] = bounds.min; // near bottom-left
            corners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z); // near bottom-right
            corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z); // near top-left
            corners[3] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z); // near top-right

            corners[4] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z); // far bottom-left
            corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z); // far bottom-right
            corners[6] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z); // far top-left
            corners[7] = bounds.max; // far top-right
            
            spheres.Add(CreateSphere(bounds.center, "Center _" + name));
            for (int i = 0; i < corners.Length; i++)
            {
                CreateSphere(corners[i], i.ToString() + "_"+ name);
            }

            return spheres;
        }

        public static void RemoveSpheres()
        {
            if(Application.isPlaying) Object.Destroy(GameObject.Find("SphereParent"));
            else Object.DestroyImmediate(GameObject.Find("SphereParent"));
        }

        public static GameObject CreateQuad(Vector3 position, Vector3 planeNormal)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.transform.rotation = Quaternion.LookRotation(planeNormal);
            quad.transform.position = position;
            return quad;
        }
    }
}
