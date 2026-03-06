using BovineLabs.Core.Utility;
using BovineLabs.Timeline.Data.Schedular;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Timeline;

namespace BovineLabs.Timeline.Authoring
{
    public abstract class TimelineTrackBaker
    {
        public abstract void BakeTrack(TrackAsset track, BakingContext context, ActiveRange range);
    }

    public abstract class TimelineTrackBaker<TTrack> : TimelineTrackBaker where TTrack : TrackAsset
    {
        public abstract void BakeTrack(TTrack track, BakingContext context, ActiveRange range);

        public override void BakeTrack(TrackAsset track, BakingContext context, ActiveRange range)
        {
            BakeTrack((TTrack)track, context, range);
        }
    }

    public static class BakerTypeManager
    {
        static readonly Dictionary<Type, Type> Bakers = new();

        public static bool TryGetBaker(Type assetType, out Type bakerType)
        {
            while(assetType != typeof(TrackAsset))
            {
                if(Bakers.TryGetValue(assetType, out bakerType))
                    return true;

                assetType = assetType.BaseType;
            }

            bakerType = null;
            return false;
        }

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            var current = typeof(BakerTypeManager).Assembly;

            var bakerToBaked = new Dictionary<Type, Type>();

            foreach(var asm in AppDomain.CurrentDomain.GetAssemblies().Where(asm => asm == current || asm.IsAssemblyReferencingAssembly(current)))
            {
                foreach(var bakerType in asm.GetTypes().Where(type => type != typeof(TimelineTrackBaker) && typeof(TimelineTrackBaker).IsAssignableFrom(type)))
                {
                    var bakedType = bakerType.BaseType.GenericTypeArguments[0];
                    bakerToBaked[bakerType] = bakedType;
                    break;
                }
            }

            foreach(var (baker, baked) in bakerToBaked)
            {
                var baked_ = baked;

                while(baked_ != typeof(TrackAsset))
                {
                    if(!Bakers.TryAdd(baked_, baker))
                    {
                        // conflict; use the most specific compatible baker
                        if(bakerToBaked[Bakers[baked_]].IsAssignableFrom(baked))
                        {
                            Bakers[baked_] = baker;
                        }
                    }

                    baked_ = baked_.BaseType;
                }
            }
        }
    }
}