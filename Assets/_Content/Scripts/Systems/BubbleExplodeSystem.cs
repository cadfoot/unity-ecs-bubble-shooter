using Client;
using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public class BubbleExplodeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Bubble, Position> _bubbleFilter = default;
        private readonly EcsFilter<Merging> _mergingFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;
        
        void IEcsRunSystem.Run()
        {
            if (!_mergingFilter.IsEmpty() || !_movingFilter.IsEmpty())
            {
                return;
            }

            foreach (var i in _bubbleFilter)
            {
                var value = _bubbleFilter.Get1(i).Value;
                if (value < 2048)
                {
                    continue;
                }
                
                var position = _bubbleFilter.Get2(i).Value;
                foreach (var offset in Hex.NeighboursOffsets)
                {
                    var neighbourPosition = position + offset;
                    foreach (var j in _bubbleFilter)
                    {
                        if (_bubbleFilter.Get2(j).Value != neighbourPosition)
                        {
                            continue;
                        }
                        var bubble = _bubbleFilter.GetEntity(j);
                        bubble.Del<Bubble>();
                        bubble.Del<Position>();
                        bubble.Get<Destroyed>();
                        break;
                    }
                }
                var self = _bubbleFilter.GetEntity(i);
                self.Del<Bubble>();
                self.Del<Position>();
                self.Del<Merge>();
                self.Get<Destroyed>();
            }
        }
    }
}
