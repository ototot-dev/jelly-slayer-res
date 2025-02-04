// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopOnBecameVisible : PGStopClassBase
    {

        public override string ModuleName()
        {
            return "On Became Visible";
        }
        public override string ModuleInfo()
        {
            return "Stops when an attached renderer became visible by any camera.";
        }

        public override void ComponentOnBecameVisible(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ComponentOnBecameVisible(baseComponent, StopAction);
            StopAction();
        }
        
    }
}