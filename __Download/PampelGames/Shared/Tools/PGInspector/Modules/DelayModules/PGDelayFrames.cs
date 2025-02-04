// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGDelayFrames : PGDelayClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

        public override string ModuleName()
        {
            return "Frames";
        }
        public override string ModuleInfo()
        {
            return "Delays execution for the specified amount of frames.";
        }

        [Tooltip("Delay in frames.")] public int delayFrames = 1;
        
        public override void ExecutionPreStart(MonoBehaviour mono, PGIHeader pgIHeader, Action ExecuteAction)
        {
            base.ExecutionPreStart(mono, pgIHeader, ExecuteAction);
            scheduler = PGScheduler.ScheduleFrames(mono, delayFrames, ExecuteAction);
        }
        

    }
}