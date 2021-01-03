using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class ShootSystem : IEcsRunSystem
    {
        private readonly EcsFilter<InputReleased> _inputFilter = default;
        private readonly EcsFilter<Trajectory> _trajectoryFilter = default;
        private readonly EcsFilter<Prediction, Position> _predictionFilter = default;
        private readonly EcsFilter<Bubble, Next> _nextBubbleFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;

        void IEcsRunSystem.Run()
        {
            if (!_movingFilter.IsEmpty())
            {
                return;
            }
            
            if (_inputFilter.IsEmpty() || _trajectoryFilter.IsEmpty() || _predictionFilter.IsEmpty())
            {
                return;
            }

            var bubble = _nextBubbleFilter.GetEntity(0);
            
            bubble.Get<Position>().Value = _predictionFilter.Get2(0).Value;
            bubble.Get<Created>();
            bubble.Get<Merge>();
            bubble.Del<Next>();
        }
    }
}
