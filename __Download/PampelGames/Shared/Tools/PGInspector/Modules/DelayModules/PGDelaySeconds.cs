// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGDelaySeconds : PGDelayClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

        public override string ModuleName()
        {
            return "Seconds";
        }
        public override string ModuleInfo()
        {
            return "Delays execution for the specified amount of seconds.";
        }

        [Tooltip("Delay in seconds.")] public float delaySeconds = 1f;


        public override void ExecutionPreStart(MonoBehaviour mono, PGIHeader pgIHeader, Action ExecuteAction)
        {
            base.ExecutionPreStart(mono, pgIHeader, ExecuteAction);
            scheduler = PGScheduler.ScheduleTime(mono, delaySeconds, ExecuteAction);
        }

    }
}