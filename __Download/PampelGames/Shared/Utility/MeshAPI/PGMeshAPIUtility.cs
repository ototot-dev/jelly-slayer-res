// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace PampelGames.Shared.Utility
{
    public static class PGMeshAPIUtility
    {
        public static void SetSubMesh(Mesh mesh, int index, int vertexCount, int trianglesCount)
        {
            mesh.subMeshCount = index + 1;
            var smd = new SubMeshDescriptor
            {
                topology = MeshTopology.Triangles,
                vertexCount = vertexCount,
                indexCount = trianglesCount,
            };
            mesh.SetSubMesh(index, smd);
        }
        
        /********************************************************************************************************************************/
        
        /// <summary>
        ///     Sets mesh data using Unity's Advanced Mesh API (no colors).
        /// </summary>
        public static void SetBufferData01(Mesh mesh, NativeArray<VertexStruct01> vertexStruct01s, NativeArray<int> triangles)
        {
            mesh.SetVertexBufferParams(vertexStruct01s.Length, PGMeshAPILayout.VertexLayout01);
            mesh.SetVertexBufferData(vertexStruct01s, 0, 0, vertexStruct01s.Length);
            mesh.SetIndexBufferParams(triangles.Length, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, triangles.Length);
        }

        public static void SetBufferData01(Mesh mesh, VertexStruct01[] vertexStruct01s, int[] triangles)
        {
            mesh.SetVertexBufferParams(vertexStruct01s.Length, PGMeshAPILayout.VertexLayout01);
            mesh.SetVertexBufferData(vertexStruct01s, 0, 0, vertexStruct01s.Length);
            mesh.SetIndexBufferParams(triangles.Length, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, triangles.Length);
        }
        
        /// <summary>
        ///     Sets mesh data using Unity's Advanced Mesh API including <see cref="Color32"/> values.
        /// </summary>
        public static void SetBufferData02(Mesh mesh, NativeArray<VertexStruct02> vertexStruct02s, NativeArray<int> triangles)
        {
            mesh.SetVertexBufferParams(vertexStruct02s.Length, PGMeshAPILayout.VertexLayout02);
            mesh.SetVertexBufferData(vertexStruct02s, 0, 0, vertexStruct02s.Length);
            mesh.SetIndexBufferParams(triangles.Length, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, triangles.Length);
        }

        public static void SetBufferData02(Mesh mesh, VertexStruct02[] vertexStruct02s, int[] triangles)
        {
            mesh.SetVertexBufferParams(vertexStruct02s.Length, PGMeshAPILayout.VertexLayout02);
            mesh.SetVertexBufferData(vertexStruct02s, 0, 0, vertexStruct02s.Length);
            mesh.SetIndexBufferParams(triangles.Length, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, triangles.Length);
        }
        
        /// <summary>
        ///     Sets mesh data using Unity's Advanced Mesh API including a second UV channel (mesh.uv3, as uv2 are lightmaps).
        /// </summary>
        public static void SetBufferData03(Mesh mesh, NativeArray<VertexStruct03> vertexStruct03s, NativeArray<int> triangles)
        {
            mesh.SetVertexBufferParams(vertexStruct03s.Length, PGMeshAPILayout.VertexLayout03);
            mesh.SetVertexBufferData(vertexStruct03s, 0, 0, vertexStruct03s.Length);
            mesh.SetIndexBufferParams(triangles.Length, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, triangles.Length);
        }

        public static void SetBufferData03(Mesh mesh, VertexStruct03[] vertexStruct03s, int[] triangles)
        {
            mesh.SetVertexBufferParams(vertexStruct03s.Length, PGMeshAPILayout.VertexLayout03);
            mesh.SetVertexBufferData(vertexStruct03s, 0, 0, vertexStruct03s.Length);
            mesh.SetIndexBufferParams(triangles.Length, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, triangles.Length);
        }
        

    }
}