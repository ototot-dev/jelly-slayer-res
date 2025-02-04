// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    /// <summary>
    ///     Extension methods for <see cref="PGEditorSchedulerDescr" />.
    /// </summary>
    public static class PGEditorSchedulerExtensions
    {
        
        public static void Stop(this PGEditorSchedulerDescr editorScheduler)
        {
            editorScheduler.stopped = true;
        }
    }
}