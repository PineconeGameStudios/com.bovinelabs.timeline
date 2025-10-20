// <copyright file="Clip.cs" company="BovineLabs">
//     Copyright (c) BovineLabs. All rights reserved.
// </copyright>

namespace BovineLabs.Timeline.Data
{
    using Unity.Entities;

    public struct Clip : IComponentData
    {
        // The track entity the clip belongs to
        public Entity Track;
    }
}
