// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.Shared.Tools.PGInspector
{
    public class PGStopOnTriggerEnter : PGStopClassBase
    {

        public override string ModuleName()
        {
            return "On Trigger Enter";
        }
        
        public override string ModuleInfo()
        {
            return "Stops when an attached trigger collider collides with another.";
        }

        [Tooltip("Layer Filter: Only stop when one of the specified Layers matches.")]
        public bool useLayerFilter;
        
        public LayerMask matchingLayers;

        [Tooltip("Tag Filter: Only stop when one of the specified Tags matches.")]
        public bool useTagFilter;

        public List<string> matchingTags = new();
        
        public override void ComponentOnTriggerEnter(MonoBehaviour baseComponent, Action StopAction, Collider other)
        {
            base.ComponentOnTriggerEnter(baseComponent, StopAction, other);
            
            if (useLayerFilter && matchingLayers != (matchingLayers | (1 << other.transform.gameObject.layer)) && !useTagFilter) return;
            if (useTagFilter && !matchingTags.Contains(other.gameObject.tag)) return;
            StopAction();
        }
        
    }
}