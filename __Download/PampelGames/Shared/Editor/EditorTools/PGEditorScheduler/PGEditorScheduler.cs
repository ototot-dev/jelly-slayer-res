// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    public static class PGEditorScheduler
    {
        /// <summary>
        ///     Waits the specified duration to execute the action.
        /// </summary>
        public static PGEditorSchedulerDescr ScheduleTime(float delayDuration, Action action)
        {
            var scheduler = new PGEditorSchedulerDescr
            {
                duration = delayDuration,
                onComplete = action
            };

            PGEditorCoroutineUtility.StartCoroutine(PGEditorSchedulerUpdate._SchedulerUpdate(scheduler));
            return scheduler;
        }
        
        /// <summary>
        ///     Executes one action after the duration until the list is done.
        /// </summary>
        public static PGEditorSchedulerDescr ScheduleTimeList(float delayDuration, List<Action> actionList)
        {
            var scheduler = new PGEditorSchedulerDescr
            {
                duration = delayDuration,
                scheduleList = true,
                onCompleteList = actionList
            };

            PGEditorCoroutineUtility.StartCoroutine(PGEditorSchedulerUpdate._SchedulerUpdate(scheduler));
            return scheduler;
        }
    }
}
