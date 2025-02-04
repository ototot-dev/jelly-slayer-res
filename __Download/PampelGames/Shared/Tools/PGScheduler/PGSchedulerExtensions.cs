// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Extension methods for <see cref="PGSchedulerDescr" />.
    /// </summary>
    public static class PGSchedulerExtensions
    {
        public static void Pause(this PGSchedulerDescr scheduler)
        {
            if (scheduler == null) return;
            scheduler.active = false;
            scheduler.internalEvents.onPause?.Invoke();
        }

        public static void Resume(this PGSchedulerDescr scheduler)
        {
            if (scheduler == null) return;
            if (!scheduler.completed) scheduler.active = true;
            scheduler.internalEvents.onResume?.Invoke();
        }

        public static void Stop(this PGSchedulerDescr scheduler)
        {
            if (scheduler == null) return;
            scheduler.stopped = true;
            scheduler.internalEvents.onStop?.Invoke();
        }

        /* Callbacks *******************************************************************************************************************************/
        
        public static void OnUpdate(this PGSchedulerDescr scheduler, Action action)
        {
            scheduler.internalEvents.onUpdate = action;
        }

        public static void OnComplete(this PGSchedulerDescr scheduler, Action action)
        {
            scheduler.internalEvents.onComplete = action;
        }

        public static void OnPause(this PGSchedulerDescr scheduler, Action action)
        {
            scheduler.internalEvents.onPause = action;
        }

        public static void OnResume(this PGSchedulerDescr scheduler, Action action)
        {
            scheduler.internalEvents.onResume = action;
        }

        public static void OnStop(this PGSchedulerDescr scheduler, Action action)
        {
            scheduler.internalEvents.onStop = action;
        }
        
    }
}