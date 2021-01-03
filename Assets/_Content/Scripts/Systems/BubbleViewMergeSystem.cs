using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleViewMergeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Merging, UnityObject<BubbleView>> _bubbleFilter = default;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _bubbleFilter)
            {
                var entity = _bubbleFilter.GetEntity(i);
                var mergeWorldPosition = Hex.ToWorldPosition(_bubbleFilter.Get1(i).Target);

                entity.Get<WorldPosition>().Value = mergeWorldPosition;
            }
        }
    }
}
