// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    [Serializable]
    public class PGShakeSharedClass : PGIDuplicable
    {
        public float duration = 1;
        public Vector2 fadeInOut = new(0.1f, 0.75f);
        public float amplitude = 1;
        public float frequency = 10;
    }
}