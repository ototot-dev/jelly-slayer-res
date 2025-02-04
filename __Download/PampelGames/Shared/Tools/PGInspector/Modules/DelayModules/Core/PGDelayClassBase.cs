// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using System;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    /// <summary>
    ///     Abstract base class for shared Delay implementations. All virtual methods should be called by the implementing MonoBehaviour.
    /// </summary>
    [Serializable]
    public abstract class PGDelayClassBase : PGIModule
    {
        /* Editor Virtual******************************************************************************************************************/

        /// <summary>
        ///     Name of the Delay used for custom inspectors.
        /// </summary>
        public virtual string ModuleName()
        {
            return "";
        }

        /// <summary>
        ///     Info about the Delay, used as Tooltip.
        /// </summary>
        public virtual string ModuleInfo()
        {
            return "";
        }

        protected PGSchedulerDescr scheduler;
        
        public virtual void ExecutionPreStart(MonoBehaviour mono, PGIHeader pgIHeader, Action ExecuteAction)
        {
        }
        public virtual void ExecutionStart(MonoBehaviour mono, PGIHeader pgIHeader)
        {
            PGScheduler.Stop(scheduler);
        }
        public virtual void ExecutionStop(MonoBehaviour baseComponent, PGIHeader pgIHeader)
        {
            PGScheduler.Stop(scheduler);
        }
        public virtual void ExecutionPause(MonoBehaviour mono, PGIHeader pgIHeader)
        {
            PGScheduler.Pause(scheduler);
        }
        public virtual void ExecutionResume(MonoBehaviour mono, PGIHeader pgIHeader)
        {
            PGScheduler.Resume(scheduler);
        }
    }
}