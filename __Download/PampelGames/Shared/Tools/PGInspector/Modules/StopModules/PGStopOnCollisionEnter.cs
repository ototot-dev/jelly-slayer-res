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
    public class PGStopOnCollisionEnter : PGStopClassBase
    {

        public override string ModuleName()
        {
            return "On Collision Enter";
        }
        public override string ModuleInfo()
        {
            return "Stops when an attached non-trigger collider collides with another.";
        }

        [Tooltip("Layer Filter: Only stops when one of the specified Layers matches.")]
        public bool useLayerFilter;
        
        public LayerMask matchingLayers;

        [Tooltip("Tag Filter: Only stops when one of the specified Tags matches.")]
        public bool useTagFilter;

        public List<string> matchingTags = new();
        
        public override void ComponentOnCollisionEnter(MonoBehaviour baseComponent, Action StopAction, Collision collision)
        {
            base.ComponentOnCollisionEnter(baseComponent, StopAction, collision);
            
            if (useLayerFilter && matchingLayers != (matchingLayers | (1 << collision.gameObject.layer)) && !useTagFilter) return;
            if (useTagFilter && !matchingTags.Contains(collision.gameObject.tag)) return;
            StopAction();
        }
        
    }
}