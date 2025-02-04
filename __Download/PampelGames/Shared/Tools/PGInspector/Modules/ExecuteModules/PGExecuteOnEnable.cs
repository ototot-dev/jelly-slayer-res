// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteOnEnable : PGExecuteClassBase
    {

        public override string ModuleName()
        {
            return "OnEnable";
        }
        public override string ModuleInfo()
        {
            return "Starts automatically with OnEnable().";
        }

        public override void ComponentOnEnable(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnEnable(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}