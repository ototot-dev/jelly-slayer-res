// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteGetKeyUp : PGExecuteClassBase
    {

        public override string ModuleName()
        {
            return "Get Key Up";
        }
        public override string ModuleInfo()
        {
            return "Starts when Input.GetKeyUp() recognizes the specified key.";
        }

        [Tooltip("Identifier of the Key that needs to be released.")]
        public KeyCode keyCode = KeyCode.Space;

        private Coroutine checkKeyCodeCoroutine;
        
        public override void ComponentOnEnable(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnEnable(baseComponent, ExecuteAction);
            checkKeyCodeCoroutine = baseComponent.StartCoroutine(_GetKeyUpStartCheck(ExecuteAction));
        }
        
        private IEnumerator _GetKeyUpStartCheck(Action ExecuteAction)
        {
            for (;;)
            {
                if (isPaused) yield return null;
                if (Input.GetKeyUp(keyCode))
                    ExecuteAction();
                yield return null;
            }
        }
    }
}