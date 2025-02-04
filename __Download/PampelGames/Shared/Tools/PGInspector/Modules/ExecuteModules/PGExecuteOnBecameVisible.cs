// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnBecameVisible : PGExecuteClassBase
    {

        public override string ModuleName()
        {
            return "On Became Visible";
        }
        public override string ModuleInfo()
        {
            return "Starts when an attached renderer became visible by any camera.";
        }

        public override void ComponentOnBecameVisible(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnBecameVisible(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}