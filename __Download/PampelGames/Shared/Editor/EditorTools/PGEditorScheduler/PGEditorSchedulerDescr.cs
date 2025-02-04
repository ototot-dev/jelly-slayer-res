// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PampelGames.Shared.Editor.EditorTools
{
    public class PGEditorSchedulerDescr
    {
        public bool completed;
        
        internal float duration;
        internal float currentTime;
        internal bool stopped;
        
        public Action onComplete;
    
        // Schedule List
        public bool scheduleList;
        public int currentListItem;
        public List<Action> onCompleteList;
    }
    
}