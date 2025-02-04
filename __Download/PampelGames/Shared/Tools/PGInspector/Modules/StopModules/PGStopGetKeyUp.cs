// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopGetKeyUp : PGStopClassBase
    {

        public override string ModuleName()
        {
            return "Get Key Up";
        }
        public override string ModuleInfo()
        {
            return "Stops when Input.GetKeyUp() recognizes the specified key.";
        }

        [Tooltip("Identifier of the Key that needs to be released.")]
        public KeyCode keyCode = KeyCode.Space;

        private Coroutine checkKeyCodeCoroutine;
        
        public override void ExecutionStart(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ExecutionStart(baseComponent, StopAction);
            checkKeyCodeCoroutine = baseComponent.StartCoroutine(_GetKeyUpStopCheck(StopAction));
        }
        public override void ExecutionStop(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ExecutionStop(baseComponent, StopAction);
            if(checkKeyCodeCoroutine != null) baseComponent.StopCoroutine(checkKeyCodeCoroutine);
        }
        
        private IEnumerator _GetKeyUpStopCheck(Action StopAction)
        {
            for (;;)
            {
                if (isPaused) yield return null;
                if (Input.GetKeyUp(keyCode))
                    StopAction();
                yield return null;
            }
        }
    }
}