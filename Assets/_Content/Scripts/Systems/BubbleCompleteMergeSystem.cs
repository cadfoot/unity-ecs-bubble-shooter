using Client;
using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class BubbleCompleteMergeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Bubble, Merging>.Exclude<Moving> _bubbleFilter = default;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _bubbleFilter)
            {
                var entity = _bubbleFilter.GetEntity(i);
                entity.Get<Destroyed>();
                entity.Del<Merging>();
                entity.Del<Bubble>();
            }
        }
    }
}
