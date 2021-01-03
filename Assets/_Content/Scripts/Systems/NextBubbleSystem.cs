using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class NextBubbleSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private readonly IConfig _config = default;
        private readonly IRandomService _random = default;

        private readonly EcsFilter<Bubble, Next> _nextBubbleFilter = default;

        void IEcsRunSystem.Run()
        {
            if (!_nextBubbleFilter.IsEmpty())
            {
                return;
            }
            
            var entity = _world.NewEntity();
            var random = _random.Range(0, 6);
            entity.Get<Bubble>().Value = _config.BubbleData[random].Number;
            entity.Get<Next>().Index = 0;
        }
    }
}
