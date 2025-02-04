// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PampelGames.Shared.Utility
{
    public static class PGMeshUtility
    {
        public static void RecalculateMeshData(Mesh mesh, bool normals)
        {
            if (normals) mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds(); // Should be done by SetSubMesh, but isn't because of bug.
            // See here: https://issuetracker.unity3d.com/issues/bounds-are-not-applied-to-mesh-when-using-mesh-dot-applyanddisposewritablemeshdata
        }

        /********************************************************************************************************************************/

        /// <summary>
        ///     Combines an array of meshes into one new single mesh.
        /// </summary>
        public static Mesh CombineMeshes(Mesh[] meshes, bool mergeSubmeshes)
        {
            return CombineMeshesInternal(meshes, mergeSubmeshes);
        }

        /// <summary>
        ///     Requires equal materials and meshes count.
        ///     Combines and packs an array of meshes into one new single mesh, with the corresponding combined materials.
        /// </summary>
        public static void CombineAndPackMeshes(List<Material> materials, List<Mesh> meshes,
            out Material[] combinedMaterials, out Mesh combinedMesh)
        {
            CombineAndPackMeshesInternal(materials, meshes, out combinedMaterials, out combinedMesh);
        }

        /// <summary>
        ///     Combines multiple mesh instances into a single mesh manually.
        ///     This works similar to Unity's mesh.CombineMeshes() method, but works with mal-formed mesh
        ///     and handles sub-meshes properly.
        ///     For best performance, the first mesh should have the highest vertex count.
        /// </summary>
        public static Mesh CombineMeshesManually(CombineInstance[] combineInstances)
        {
            return CombineMeshesManuallyInternal(combineInstances);
        }

        /// <summary>
        ///     Combines multiple mesh instances into a single mesh manually.
        ///     This works similar to Unity's mesh.CombineMeshes() method, but works with mal-formed mesh
        ///     and handles sub-meshes properly.
        ///     For best performance, the first mesh should have the highest vertex count.
        /// </summary>
        public static Mesh CombineMeshesManually(Mesh[] meshes)
        {
            return CombineMeshesManuallyInternal(meshes);
        }

        /// <summary>
        ///     Get all indexes that are within the radius from the pivot point.
        /// </summary>
        /// <param name="indexesInRadius">All indexes within the radius.</param>
        /// <param name="closestIndex">The closest index to the pivot point.</param>
        public static void GetIndexesInRadius(Mesh mesh, Vector3 pivot, float radius, out int[] indexesInRadius, out int closestIndex)
        {
            GetIndexesInRadiusInternal(mesh, pivot, radius, out indexesInRadius, out closestIndex);
        }

        /// <summary>
        ///     Equalizes the normals of vertices that share the same position in the mesh.
        /// </summary>
        public static void EqualizeNormals(Mesh mesh)
        {
            EqualizeNormalsInternal(mesh);
        }

        /* Bounds ***********************************************************************************************************************/

        /// <summary>
        ///     Adapts a capsule collider to the mesh bounds.
        /// </summary>
        public static void MatchCapsuleColliderToBounds(Mesh mesh, CapsuleCollider capsuleCollider)
        {
            MatchCapsuleColliderToBoundsInternal(mesh, capsuleCollider);
        }


        /// <summary>
        ///     Transforms the bounds of the given CombineInstance from local space to world space.
        /// </summary>
        public static Bounds GetBoundsWorldSpace(CombineInstance combineInstance)
        {
            var boundsLocal = combineInstance.mesh.bounds;
            var matrix = combineInstance.transform;
            return TransformBoundsToWorldInternal(boundsLocal, matrix);
        }


        /* Vertices & UVs ****************************************************************************************************************/

        /// <summary>
        ///     Creates a new list from the current UVs.
        ///     Remember to use mesh.SetUVs(UVs); when done.
        /// </summary>
        public static List<Vector2> CreateUVList(Mesh mesh, int channel = 0)
        {
            var UVs = new List<Vector2>();
            mesh.GetUVs(channel, UVs);
            return UVs;
        }

        /// <summary>
        ///     Creates a new list from the current vertices which can be used for the transformation methods provided below.
        ///     Remember to use mesh.SetVertices(vertices); when done.
        /// </summary>
        public static List<Vector3> CreateVertexList(Mesh mesh)
        {
            var vertices = new List<Vector3>();
            mesh.GetVertices(vertices);
            return vertices;
        }

        /// <summary>
        ///     Translates all vertices.
        /// </summary>
        /// <param name="translation">Delta position of the vertices.</param>
        public static void PGTranslateVertices(List<Vector3> vertices, Vector3 translation)
        {
            for (var i = 0; i < vertices.Count; i++) vertices[i] += translation;
        }

        /// <summary>
        ///     Rotates all vertices around a pivot point.
        /// </summary>
        /// <param name="pivot">The pivot point around which the rotation occurs. For example mesh.bounds.center</param>
        public static void PGRotateVertices(List<Vector3> vertices, Quaternion rotation, Vector3 pivot)
        {
            for (var i = 0; i < vertices.Count; i++) vertices[i] = rotation * (vertices[i] - pivot) + pivot;
        }

        /// <summary>
        ///     Scales the vertices of the mesh by the specified scaleFactor around the given center point.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply to the vertices.</param>
        /// <param name="center">The center point around which the scaling is performed.</param>
        public static void PGScaleVertices(List<Vector3> vertices, Vector3 scaleFactor, Vector3 center)
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                var scaledPosition = vertices[i] - center;
                scaledPosition = new Vector3(scaledPosition.x * scaleFactor.x, scaledPosition.y * scaleFactor.y, scaledPosition.z * scaleFactor.z);
                vertices[i] = scaledPosition + center;
            }
        }

        public static void FlipNormals(Mesh mesh)
        {
            FlipNormalsInternal(mesh);
        }


        /* Combine Instances ************************************************************************************************************/

        public static void PGTranslateCombine(this ref CombineInstance combineInstance, Vector3 translation)
        {
            if (translation == Vector3.zero) return;
            var translationMatrix = Matrix4x4.Translate(translation);
            combineInstance.transform = translationMatrix * combineInstance.transform;
        }

        public static void PGSetPositionCombine(this ref CombineInstance combineInstance, Vector3 position)
        {
            var matrix4x4 = combineInstance.transform;
            var rotation = matrix4x4.rotation;
            var scale = matrix4x4.lossyScale;
            combineInstance.transform = Matrix4x4.TRS(position, rotation, scale);
        }

        public static void PGRotateCombine(this ref CombineInstance combineInstance, Vector3 pivot, Vector3 eulerRotation)
        {
            if (eulerRotation == Vector3.zero) return;
            PGRotateCombine(ref combineInstance, pivot, Quaternion.Euler(eulerRotation));
        }
        
        public static void PGRotateCombine(this ref CombineInstance combineInstance, Vector3 pivot, Quaternion rotation)
        {
            if (rotation == Quaternion.identity) return;
            var translationToOrigin = Matrix4x4.Translate(-pivot);
            var rotationMatrix = Matrix4x4.Rotate(rotation);
            var translationBackToPivot = Matrix4x4.Translate(pivot);
            var transform = translationBackToPivot * rotationMatrix * translationToOrigin;
            combineInstance.transform = transform * combineInstance.transform;
        }

        public static void PGSetRotationCombine(this ref CombineInstance combineInstance, Quaternion rotation)
        {
            var matrix4x4 = combineInstance.transform;
            var position = matrix4x4.GetPosition();
            var scale = matrix4x4.lossyScale;
            combineInstance.transform = Matrix4x4.TRS(position, rotation, scale);
        }

        public static void PGSetRotationCombine(this ref CombineInstance combineInstance, Vector3 eulerRotation)
        {
            var matrix4x4 = combineInstance.transform;
            var position = matrix4x4.GetPosition();
            var scale = matrix4x4.lossyScale;
            var rotation = Quaternion.Euler(eulerRotation);
            combineInstance.transform = Matrix4x4.TRS(position, rotation, scale);
        }

        public static void PGScaleCombine(this ref CombineInstance combineInstance, Vector3 pivot, float size)
        {
            combineInstance.PGScaleCombine(pivot, new Vector3(size, size, size));
        }

        public static void PGScaleCombine(this ref CombineInstance combineInstance, Vector3 pivot, Vector3 size)
        {
            if (size == Vector3.one) return;
            var translationToOrigin = Matrix4x4.Translate(-pivot);
            var scaleMatrix = Matrix4x4.Scale(size);
            var translationBackToPivot = Matrix4x4.Translate(pivot);
            var transform = translationBackToPivot * scaleMatrix * translationToOrigin;
            combineInstance.transform = transform * combineInstance.transform;
        }
        
        public static void PGScaleCombineLocal(this ref CombineInstance combineInstance, Vector3 pivot, float size)
        {
            combineInstance.PGScaleCombineLocal(pivot, new Vector3(size, size, size));
        }
        public static void PGScaleCombineLocal(this ref CombineInstance combineInstance, Vector3 pivot, Vector3 localSize)
        {
            if (localSize == Vector3.one) return;
            var localScaleMatrix = Matrix4x4.Scale(localSize);
            var localTransform = combineInstance.transform * localScaleMatrix;
            var translationToOrigin = Matrix4x4.Translate(-pivot);
            var translationBackToPivot = Matrix4x4.Translate(pivot);
            var worldTransform = translationBackToPivot * translationToOrigin;
            combineInstance.transform = worldTransform * localTransform;
        }

        public static void PGSetScaleCombine(this ref CombineInstance combineInstance, Vector3 pivot, float size)
        {
            combineInstance.PGSetScaleCombine(pivot, new Vector3(size, size, size));
        }

        public static void PGSetScaleCombine(this ref CombineInstance combineInstance, Vector3 pivot, Vector3 size)
        {
            var matrix4x4 = combineInstance.transform;
            var position = matrix4x4.GetPosition();
            var rotation = matrix4x4.rotation;
            combineInstance.transform = Matrix4x4.TRS(position, rotation, Vector3.one);
            combineInstance.PGScaleCombine(pivot, size);
        }


        /********************************************************************************************************************************/

        /// <summary>
        ///     Combines the Mesh Renderers into a new one.
        /// </summary>
        /// <param name="rootObj">GameObject to attach the renderer to.</param>
        /// <param name="objects">List of GameObjects with MeshFilter and MeshRenderer to be combined.</param>
        /// <param name="mergeSubMeshes">If true, only one resulting sub-mesh will be created.</param>
        /// <param name="saveMesh">Save the Mesh into the project (Editor only).</param>
        public static bool CombineMeshes(GameObject rootObj, List<GameObject> objects, bool mergeSubMeshes, bool saveMesh)
        {
            if (mergeSubMeshes) return CombineMeshesMergeInternal(rootObj, objects, saveMesh);
            return CombineMeshesInternal(rootObj, objects, saveMesh);
        }

        /********************************************************************************************************************************/
        /********************************************************************************************************************************/


        private static Mesh CombineMeshesInternal(Mesh[] meshes, bool mergeSubmeshes)
        {
            var combinedInstances = new CombineInstance[meshes.Length];
            for (var i = 0; i < combinedInstances.Length; i++) combinedInstances[i].transform = Matrix4x4.identity;
            for (var i = 0; i < combinedInstances.Length; i++) combinedInstances[i].mesh = meshes[i];

            var combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combinedInstances, mergeSubmeshes, true);
            return combinedMesh;
        }

        private static void CombineAndPackMeshesInternal(List<Material> materials, List<Mesh> meshes,
            out Material[] combinedMaterials, out Mesh combinedMesh)
        {
            GroupByMaterialName(materials, meshes,
                out var orderedMaterials, out var orderedMeshes);

            var combinedMeshes = new List<Mesh>();
            for (var i = 0; i < orderedMaterials.Count; i++) combinedMeshes.Add(CombineMeshes(orderedMeshes[i].ToArray(), true));

            combinedMesh = CombineMeshes(combinedMeshes.ToArray(), false);
            combinedMaterials = orderedMaterials.ToArray();
        }

        private static void GroupByMaterialName(List<Material> materials, List<Mesh> meshes, out List<Material> orderedMaterials,
            out List<List<Mesh>> orderedMeshes)
        {
            var materialNames = new List<string>();

            orderedMaterials = new List<Material>();
            orderedMeshes = new List<List<Mesh>>();

            for (var i = 0; i < materials.Count; i++)
            {
                var materialName = materials[i].name;
                var index = materialNames.IndexOf(materialName);
                if (index < 0)
                {
                    materialNames.Add(materialName);
                    orderedMaterials.Add(materials[i]);
                    orderedMeshes.Add(new List<Mesh>());
                    orderedMeshes[^1].Add(meshes[i]);
                }
                else
                {
                    orderedMeshes[index].Add(meshes[i]);
                }
            }
        }

        private static Mesh CombineMeshesManuallyInternal(CombineInstance[] combineInstances)
        {
            var finalMesh = new Mesh();

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv0 = new List<Vector2>();
            var uv1 = new List<Vector2>();
            var tangents = new List<Vector4>();
            var colors = new List<Color>();

            var subMeshTriangles = new List<List<int>>();

            var vertexOffset = 0;

            foreach (var ci in combineInstances)
            {
                var mesh = ci.mesh;
                var transform = ci.transform;

                foreach (var vertex in mesh.vertices)
                    vertices.Add(transform.MultiplyPoint3x4(vertex));

                foreach (var normal in mesh.normals)
                    normals.Add(transform.MultiplyVector(normal));

                foreach (var tangent in mesh.tangents)
                {
                    Vector4 newTangent = transform.MultiplyPoint3x4(tangent);
                    newTangent.w = tangent.w;
                    tangents.Add(newTangent);
                }

                uv0.AddRange(mesh.uv);

                if (mesh.uv2.Length > 0) uv1.AddRange(mesh.uv2);

                colors.AddRange(mesh.colors);

                for (var i = 0; i < mesh.subMeshCount; i++)
                {
                    var triangles = new List<int>();
                    mesh.GetTriangles(triangles, i);
                    for (var j = 0; j < triangles.Count; j++)
                        triangles[j] += vertexOffset;

                    subMeshTriangles.Add(triangles);
                }

                vertexOffset += mesh.vertexCount;
            }

            finalMesh.SetVertices(vertices);
            finalMesh.SetNormals(normals);
            finalMesh.SetUVs(0, uv0);
            if (uv1.Count > 0) finalMesh.SetUVs(1, uv1);
            finalMesh.SetTangents(tangents);
            if (vertices.Count == colors.Count) finalMesh.SetColors(colors);
            finalMesh.subMeshCount = subMeshTriangles.Count;
            for (var i = 0; i < subMeshTriangles.Count; i++) finalMesh.SetTriangles(subMeshTriangles[i], i);

            finalMesh.RecalculateBounds();

            return finalMesh;
        }

        private static Mesh CombineMeshesManuallyInternal(Mesh[] meshes)
        {
            var finalMesh = new Mesh();

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv0 = new List<Vector2>();
            var uv1 = new List<Vector2>();
            var tangents = new List<Vector4>();
            var colors = new List<Color>();

            var subMeshTriangles = new List<List<int>>();

            var vertexOffset = 0;

            foreach (var mesh in meshes)
            {
                vertices.AddRange(mesh.vertices);
                normals.AddRange(mesh.normals);
                tangents.AddRange(mesh.tangents);
                uv0.AddRange(mesh.uv);

                if (mesh.uv2.Length > 0) uv1.AddRange(mesh.uv2);

                colors.AddRange(mesh.colors);

                for (var i = 0; i < mesh.subMeshCount; i++)
                {
                    var triangles = new List<int>();
                    mesh.GetTriangles(triangles, i);
                    if (vertexOffset != 0)
                        for (var j = 0; j < triangles.Count; j++)
                            triangles[j] += vertexOffset;
                    subMeshTriangles.Add(triangles);
                }

                vertexOffset += mesh.vertexCount;
            }

            finalMesh.SetVertices(vertices);
            finalMesh.SetNormals(normals);
            finalMesh.SetUVs(0, uv0);
            if (uv1.Count > 0) finalMesh.SetUVs(1, uv1);
            finalMesh.SetTangents(tangents);
            if (vertices.Count == colors.Count) finalMesh.SetColors(colors);
            finalMesh.subMeshCount = subMeshTriangles.Count;
            for (var i = 0; i < subMeshTriangles.Count; i++) finalMesh.SetTriangles(subMeshTriangles[i], i);

            finalMesh.RecalculateBounds();

            return finalMesh;
        }

        private static void GetIndexesInRadiusInternal(Mesh mesh, Vector3 pivot, float radius, out int[] indexesInRadius, out int closestIndex)
        {
            var indexesWithinRadiusSet = new HashSet<int>();
            var closestDistanceSqr = Mathf.Infinity;
            closestIndex = -1;

            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            for (var i = 0; i < triangles.Length; i += 3)
            {
                var vertexIndex1 = triangles[i];
                var vertexIndex2 = triangles[i + 1];
                var vertexIndex3 = triangles[i + 2];

                var vertex1 = vertices[vertexIndex1];
                var vertex2 = vertices[vertexIndex2];
                var vertex3 = vertices[vertexIndex3];

                var distanceSqr1 = (vertex1 - pivot).sqrMagnitude;
                var distanceSqr2 = (vertex2 - pivot).sqrMagnitude;
                var distanceSqr3 = (vertex3 - pivot).sqrMagnitude;

                if (distanceSqr1 <= radius * radius)
                {
                    indexesWithinRadiusSet.Add(vertexIndex1);
                    if (distanceSqr1 < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr1;
                        closestIndex = vertexIndex1;
                    }
                }

                if (distanceSqr2 <= radius * radius)
                {
                    indexesWithinRadiusSet.Add(vertexIndex2);
                    if (distanceSqr2 < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr2;
                        closestIndex = vertexIndex2;
                    }
                }

                if (distanceSqr3 <= radius * radius)
                {
                    indexesWithinRadiusSet.Add(vertexIndex3);
                    if (distanceSqr3 < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr3;
                        closestIndex = vertexIndex3;
                    }
                }
            }

            indexesInRadius = indexesWithinRadiusSet.ToArray();
        }


        private static void EqualizeNormalsInternal(Mesh mesh)
        {
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var vertexCount = mesh.vertexCount;

            var vertexMap = new Dictionary<Vector3, List<int>>();

            for (var i = 0; i < vertexCount; i++)
            {
                var vertexPosition = vertices[i];

                if (!vertexMap.ContainsKey(vertexPosition))
                    vertexMap[vertexPosition] = new List<int>();

                vertexMap[vertexPosition].Add(i);
            }

            foreach (var entry in vertexMap)
            {
                var indices = entry.Value;
                var count = indices.Count;
                if (count <= 1) continue;

                var averageNormal = Vector3.zero;
                foreach (var index in indices) averageNormal += normals[index];
                averageNormal /= count;

                foreach (var index in indices) normals[index] = averageNormal;
            }

            mesh.normals = normals;
        }

        private static void MatchCapsuleColliderToBoundsInternal(Mesh mesh, CapsuleCollider capsuleCollider)
        {
            var bounds = mesh.bounds;
            var size = bounds.size;
            var lengths = new List<float> {size.x, size.y, size.z};
            lengths.Sort();
            capsuleCollider.radius = lengths[1] / 2;
            capsuleCollider.height = lengths[2];
            capsuleCollider.center = bounds.center;
            var direction = 0;
            var maxLength = size.x;
            if (size.y > maxLength)
            {
                direction = 1;
                maxLength = size.y;
            }

            if (size.z > maxLength) direction = 2;

            capsuleCollider.direction = direction;
        }

        private static Bounds TransformBoundsToWorldInternal(Bounds boundsLocal, Matrix4x4 matrix)
        {
            var center = matrix.MultiplyPoint3x4(boundsLocal.center);
            var extents = boundsLocal.extents;
            var boundsWorld = new Bounds(center, Vector3.zero);

            // Encapsulate all the possible world points to get full world size bounds
            boundsWorld.Encapsulate(matrix.MultiplyPoint3x4(PointFromBBox(boundsLocal.center, extents)));
            boundsWorld.Encapsulate(matrix.MultiplyPoint3x4(PointFromBBox(boundsLocal.center, -extents)));
            extents = -extents;
            extents.y *= -1;
            boundsWorld.Encapsulate(matrix.MultiplyPoint3x4(PointFromBBox(boundsLocal.center, extents)));
            extents.z *= -1;
            boundsWorld.Encapsulate(matrix.MultiplyPoint3x4(PointFromBBox(boundsLocal.center, extents)));
            extents = -extents;
            extents.x *= -1;
            boundsWorld.Encapsulate(matrix.MultiplyPoint3x4(PointFromBBox(boundsLocal.center, extents)));
            extents.y *= -1;
            boundsWorld.Encapsulate(matrix.MultiplyPoint3x4(PointFromBBox(boundsLocal.center, extents)));
            extents.z *= -1;
            boundsWorld.Encapsulate(matrix.MultiplyPoint3x4(PointFromBBox(boundsLocal.center, extents)));

            return boundsWorld;
        }

        private static Vector3 PointFromBBox(Vector3 center, Vector3 extents)
        {
            return new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);
        }

        private static bool CombineMeshesInternal(GameObject rootObj, List<GameObject> objects, bool saveMesh)
        {
            var renderers = new List<MeshRenderer>();
            var meshFilters = new List<MeshFilter>();
            foreach (var obj in objects)
            {
                if (!obj.TryGetComponent<MeshRenderer>(out var renderer) ||
                    !obj.TryGetComponent<MeshFilter>(out var meshFilter)) continue;
                renderers.Add(renderer);
                meshFilters.Add(meshFilter);
            }

            var allBoneWeights = new List<BoneWeight>();
            var allCombineInstances = new List<CombineInstance>();
            var allBindPoses = new List<Matrix4x4>();
            var allMaterials = new List<Material>();

            var boneOffset = 0;

            for (var i = 0; i < renderers.Count; i++)
            {
                var renderer = renderers[i];
                var meshFilter = meshFilters[i];

                allMaterials.AddRange(renderer.sharedMaterials);

                // Adjust the boneweights
                var boneWeights = meshFilter.sharedMesh.boneWeights;
                for (var j = 0; j < boneWeights.Length; j++)
                {
                    var boneWeight = boneWeights[j];
                    boneWeight.boneIndex0 += boneOffset;
                    boneWeight.boneIndex1 += boneOffset;
                    boneWeight.boneIndex2 += boneOffset;
                    boneWeight.boneIndex3 += boneOffset;
                    allBoneWeights.Add(boneWeight);
                }

                // Adjust the bind poses
                for (var j = 0; j < meshFilter.sharedMesh.bindposes.Length; ++j)
                    allBindPoses.Add(meshFilter.sharedMesh.bindposes[j] * renderer.transform.worldToLocalMatrix);

                var combineInstance = new CombineInstance
                {
                    mesh = meshFilter.sharedMesh,
                    transform = renderer.transform.localToWorldMatrix
                };
                allCombineInstances.Add(combineInstance);
            }

            var combinedRenderer = rootObj.GetComponent<MeshRenderer>();
            if (combinedRenderer == null) combinedRenderer = rootObj.AddComponent<MeshRenderer>();

            var combinedMeshFilter = rootObj.GetComponent<MeshFilter>();
            if (combinedMeshFilter == null) combinedMeshFilter = rootObj.AddComponent<MeshFilter>();

            /********************************************************************************************************************************/


            var mesh = new Mesh();
            mesh.CombineMeshes(allCombineInstances.ToArray(), false, true);
            mesh.boneWeights = allBoneWeights.ToArray();
            mesh.bindposes = allBindPoses.ToArray();
            mesh.RecalculateBounds();
            mesh.Optimize();

            combinedMeshFilter.sharedMesh = mesh;
            combinedRenderer.sharedMaterials = allMaterials.ToArray();


#if UNITY_EDITOR
            if (saveMesh)
                if (!SaveMesh(mesh))
                    return false;
#endif
            return true;
        }

        private static bool CombineMeshesMergeInternal(GameObject rootObj, List<GameObject> objects, bool saveMesh)
        {
            var firstObjRenderer = objects[0].GetComponent<MeshRenderer>();
            var materials = firstObjRenderer.sharedMaterials;

            var combineInstances = new List<CombineInstance>();

            foreach (var obj in objects)
            {
                var meshFilter = obj.GetComponent<MeshFilter>();
                for (var i = 0; i < meshFilter.sharedMesh.subMeshCount; i++)
                    combineInstances.Add(new CombineInstance
                    {
                        mesh = meshFilter.sharedMesh,
                        subMeshIndex = i,
                        transform = meshFilter.transform.localToWorldMatrix
                    });
            }

            var combinedRenderer = rootObj.AddComponent<MeshRenderer>();
            var combinedMeshFilter = rootObj.AddComponent<MeshFilter>();
            combinedMeshFilter.sharedMesh = new Mesh();
            combinedMeshFilter.sharedMesh.CombineMeshes(combineInstances.ToArray(), true);

            // use the materials array from the first object renderer's materials
            combinedRenderer.sharedMaterials = materials;

#if UNITY_EDITOR
            if (saveMesh)
                if (!SaveMesh(combinedMeshFilter.sharedMesh))
                    return false;
#endif

            return true;
        }

#if UNITY_EDITOR
        private static bool SaveMesh(Mesh mesh)
        {
            var defaultPath = "Assets/";
            var defaultName = "CombinedMesh.asset";
            var message = "Save Combined Mesh";
            var defaultExtension = "asset";

            var savePath = EditorUtility.SaveFilePanelInProject(message, defaultName, defaultExtension,
                "Please enter a file name to save the mesh to.", defaultPath);
            if (string.IsNullOrEmpty(savePath)) return false;

            AssetDatabase.CreateAsset(mesh, savePath);
            AssetDatabase.SaveAssets();
            return true;
        }
#endif

        private static void FlipNormalsInternal(Mesh mesh)
        {
            var normals = mesh.normals;
            for (var i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (var m = 0; m < mesh.subMeshCount; m++)
            {
                var triangles = mesh.GetTriangles(m);
                for (var i = 0; i < triangles.Length; i += 3) (triangles[i], triangles[i + 2]) = (triangles[i + 2], triangles[i]);
                mesh.SetTriangles(triangles, m);
            }
        }
    }
}