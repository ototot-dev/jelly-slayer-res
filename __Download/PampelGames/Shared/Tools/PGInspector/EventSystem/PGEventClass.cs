// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine.Events;

namespace PampelGames.Shared.Tools.PGInspector
{
    [Serializable]
    public class PGEventClass
    {
        public bool activated;
        public float chance = 1f;
        public UnityEvent unityEvent;

        public bool Invoke()
        {
            if (!activated) return false;
            if (UnityEngine.Random.value > chance) return false;
            unityEvent.Invoke();
            return true;
        }
    }
}
