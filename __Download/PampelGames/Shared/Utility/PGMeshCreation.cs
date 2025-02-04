// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGMeshCreation
    {
        /// <summary>
        ///     forwardAxis = Z is equal to the Unity plane.
        /// </summary>
        /// <param name="uvX">Default is Vector2(0,1).</param>
        /// <param name="uvY">Default is Vector2(0,1).</param>
        public static void Rectangle(Mesh mesh, float width, float length, PGEnums.Axis forwardAxis, Vector2 uvX = default, Vector2 uvY = default)
        {
            RectangleInternal(mesh, width, length, forwardAxis, uvX, uvY);
        }

        /// <summary>
        ///     Generates a rectangle with the specified resolutions.
        ///     The forward axis is Z, equal to the Unity plane.
        /// </summary>
        /// <param name="resolutionW">The number of faces along the width.</param>
        /// <param name="resolutionL">The number of faces along the length.</param>
        public static void Rectangle(Mesh mesh, float width, float length, int resolutionW = 1, int resolutionL = 1)
        {
            RectangleInternal(mesh, width, length, resolutionW, resolutionL);
        }

        /// <summary>
        ///     Creates a circle.
        /// </summary>
        /// <param name="resolution">Amount of edgeloops, where one loop covers the radius (not diameter).</param>
        /// <param name="degrees">Starting at forward direction and going clockwise.</param>
        /// <param name="uvRadius">Starting at UV(0.5, 1) and going clockwise.</param>
        /// <param name="rotationY">Y-Rotation in degrees.</param>
        public static void Circle(Mesh mesh, float radius, int resolution, float degrees = 360f, float uvRadius = 0.5f, float rotationY = 0f)
        {
            CircleInternal(mesh, radius, resolution, degrees, uvRadius, rotationY);
        }

        /// <summary>
        ///     Creates a cylinder.
        /// </summary>
        /// <param name="height">The height of the cylinder.</param>
        /// <param name="topRadius">The radius of the cylinder at the top.</param>
        /// <param name="bottomRadius">The radius of the cylinder at the bottom.</param>
        /// <param name="verticalSegments">The number of vertical segments the cylinder should be divided into.</param>
        /// <param name="radialSegments">The number of radial segments the cylinder should be divided into.</param>
        /// <param name="closed">Whether the cylinder should have top and bottom covers.</param>
        /// <param name="uvStart">UV.y start coordinate of the radial faces.</param>
        /// <param name="uvEnd">UV.y end coordinate of the radial races.</param>
        /// <param name="markIndexes">
        ///     Creates 'UVs03' with vertex position data in the form of vertical segment index (x) and radial angle around the cylinder
        ///     (y). The angle for top and bottom closing center vertices is -1f.
        /// </param>
        public static void Cylinder(Mesh mesh, float height, float topRadius, float bottomRadius, int verticalSegments, int radialSegments,
            bool closed, float uvStart = 0f, float uvEnd = 1f, bool markIndexes = false)
        {
            CylinderInternal(mesh, height, topRadius, bottomRadius, verticalSegments, radialSegments, closed, uvStart, uvEnd, markIndexes);
        }


        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        private static void RectangleInternal(Mesh mesh, float width, float length, PGEnums.Axis forwardAxis, Vector2 uvX = default,
            Vector2 uvY = default)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();
            var uv01 = new Vector2(0f, 0f);
            var uv02 = new Vector2(1f, 0f);
            var uv03 = new Vector2(0f, 1f);
            var uv04 = new Vector2(1f, 1f);

            if (uvX != Vector2.zero || uvY != Vector2.zero)
            {
                uv01 = new Vector2(uvX.x, uvY.x);
                uv02 = new Vector2(uvX.y, uvY.x);
                uv03 = new Vector2(uvX.x, uvY.y);
                uv04 = new Vector2(uvX.y, uvY.y);
            }

            if (forwardAxis == PGEnums.Axis.Z)
            {
                triangles.AddRange(new[] {2, 1, 0, 1, 2, 3});
                vertices.AddRange(new Vector3[]
                {
                    new(-width / 2, 0, -length / 2),
                    new(width / 2, 0, -length / 2),
                    new(-width / 2, 0, length / 2),
                    new(width / 2, 0, length / 2)
                });
                normals.AddRange(new[] {Vector3.up, Vector3.up, Vector3.up, Vector3.up});
                uvs.AddRange(new[] {uv01, uv02, uv03, uv04});
            }
            else if (forwardAxis == PGEnums.Axis.Y)
            {
                triangles.AddRange(new[] {0, 1, 2, 3, 2, 1});
                vertices.AddRange(new Vector3[]
                {
                    new(-width / 2, -length / 2, 0),
                    new(width / 2, -length / 2, 0),
                    new(-width / 2, length / 2, 0),
                    new(width / 2, length / 2, 0)
                });
                normals.AddRange(new[] {Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward});
                uvs.AddRange(new[] {uv01, uv02, uv03, uv04});
            }
            else
            {
                triangles.AddRange(new[] {2, 1, 0, 1, 2, 3});
                vertices.AddRange(new Vector3[]
                {
                    new(0, -width / 2, -length / 2),
                    new(0, width / 2, -length / 2),
                    new(0, -width / 2, length / 2),
                    new(0, width / 2, length / 2)
                });
                normals.AddRange(new[] {Vector3.right, Vector3.right, Vector3.right, Vector3.right});
                uvs.AddRange(new[] {uv01, uv02, uv03, uv04});
            }

            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            PGMeshUtility.RecalculateMeshData(mesh, false);
        }

        private static void RectangleInternal(Mesh mesh, float width, float length, int resolutionW = 1, int resolutionL = 1)
        {
            resolutionW = Mathf.Max(1, resolutionW);
            resolutionL = Mathf.Max(1, resolutionL);
            var division = new Vector2(resolutionW, resolutionL);

            var size = new Vector2(width, length);
            var divx = (int) division.x;
            var divy = (int) division.y;

            var step = size / division;
            var offset = new Vector3(size.x / 2f, 0, size.y / 2f);

            var vCount = (divx + 1) * (divy + 1);
            var vertices = new Vector3[vCount];
            var uvs = new Vector2[vCount];
            var tCount = divx * divy * 2 * 3;

            var triangles = new int[tCount];

            var tIndex = 0;
            for (var dy = 0; dy <= divy; dy++)
            for (var dx = 0; dx <= divx; dx++)
            {
                var sx = dx * step.x;
                var sy = dy * step.y;
                var vIndex = dy * (divx + 1) + dx;
                var pos = Vector3.right * sx + Vector3.forward * sy;
                vertices[vIndex] = pos - offset;
                uvs[vIndex] = new Vector2(sx / size.x, sy / size.y);

                if (dx == divx || dy == divy) continue;

                var tIndexP1 = tIndex + 1;
                var tIndexP2 = tIndex + 2;
                var tIndexP3 = tIndex + 3;
                var tIndexP4 = tIndex + 4;
                var tIndexP5 = tIndex + 5;

                triangles[tIndex] = vIndex;
                triangles[tIndexP1] = vIndex + divx + 1;
                triangles[tIndexP2] = vIndex + divx + 2;

                triangles[tIndexP3] = vIndex;
                triangles[tIndexP4] = vIndex + divx + 2;
                triangles[tIndexP5] = vIndex + 1;

                tIndex += 6;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            PGMeshUtility.RecalculateMeshData(mesh, true);
        }

        private static void CircleInternal(Mesh mesh, float radius, int resolution, float degrees = 360f, float uvRadius = 0.5f,
            float rotationY = 0f)
        {
            resolution++;
            var totalPoints = Mathf.FloorToInt(resolution * (degrees / 360f)) + 1;
            var radStartRotation = rotationY * Mathf.Deg2Rad;

            var vertices = new Vector3[totalPoints];
            var uv = new Vector2[totalPoints];

            vertices[0] = Vector3.zero;
            uv[0] = new Vector2(0.5f, 0.5f);

            for (var i = 0; i < totalPoints - 1; i++)
            {
                var ratio = (float) i / (totalPoints - 2);
                var rad = ratio * Mathf.Deg2Rad * degrees + radStartRotation;
                var cos = Mathf.Cos(rad);
                var sin = Mathf.Sin(rad);

                vertices[i + 1] = new Vector3(sin * radius, 0, cos * radius);
                uv[i + 1] = new Vector2(sin * uvRadius + 0.5f, cos * uvRadius + 0.5f);
            }

            var triangles = new int[(totalPoints - 2) * 3];

            for (var i = 0; i < totalPoints - 2; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            PGMeshUtility.RecalculateMeshData(mesh, true);
        }

        private static void CylinderInternal(Mesh mesh, float height, float topRadius, float bottomRadius, int verticalSegments, int radialSegments,
            bool closed, float uvStart, float uvEnd, bool markIndexes)
        {
            mesh.Clear();

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var UVs = new List<Vector2>();
            var triangles = new List<int>();
            var UVs03 = new List<Vector2>();


            var halfHeight = height * 0.5f;
            int verticalIndex;

            var vertexRows = new List<List<int>>();

            // Vertical segmentation
            for (verticalIndex = 0; verticalIndex <= verticalSegments; verticalIndex++)
            {
                var currentRowVertices = new List<int>();

                var vCoord = verticalIndex / (float) verticalSegments;
                var currentRadius = vCoord * (bottomRadius - topRadius) + topRadius;

                // UV y-coordinate mapped into uvStart-uvEnd range.
                var uvHeight = uvStart + vCoord * (uvEnd - uvStart);

                // Radial segmentation
                for (var radialIndex = 0; radialIndex <= radialSegments; radialIndex++)
                {
                    var uCoord = radialIndex / (float) radialSegments;

                    var vertex = new Vector3();
                    vertex.x = currentRadius * Mathf.Sin(uCoord * Mathf.PI * 2.0f);
                    vertex.y = -vCoord * height + halfHeight;
                    vertex.z = currentRadius * Mathf.Cos(uCoord * Mathf.PI * 2.0f);

                    vertices.Add(vertex);
                    normals.Add(new Vector3(vertex.x, 0f, vertex.z).normalized);
                    UVs.Add(new Vector2(1 - uCoord, 1 - uvHeight));

                    currentRowVertices.Add(vertices.Count - 1);

                    if (markIndexes)
                    {
                        var verticalSegmentIndex = verticalSegments - verticalIndex;
                        var uvAngle = 360f * radialIndex / radialSegments;
                        UVs03.Add(new Vector2(verticalSegmentIndex, uvAngle));
                    }
                }

                vertexRows.Add(currentRowVertices);
            }

            var radiusDiffTangent = (bottomRadius - topRadius) / height;

            // Faces creation
            for (var radialIndex = 0; radialIndex < radialSegments; radialIndex++)
            {
                Vector3 normalStart;
                Vector3 normalEnd;
                if (topRadius != 0)
                {
                    normalStart = vertices[vertexRows[0][radialIndex]];
                    normalEnd = vertices[vertexRows[0][radialIndex + 1]];
                }
                else
                {
                    normalStart = vertices[vertexRows[1][radialIndex]];
                    normalEnd = vertices[vertexRows[1][radialIndex + 1]];
                }

                normalStart.y = Mathf.Sqrt(normalStart.x * normalStart.x + normalStart.z * normalStart.z) * radiusDiffTangent;
                normalEnd.y = Mathf.Sqrt(normalEnd.x * normalEnd.x + normalEnd.z * normalEnd.z) * radiusDiffTangent;

                for (verticalIndex = 0; verticalIndex < verticalSegments; verticalIndex++)
                {
                    var vertexIndex1 = vertexRows[verticalIndex][radialIndex];
                    var vertexIndex2 = vertexRows[verticalIndex + 1][radialIndex];
                    var vertexIndex3 = vertexRows[verticalIndex + 1][radialIndex + 1];
                    var vertexIndex4 = vertexRows[verticalIndex][radialIndex + 1];

                    triangles.Add(vertexIndex1);
                    triangles.Add(vertexIndex2);
                    triangles.Add(vertexIndex4);

                    triangles.Add(vertexIndex2);
                    triangles.Add(vertexIndex3);
                    triangles.Add(vertexIndex4);
                }
            }

            // Adding the top and bottom cover if needed
            if (closed)
            {
                if (topRadius > 0f)
                {
                    vertices.Add(new Vector3(0, halfHeight, 0));
                    normals.Add(Vector3.up);
                    UVs.Add(new Vector2(0.5f, 0));

                    if (markIndexes)
                    {
                        var verticalSegmentIndex = verticalSegments;
                        UVs03.Add(new Vector2(verticalSegmentIndex, -1f));
                    }

                    for (var radialIndex = 0; radialIndex < radialSegments; radialIndex++)
                    {
                        var vertexIndex1 = vertexRows[0][radialIndex];
                        var vertexIndex2 = vertexRows[0][radialIndex + 1];
                        var vertexIndex3 = vertices.Count - 1;

                        triangles.Add(vertexIndex1);
                        triangles.Add(vertexIndex2);
                        triangles.Add(vertexIndex3);
                    }
                }

                if (bottomRadius > 0f)
                {
                    vertices.Add(new Vector3(0, -halfHeight, 0));
                    normals.Add(Vector3.down);
                    UVs.Add(new Vector2(0.5f, 1));

                    if (markIndexes)
                    {
                        var verticalSegmentIndex = 0;
                        UVs03.Add(new Vector2(verticalSegmentIndex, -1f));
                    }

                    for (var radialIndex = 0; radialIndex < radialSegments; radialIndex++)
                    {
                        var vertexIndex1 = vertexRows[verticalIndex][radialIndex + 1];
                        var vertexIndex2 = vertexRows[verticalIndex][radialIndex];
                        var vertexIndex3 = vertices.Count - 1;

                        triangles.Add(vertexIndex1);
                        triangles.Add(vertexIndex2);
                        triangles.Add(vertexIndex3);
                    }
                }
            }


            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = UVs.ToArray();
            mesh.triangles = triangles.ToArray();
            if (markIndexes) mesh.SetUVs(2, UVs03);

            PGMeshUtility.RecalculateMeshData(mesh, false);
        }
    }
}