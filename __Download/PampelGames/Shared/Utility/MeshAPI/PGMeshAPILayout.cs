// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace PampelGames.Shared.Utility
{
    public struct VertexStruct01
    {
        public float3 vertex;
        public float3 normal;
        public float2 uv;
    }
    public struct VertexStruct02
    {
        public float3 vertex;
        public float3 normal;
        public Color32 color;
        public float2 uv;
    }
    public struct VertexStruct03
    {
        public float3 vertex;
        public float3 normal;
        public float2 uv;
        public float2 uv3; // Can be used for custom information if Color32 (byte max. 255) is not enough.
    }
    public static class PGMeshAPILayout
    {
        public static readonly VertexAttributeDescriptor[] VertexLayout01 =
        {
            new() {attribute = VertexAttribute.Position, format = VertexAttributeFormat.Float32, dimension = 3},
            new() {attribute = VertexAttribute.Normal, format = VertexAttributeFormat.Float32, dimension = 3},
            new() {attribute = VertexAttribute.TexCoord0, format = VertexAttributeFormat.Float32, dimension = 2}
        };
        public static readonly VertexAttributeDescriptor[] VertexLayout02 =
        {
            new() {attribute = VertexAttribute.Position, format = VertexAttributeFormat.Float32, dimension = 3},
            new() {attribute = VertexAttribute.Normal, format = VertexAttributeFormat.Float32, dimension = 3},
            new() {attribute = VertexAttribute.Color, format = VertexAttributeFormat.UNorm8, dimension = 4},
            new() {attribute = VertexAttribute.TexCoord0, format = VertexAttributeFormat.Float32, dimension = 2}
        };
        public static readonly VertexAttributeDescriptor[] VertexLayout03 =
        {
            new() {attribute = VertexAttribute.Position, format = VertexAttributeFormat.Float32, dimension = 3},
            new() {attribute = VertexAttribute.Normal, format = VertexAttributeFormat.Float32, dimension = 3},
            new() {attribute = VertexAttribute.TexCoord0, format = VertexAttributeFormat.Float32, dimension = 2},
            new() {attribute = VertexAttribute.TexCoord2, format = VertexAttributeFormat.Float32, dimension = 2}
        };
    }
}
