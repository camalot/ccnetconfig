﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core
{
    /// <summary>
    /// Defines the different ways duplicates can be handled in a queue.
    /// </summary>
    /// <remarks>
    /// This type has come directly from the CruiseControl.Net project.
    /// </remarks>
    public enum QueueDuplicateHandlingMode
    {
        /// <summary>
        /// If a duplicate is found, then it should be ignored.
        /// </summary>
        UseFirst,
        /// <summary>
        /// If a duplicate is found and it is not a force build, then the initial item should be removed and the 
        /// new item added (position of the item may change.)
        /// </summary>
        ApplyForceBuildsReAdd,
        /// <summary>
        /// If a duplicate is found and it is not a force build, then the initial item should be replaced with
        /// the new item (position of the item won't change).
        /// </summary>
        ApplyForceBuildsReplace,
    }
}
