// <copyright file="IAnimatedComponent.cs" company="BovineLabs">
//     Copyright (c) BovineLabs. All rights reserved.
// </copyright>

namespace BovineLabs.Timeline.Data
{
    using Unity.Entities;

    public interface IAnimatedComponent<out T> : IComponentData
        where T : unmanaged
    {
        T DefaultValue { get; }
    }
}
