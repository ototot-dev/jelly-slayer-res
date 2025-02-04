// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using Unity.Mathematics;

namespace PampelGames.Shared.Utility
{
    /// <summary>
    ///     Deals with tangents and angles in radians.
    /// </summary>
    public static class PGTrigonometryUtility
    {
        /// <summary>
        ///     Calculates the radian angle between two directional vectors projected onto the XZ plane.
        /// </summary>
        public static float AngleXZ(float3 tangent1, float3 tangent2)
        {
            var tan1_2D = new float2(tangent1.x, tangent1.z);
            var tan2_2D = new float2(tangent2.x, tangent2.z);
            return Angle(tan1_2D, tan2_2D);
        }

        /// <summary>
        ///     Calculates the radian angle between two directional vectors.
        /// </summary>
        public static float Angle(float2 tangent1, float2 tangent2)
        {
            var dotProd = math.dot(tangent1, tangent2);
            var det = tangent1.x * tangent2.y - tangent1.y * tangent2.x;
            return -math.atan2(det, dotProd);
        }

        /// <summary>
        ///     Returns the relative angle between the tangents in clockwise order in radian.
        /// </summary>
        public static float AngleClockwiseXZ(float3 tangent1, float3 tangent2)
        {
            var tan1_2D = new float2(tangent1.x, tangent1.z);
            var tan2_2D = new float2(tangent2.x, tangent2.z);
            return AngleClockwise(tan1_2D, tan2_2D);
        }

        /// <summary>
        ///     Returns the relative angle between the tangents in clockwise order in radian.
        /// </summary>
        public static float AngleClockwise(float2 tangent1, float2 tangent2)
        {
            var dotProd = math.dot(tangent1, tangent2);
            var det = tangent1.x * tangent2.y - tangent1.y * tangent2.x;
            var angleRad = -math.atan2(det, dotProd);
            if (angleRad < 0) angleRad += 2 * math.PI;
            return angleRad;
        }

        /// <summary>
        ///     Calculates the slope in radians between two 3D positions on the XZ plane.
        ///     Clamped between [-π/2, π/2], or in terms of degrees, [-90, 90].
        /// </summary>
        public static float Slope(float3 position01, float3 position02)
        {
            var rise = position02.y - position01.y;
            var distance = math.distance(new float2(position01.x, position01.z), new float2(position02.x, position02.z));
            if (distance == 0f) return 0f;
            var slope = math.atan(rise / distance);
            return slope;
        }

        /// <summary>
        ///     Rotates a tangent by a given angle in radians, where positive angles move clockwise.
        /// </summary>
        public static float3 RotateTangentXZ(float3 tangent, float radian)
        {
            var turnedTangent2D = RotateTangent(new float2(tangent.x, tangent.z), radian);
            return new float3(turnedTangent2D.x, tangent.y, turnedTangent2D.y);
        }

        /// <summary>
        ///     Rotates a tangent by a given angle in radians, where positive angles move clockwise.
        /// </summary>
        public static float2 RotateTangent(float2 tangent, float radian)
        {
            var rotationMatrix = new float2x2(math.cos(-radian), -math.sin(-radian), math.sin(-radian), math.cos(-radian));
            return math.mul(rotationMatrix, tangent);
        }

        /// <summary>
        ///     Turns the given tangent vector 90 degrees clockwise in the XZ plane.
        /// </summary>
        public static float3 RotateTangent90ClockwiseXZ(float3 tangent)
        {
            var tan_2D = new float2(tangent.x, tangent.z);
            var turnedTan_2D = RotateTangent90Clockwise(tan_2D);
            tangent = new float3(turnedTan_2D.x, tangent.y, turnedTan_2D.y);
            return tangent;
        }

        /// <summary>
        ///     Turns the given tangent vector 90 degrees clockwise.
        /// </summary>
        public static float2 RotateTangent90Clockwise(float2 tangent)
        {
            return new float2(tangent.y, -tangent.x);
        }

