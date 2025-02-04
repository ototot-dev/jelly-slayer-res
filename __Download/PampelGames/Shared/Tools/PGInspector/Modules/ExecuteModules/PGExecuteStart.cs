// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteStart : PGExecuteClassBase
    {

        public override string ModuleName()
        {
            return "Start";
        }
        public override string ModuleInfo()
        {
            return "Starts automatically with Start().";
        }

        public override void ComponentStart(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentStart(baseComponent, ExecuteAction);
            ExecuteAction();
        }
        
    }
}
