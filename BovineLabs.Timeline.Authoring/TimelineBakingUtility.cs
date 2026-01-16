// <copyright file="TimelineBakingUtility.cs" company="BovineLabs">
//     Copyright (c) BovineLabs. All rights reserved.
// </copyright>

namespace BovineLabs.Timeline.Authoring
{
    using Unity.Collections;
    using UnityEditor;

    /// <summary>
    /// Utility methods for timeline baking operations.
    /// </summary>
    public static class TimelineBakingUtility
    {
        /// <summary>
        /// Converts a DOTS track into a unique identifier used for track binding lookups.
        /// </summary>
        /// <param name="dotsTrack">The track to get an identifier for.</param>
        /// <returns>A unique identifier for the track.</returns>
        public static FixedString128Bytes TrackToIdentifier(DOTSTrack dotsTrack)
        {
            var globalId = GlobalObjectId.GetGlobalObjectIdSlow(dotsTrack);
            return globalId.ToString();
        }
    }
}
