using System.Collections.Generic;
using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleMergeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private readonly IConfig _config = default;

        private readonly EcsFilter<Bubble, Merge, Position> _newBubbleFilter = default;
        private readonly EcsFilter<Bubble, Position> _bubbleFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;

        private Dictionary<Vector2Int, EcsEntity> _map;
        private HashSet<EcsEntity> _toMerge;

        void IEcsInitSystem.Init()
        {
            var boardSize = _config.BoardSize.x * _config.BoardSize.y;
            _map = new Dictionary<Vector2Int, EcsEntity>(boardSize);
            _toMerge = new HashSet<EcsEntity>();
        }
        
        void IEcsRunSystem.Run()
        {
            if (!_movingFilter.IsEmpty() || _newBubbleFilter.IsEmpty())
            {
                return;
            }
            
            UpdateMap();

            var mergeIndex = _newBubbleFilter.Get2(0).Index;
            _newBubbleFilter.GetEntity(0).Del<Merge>();

            var newBubblePosition = _newBubbleFilter.Get3(0).Value;
            var newBubbleValue = _newBubbleFilter.Get1(0).Value;

            _toMerge.Clear();
            GetBubblesToMerge(newBubblePosition, newBubbleValue, _toMerge);

            if (_toMerge.Count == 0)
            {
                return;
            }

            var mergedValue = newBubbleValue * (1 << _toMerge.Count - 1);
            if (mergedValue > 2048)
            {
                mergedValue = 2048;
            }

            var mergeBubble = GetMergeBubble(_toMerge, mergedValue);
            var mergePosition = mergeBubble.Get<Position>().Value;

            foreach (var bubble in _toMerge)
            {
                bubble.Get<Merging>().Target = mergePosition;
                bubble.Del<Position>();
            }

            var newBubble = _world.NewEntity();
            newBubble.Get<Bubble>().Value = mergedValue;
            newBubble.Get<Position>().Value = mergePosition;
            newBubble.Get<Merge>().Index = mergeIndex + 1;
        }

        private void UpdateMap()
        {
            _map.Clear();

            foreach (var i in _bubbleFilter)
            {
                _map[_bubbleFilter.Get2(i).Value] = _bubbleFilter.GetEntity(i);
            }
        }

        private void GetBubblesToMerge(Vector2Int position, int value, HashSet<EcsEntity> result)
        {
            foreach (var offset in Hex.NeighboursOffsets)
            {
                var neighbourPosition = position + offset;
                if (!_map.TryGetValue(neighbourPosition, out var neighbour))
                {
                    continue;
                }

                var neighbourValue = neighbour.Get<Bubble>().Value;
                if (neighbourValue == value && result.Add(neighbour))
                {
                    GetBubblesToMerge(neighbourPosition, neighbourValue, result);
                }
            }
        }

        private EcsEntity GetMergeBubble(HashSet<EcsEntity> toMerge, int mergedValue)
        {
            var bestBubbleMergeableNeighbourCount = 0;
            var bestBubble = EcsEntity.Null;
            var highestBubble = EcsEntity.Null;
            
            foreach (var bubble in toMerge)
            {
                var bubblePosition = bubble.Get<Position>().Value;

                highestBubble = highestBubble.IsNull() ? bubble : highestBubble;
                var highestBubblePosition = highestBubble.Get<Position>().Value;
                if (bubblePosition.y < highestBubblePosition.y ||
                    bubblePosition.y == highestBubblePosition.y && bubblePosition.x < highestBubblePosition.x)
                {
                    highestBubble = bubble;
                }

                var mergeableNeighbours = 0;
                foreach (var offset in Hex.NeighboursOffsets)
                {
                    if (!_map.TryGetValue(bubblePosition + offset, out var neighbour))
                    {
                        continue;
                    }
                    
                    if (neighbour.Get<Bubble>().Value == mergedValue)
                    {
                        mergeableNeighbours++;
                    }
                }

                if (mergeableNeighbours > bestBubbleMergeableNeighbourCount)
                {
                    bestBubble = bubble;
                    bestBubbleMergeableNeighbourCount = mergeableNeighbours;
                }
            }

            return bestBubble.IsNull() ? highestBubble : bestBubble;
        }
    }
}
