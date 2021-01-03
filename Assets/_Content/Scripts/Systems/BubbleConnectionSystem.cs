using System.Collections.Generic;
using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleConnectionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IConfig _config = default;

        private readonly EcsFilter<Bubble, Position>.Exclude<Next> _bubbleFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;

        private Dictionary<Vector2Int, EcsEntity> _map;
        private (List<EcsEntity> a, List<EcsEntity> b) _cache;

        void IEcsInitSystem.Init()
        {
            var size = _config.BoardSize.x * _config.BoardSize.y;
            _map = new Dictionary<Vector2Int, EcsEntity>(_config.BoardSize.x * _config.BoardSize.y);
            
            _cache.a = new List<EcsEntity>(size);
            _cache.b = new List<EcsEntity>(size);
        }
        
        void IEcsRunSystem.Run()
        {
            if (!_movingFilter.IsEmpty())
            {
                return;
            }
            
            UpdateBubbleMap();
            
            _cache.a.Clear();
            _cache.b.Clear();

            Hex.GetColBounds(0, _config.BoardSize.x, out var start, out var end);

            foreach (var i in _bubbleFilter)
            {
                if (_bubbleFilter.Get2(i).Value.y != 0)
                {
                    continue;
                }
                var bubble = _bubbleFilter.GetEntity(i);
                bubble.Get<Connected>();
                _cache.a.Add(bubble);
            }

            while (_cache.a.Count > 0)
            {
                _cache.b.Clear();
                foreach (var bubble in _cache.a)
                {
                    var position = bubble.Get<Position>().Value;
                    foreach (var offset in Hex.NeighboursOffsets)
                    {
                        var neighbour = GetBubbleFromMap(position + offset);
                        if (neighbour.IsNull() || neighbour.Has<Connected>())
                        {
                            continue;
                        }

                        neighbour.Get<Connected>();
                        _cache.b.Add(neighbour);
                    }
                }

                (_cache.a, _cache.b) = (_cache.b, _cache.a);
            }
        }

        private void UpdateBubbleMap()
        {
            _map.Clear();
            foreach (var i in _bubbleFilter)
            {
                var position = _bubbleFilter.Get2(i).Value;
                _map[position] = _bubbleFilter.GetEntity(i);
            }
        }

        private EcsEntity GetBubbleFromMap(Vector2Int coord)
        {
            if (!_map.TryGetValue(coord, out var bubble))
            {
                bubble = EcsEntity.Null;
            }
            return bubble;
        }
    }
}
