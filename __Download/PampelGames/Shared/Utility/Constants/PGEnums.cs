// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGEnums
    {
        public enum AxisEnum
        {
            [InspectorName("X+")] XPlus,
            [InspectorName("Y+")] YPlus,
            [InspectorName("Z+")] ZPlus,
            [InspectorName("X-")] XMinus,
            [InspectorName("Y-")] YMinus,
            [InspectorName("Z-")] ZMinus
        }
        
        public static Vector3 GetAxis(AxisEnum axis)
        {
            return m_AlignAxisToVector[(int) axis];
        }
        
        private static readonly Vector3[] m_AlignAxisToVector =
        {
            Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back
        };
        
        public enum AxisEnumXY
        {
            [InspectorName("X+")] XPlus,
            [InspectorName("Y+")] YPlus,
            [InspectorName("X-")] XMinus,
            [InspectorName("Y-")] YMinus
        }
        
        public enum AxisEnumXZ
        {
            [InspectorName("X+")] XPlus,
            [InspectorName("Z+")] ZPlus,
            [InspectorName("X-")] XMinus,
            [InspectorName("Z-")] ZMinus
        }
        
        public enum Axis
        {
            X,
            Y,
            Z,
        }
        
        /********************************************************************************************************************************/

        public enum RenderPipelineEnum
        {
            BuiltIn,
            URP,
            HDRP
        }
        
        /********************************************************************************************************************************/
        public enum UpdateMode
        {
            Update,
            FixedUpdate,
            LateUpdate
        }
        
        public static YieldInstruction GetYieldInstruction(UpdateMode axis)
        {
            return yieldInstructions[(int) axis];
        }
        
        private static readonly YieldInstruction[] yieldInstructions =
        {
            null, // corresponds to Update
            new WaitForFixedUpdate(), // corresponds to FixedUpdate
            new WaitForEndOfFrame() // corresponds to LateUpdate
        };
        
        /********************************************************************************************************************************/
        
        public enum Resolution
        {
            [InspectorName("16")] _16 = 16,
            [InspectorName("32")] _32 = 32,
            [InspectorName("64")] _64 = 64,
            [InspectorName("128")] _128 = 128,
            [InspectorName("256")] _256 = 256,
            [InspectorName("512")] _512 = 512,
            [InspectorName("1024")] _1024 = 1024,
            [InspectorName("2048")] _2048 = 2048,
            [InspectorName("4096")] _4096 = 4096,
        }
    }
}