        /// <summary>
        ///     Snaps the given tangent to a specified angle around a fix point.
        /// </summary>
        /// <param name="tangentSnap">The tangent to be snapped.</param>
        /// <param name="tangentFix">The fix tangent around which the tangent will be snapped.</param>
        /// <param name="radian">Snap steps in radian, relative to the tangentFixPoint.</param>
        public static float3 SnapTangentXZ(float3 tangentSnap, float3 tangentFix, float radian)
        {
            var tangentSnap2D = SnapTangent(new float2(tangentSnap.x, tangentSnap.z),
                new float2(tangentFix.x, tangentFix.z), radian);
            return new float3(tangentSnap2D.x, tangentSnap.y, tangentSnap2D.y);
        }

        /// <summary>
        ///     Snaps the given tangent to a specified angle around a fix point.
        /// </summary>
        /// <param name="tangentSnap">The tangent to be snapped.</param>
        /// <param name="tangentFix">The fix tangent around which the tangent will be snapped.</param>
        /// <param name="radian">Snap steps in radian, relative to the tangentFixPoint.</param>
        public static float2 SnapTangent(float2 tangentSnap, float2 tangentFix, float radian)
        {
            var angle = Angle(tangentSnap, tangentFix);
            var roundedAngle = math.round(angle / radian) * radian;
            var rotatedTangent = RotateTangent(tangentSnap, angle - roundedAngle);
            return rotatedTangent;
        }

        /// <summary>
        ///     Calculates the intersection point of two directional vectors in XZ space.
        /// </summary>
        public static float3 IntersectionPointXZ(float3 position1, float3 tangent1, float3 position2, float3 tangent2)
        {
            var intersectionPoint2D = IntersectionPoint(new float2(position1.x, position1.z), new float2(tangent1.x, tangent1.z),
                new float2(position2.x, position2.z), new float2(tangent2.x, tangent2.z));
            return new float3(intersectionPoint2D.x, position1.y, intersectionPoint2D.y);
        }

        /// <summary>
        ///     Calculates the intersection point of two directional vectors in 2D space.
        /// </summary>
        public static float2 IntersectionPoint(float2 position1, float2 tangent1, float2 position2, float2 tangent2)
        {
            var line1End = position1 + tangent1;
            var line2End = position2 + tangent2;
            var denominator = (position1.x - line1End.x) * (position2.y - line2End.y) - (position1.y - line1End.y) * (position2.x - line2End.x);

            if (math.abs(denominator) < 0.0001f) return new float2((position1.x + position2.x) / 2, (position1.x + position2.y) / 2);
            
            var line1Value = position1.x * line1End.y - position1.y * line1End.x;
            var line2Value = position2.x * line2End.y - position2.y * line2End.x;
            var intersectX = (line1Value * (position2.x - line2End.x) - (position1.x - line1End.x) * line2Value) / denominator;
            var intersectY = (line1Value * (position2.y - line2End.y) - (position1.y - line1End.y) * line2Value) / denominator;

            return new float2(intersectX, intersectY);
        }

        /// <summary>
        ///     Returns the same tangent or one that is 180°, based on the fixPoint.
        /// </summary>
        public static float3 DirectionalTangentToPointXZ(float3 fixPoint, float3 position, float3 tangent)
        {
            var pos_2D = new float2(position.x, position.z);
            var fixPoint_2D = new float2(fixPoint.x, fixPoint.z);
            var tan_2D = new float2(tangent.x, tangent.z);
            var dir = fixPoint_2D - pos_2D;
            if (math.dot(dir, tan_2D) < 0) tangent *= -1;
            return tangent;
        }

        /// <summary>
        ///     Returns the same tangent or one that is 180°, based on the fixPoint.
        /// </summary>
        public static float2 DirectionalTangentToPoint(float2 fixPoint, float2 position, float2 tangent)
        {
            var dir = fixPoint - position;
            if (math.dot(dir, tangent) < 0) tangent *= -1;
            return tangent;
        }
        
