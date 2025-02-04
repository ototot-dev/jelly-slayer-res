// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

namespace PampelGames.Shared.Utility
{
    /// <summary>
    ///     Helper interface for modules to make use of shared utility methods.
    /// </summary>
    public interface PGIModule
    {
        public string ModuleName();
        public string ModuleInfo();
    }
}