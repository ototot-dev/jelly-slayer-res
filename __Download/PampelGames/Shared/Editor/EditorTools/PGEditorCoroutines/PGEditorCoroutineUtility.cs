// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;

namespace PampelGames.Shared.Editor.EditorTools
{
    internal static class PGEditorCoroutineUtility
    {
        /// <summary>
        ///     Starts a new Editor Coroutine.
        ///     Note that only 'yield return null' works, but almost immediately.
        ///     Use instead <see cref="PGEditorScheduler"/> or <see cref="PGEditorTween"/>.
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public static PGEditorCoroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = new PGEditorCoroutine(routine);
            PGEditorCoroutineManager.AddCoroutine(coroutine);
            return coroutine;
        }
        
        public static void StopCoroutine(PGEditorCoroutine coroutine)
        {
            PGEditorCoroutineManager.StopCoroutine(coroutine);
        }
        
    }
}