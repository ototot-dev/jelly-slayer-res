// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using Unity.Mathematics;

namespace PampelGames.Shared.Utility
{
    /// <summary>
    ///     Random number generator, suitable for use in parallel threads.
    /// </summary>
    public static class PGRandom
    {
        public static float3 InsideUnitSphere(int seed)
        {
            var random = Random.CreateFromIndex((uint) seed);

            var u = random.NextFloat(0f, 1f);
            var x1 = random.NextFloat(9f, 11f) - 10f;
            var x2 = random.NextFloat(19f, 21f) - 20f;
            var x3 = random.NextFloat(29f, 31f) - 30f;
            var mag = math.sqrt(x1 * x1 + x2 * x2 + x3 * x3);
            if (mag == 0f)
            {
                x1 = 0f;
                x2 = 0f;
                x3 = 0f;
            }
            else
            {
                x1 /= mag;
                x2 /= mag;
                x3 /= mag;
            }

            var c = math.pow(u, 1f / 3f);
            return new float3(x1 * c, x2 * c, x3 * c);
        }

        public static float3 OnUnitSphere(int seed)
        {
            var random = Random.CreateFromIndex((uint) seed);

            var u = random.NextFloat();
            var v = random.NextFloat(1f, 2f) - 1f;
            var theta = 2 * math.PI * u;
            var phi = math.acos(2 * v - 1);
            var x = math.sin(phi) * math.cos(theta);
            var y = math.sin(phi) * math.sin(theta);
            var z = math.cos(phi);
            return new float3(x, y, z);
        }

        public static float3 OnUnitCircle(float3 normal, int seed)
        {
            var random = Random.CreateFromIndex((uint) seed);

            var r = 1.0f;
            var theta = random.NextFloat() * 2f * math.PI;

            var x = r * math.cos(theta);
            var z = r * math.sin(theta);
            var pointInPlane = new float3(x, 0f, z);

            if (normal.Equals(math.up())) return pointInPlane;

            var cross = math.cross(math.up(), normal);
            var dot = math.dot(math.up(), normal);
            var angle = math.atan2(math.length(cross), dot);

            var rotation = quaternion.AxisAngle(cross, angle);
            var pointIn3D = math.rotate(rotation, pointInPlane);
            return pointIn3D;
        }

        public static float3 InsideUnitCircle(float3 normal, int seed)
        {
            var random = Random.CreateFromIndex((uint) seed);

            var r = math.sqrt(random.NextFloat());
            var theta = (random.NextFloat(1f, 2f) - 1f) * 2f * math.PI;

            var x = r * math.cos(theta);
            var z = r * math.sin(theta);
            var pointInPlane = new float3(x, 0f, z);

            if (normal.Equals(math.up())) return pointInPlane;

            var cross = math.cross(math.up(), normal);
            var dot = math.dot(math.up(), normal);
            var angle = math.atan2(math.length(cross), dot);

            var rotation = quaternion.AxisAngle(cross, angle);
            var pointIn3D = math.rotate(rotation, pointInPlane);
            return pointIn3D;
        }
    }
}