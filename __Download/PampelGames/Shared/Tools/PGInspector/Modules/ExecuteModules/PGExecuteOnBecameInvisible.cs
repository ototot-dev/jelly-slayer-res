// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnBecameInvisible : PGExecuteClassBase
    {

        public override string ModuleName()
        {
            return "On Became Invisible";
        }
        
        public override string ModuleInfo()
        {
            return "Starts when an attached renderer is no longer visible by any camera.";
        }

        public override void ComponentOnBecameInvisible(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnBecameInvisible(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}