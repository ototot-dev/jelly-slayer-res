// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGExecuteGetKeyDown : PGExecuteClassBase
    {

        public override string ModuleName()
        {
            return "Get Key Down";
        }
        public override string ModuleInfo()
        {
            return "Starts when Input.GetKeyDown() recognizes the specified key.";
        }

        [Tooltip("Identifier of the key that needs to be pressed.")]
        public KeyCode keyCode = KeyCode.Space;
        
        public override void ComponentOnEnable(MonoBehaviour baseComponent, Action ExecuteAction)
        {
            base.ComponentOnEnable(baseComponent, ExecuteAction);
            baseComponent.StartCoroutine(_GetKeyDownStartCheck(ExecuteAction));
        }
        
        private IEnumerator _GetKeyDownStartCheck(Action ExecuteAction)
        {
            for (;;)
            {
                if (isPaused) yield return null;
                if (Input.GetKeyDown(keyCode))
                    ExecuteAction();
                yield return null;
            }
        }
    }
}