        /// <summary>
        ///     Returns the same tangent or one that is 180°, based on the fixPoint.
        /// </summary>
        public static float3 DirectionalTangentToPoint(float3 fixPoint, float3 position, float3 tangent)
        {
            var dir = math.normalizesafe(fixPoint - position);
            if (math.dot(dir, tangent) < 0) tangent *= -1;
            return tangent;
        }
        
        /// <summary>
        ///     Returns a tangent that is 90° to the original, rather in the direction of the fixPoint.
        /// </summary>
        public static float3 PerpendicularTangentToPointXZ(float3 fixPoint, float3 position, float3 tangent)
        {
            var position2D = new float2(position.x, position.z);
            var tangent2D = new float2(tangent.x, tangent.z);
            var fixPoint2D = new float2(fixPoint.x, fixPoint.z);
            var perpTangent2D = PerpendicularTangentToPoint(fixPoint2D, position2D, tangent2D);
            return new float3(perpTangent2D.x, tangent.y, perpTangent2D.y);
        }

        /// <summary>
        ///     Returns a tangent that is 90° to the original, rather in the direction of the fixPoint.
        /// </summary>
        public static float2 PerpendicularTangentToPoint(float2 fixPoint, float2 position, float2 tangent)
        {
            var perpTangent = new float2(tangent.y, -tangent.x);

            // Making sure tangent goes in correct direction.
            var dir1 = fixPoint - position;
            if (math.dot(dir1, perpTangent) < 0) perpTangent *= -1;

            return perpTangent;
        }

        /// <summary>
        ///     Calculates the distance between two 3D positions projected onto the XZ plane.
        /// </summary>
        public static float DistanceXZ(float3 position01, float3 position02)
        {
            var pos012D = new float2(position01.x, position01.z);
            var pos022D = new float2(position02.x, position02.z);
            return math.distance(pos012D, pos022D);
        }
        
        /// <summary>
        ///     The distance from pos01 to the intersection point from pos01 to pos02 when moving pos01 along the tangent.
        ///     Positive when position02 is in tangent direction.
        /// </summary>
        public static float DirectionalDistanceXZ(float3 position01, float3 tangent01, float3 position02)
        {
            var pos012D = new float2(position01.x, position01.z);
            var pos022D = new float2(position02.x, position02.z);
            var tan012D = new float2(tangent01.x, tangent01.z);
            return DirectionalDistance(pos012D, tan012D, pos022D);
        }

        /// <summary>
        ///     The distance from pos01 to the intersection point from pos01 to pos02 when moving pos01 along the tangent.
        ///     Positive when position02 is in tangent direction.
        /// </summary>
        public static float DirectionalDistance(float2 position01, float2 tangent01, float2 position02)
        {
            var vectorAB = position02 - position01;
            var normalizedTan01 = math.normalizesafe(tangent01);
            var distance = math.dot(vectorAB, normalizedTan01);
            return distance;
        }

        /// <returns>True if the two vectors are pointing in the same direction, false otherwise.</returns>
        public static bool IsSameDirectionXZ(float3 vectorA, float3 vectorB)
        {
            var dotProduct = math.dot(new float3(vectorA.x, 0, vectorA.z), new float3(vectorB.x, 0, vectorB.z));
            return dotProduct > 0;
        }
        
        /// <returns>True if the two vectors are pointing in the same direction, false otherwise.</returns>
        public static bool IsSameDirection(float2 vectorA, float2 vectorB)
        {
            var dotProduct = math.dot(vectorA, vectorB);
            return dotProduct > 0;
        }
        
        /// <returns>True if the two vectors are pointing in the same direction, false otherwise.</returns>
        public static bool IsSameDirection(float3 vectorA, float3 vectorB)
        {
            var dotProduct = math.dot(vectorA, vectorB);
            return dotProduct > 0;
        }
        


    }
}