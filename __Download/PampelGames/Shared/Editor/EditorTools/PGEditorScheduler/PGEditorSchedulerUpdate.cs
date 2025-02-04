// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Updates <see cref="PGEditorSchedulerDescr" />s.
    /// </summary>
    internal static class PGEditorSchedulerUpdate
    {
        internal static IEnumerator _SchedulerUpdate(PGEditorSchedulerDescr scheduler)
        {
            float timeStarted = Time.realtimeSinceStartup;
            
            
            if(!scheduler.scheduleList)
                for (;;)
                {
                    float timeElapsed = Time.realtimeSinceStartup - timeStarted;
                    scheduler.currentTime = timeElapsed;
                
                    if (scheduler.stopped)
                    {
                        yield break;
                    }
                
                    if (scheduler.currentTime >= scheduler.duration)
                    {
                        scheduler.currentTime = scheduler.duration;
                        scheduler.completed = true;
                    }

                    if (!scheduler.completed) yield return null;
                    else
                    {
                        scheduler.onComplete?.Invoke();
                        yield break;
                    }
                }    
            else
                for (;;)
                {
                    float timeElapsed = Time.realtimeSinceStartup - timeStarted;
                    scheduler.currentTime = timeElapsed;
                
                    if (scheduler.stopped)
                    {
                        yield break;
                    }
                
                    if (scheduler.currentTime >= scheduler.duration)
                    {
                        scheduler.currentTime = scheduler.duration;
                        scheduler.completed = true;
                    }

                    if (!scheduler.completed) yield return null;
                    else
                    {
                        if(scheduler.onCompleteList.Count > scheduler.currentListItem)
                            scheduler.onCompleteList[scheduler.currentListItem]?.Invoke();
                        scheduler.currentListItem++;
                        if (scheduler.currentListItem >= scheduler.onCompleteList.Count)
                        {
                            scheduler.onComplete?.Invoke();
                            yield break; 
                        }
                        else
                        {
                            timeStarted = Time.realtimeSinceStartup;
                            scheduler.completed = false;
                            yield return null;
                        }
                    }
                }    
        }
    }
}