using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class BubbleFallSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Bubble, Position>.Exclude<Connected> _bubbleFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;

        void IEcsRunSystem.Run()
        {
            if (!_movingFilter.IsEmpty())
            {
                return;
            }
            
            foreach (var i in _bubbleFilter)
            {
                var entity = _bubbleFilter.GetEntity(i);
                entity.Del<Position>();
                entity.Del<Merge>();
                entity.Get<Falling>();
            }
        }
    }
}
