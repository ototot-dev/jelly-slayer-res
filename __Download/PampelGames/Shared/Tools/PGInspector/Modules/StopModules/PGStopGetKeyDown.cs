// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopGetKeyDown : PGStopClassBase
    {

        public override string ModuleName()
        {
            return "Get Key Down";
        }
        public override string ModuleInfo()
        {
            return "Stops when Input.GetKeyDown() recognizes the specified key.";
        }

        [Tooltip("Identifier of the Key that needs to be pressed.")]
        public KeyCode keyCode = KeyCode.Space;

        private Coroutine checkKeyCodeCoroutine;
        
        public override void ExecutionStart(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ExecutionStart(baseComponent, StopAction);
            checkKeyCodeCoroutine = baseComponent.StartCoroutine(_GetKeyDownStopCheck(StopAction));
        }
        public override void ExecutionStop(MonoBehaviour baseComponent, Action StopAction)
        {
            base.ExecutionStop(baseComponent, StopAction);
            if(checkKeyCodeCoroutine != null) baseComponent.StopCoroutine(checkKeyCodeCoroutine);
        }
        
        private IEnumerator _GetKeyDownStopCheck(Action StopAction)
        {
            for (;;)
            {
                if (isPaused) yield return null;
                if (Input.GetKeyDown(keyCode))
                    StopAction();
                yield return null;
            }
        }
    }
}