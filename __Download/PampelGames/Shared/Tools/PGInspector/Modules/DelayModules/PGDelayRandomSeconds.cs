// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGDelayRandomSeconds : PGDelayClassBase
    {
        /* Editor Virtual******************************************************************************************************************/

        public override string ModuleName()
        {
            return "Random Seconds";
        }
        public override string ModuleInfo()
        {
            return "Delays execution for a random amount of seconds.";
        }

        [Tooltip("Random delay between x and y in seconds.")]
        public Vector2 delayRandomSeconds = new(0, 1);
        
        public override void ExecutionPreStart(MonoBehaviour mono, PGIHeader pgIHeader, Action ExecuteAction)
        {
            base.ExecutionPreStart(mono, pgIHeader, ExecuteAction);
            scheduler = PGScheduler.ScheduleTime(mono, Random.Range(delayRandomSeconds.x, delayRandomSeconds.y), ExecuteAction);
        }
        

    }
}