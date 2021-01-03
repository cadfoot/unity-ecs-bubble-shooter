using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleFlowSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private readonly IConfig _config = default;
        private readonly IRandomService _random = default;
        
        private readonly EcsFilter<Bubble, Position>.Exclude<Created> _bubbleFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;
        private readonly EcsFilter<Merge> _mergeFilter = default;

        void IEcsRunSystem.Run()
        {
            if (!_movingFilter.IsEmpty() || !_mergeFilter.IsEmpty())
            {
                return;
            }

            var rowCount = GetRowCount();
            
            if (rowCount > _config.RowsMax)
            {
                foreach (var i in _bubbleFilter)
                {
                    ref var bubblePosition = ref _bubbleFilter.Get2(i);
                    bubblePosition.Value.y--;
                    if (bubblePosition.Value.y < 0)
                    {
                        _bubbleFilter.GetEntity(i).Del<Bubble>();
                        _bubbleFilter.GetEntity(i).Del<Position>();
                    }
                }
            }
            else if (rowCount < _config.RowsMin)
            {
                foreach (var i in _bubbleFilter)
                {
                    _bubbleFilter.Get2(i).Value.y++;
                }
                PopulateRow(0, !IsTopRowShifted());
            }
        }

        private void PopulateRow(int row, bool shift)
        {
            Hex.GetColBounds(row, _config.BoardSize.x, out var start, out var end);

            if (shift)
            {
                start += 1;
                end += 1;
            }

            for (var x = start; x < end; x += 2)
            {
                var random = _random.Range(0, 6);
                _world.NewEntity()
                    .Replace(new Bubble {Value = _config.BubbleData[random].Number})
                    .Replace(new Position {Value = new Vector2Int(x, row)});
            }
        }

        private bool IsTopRowShifted()
        {
            if (_bubbleFilter.IsEmpty())
                return false;
            var topPosition = Vector2Int.one * int.MaxValue;
            foreach (var i in _bubbleFilter)
            {
                var position = _bubbleFilter.Get2(i).Value;
                if (position.y < topPosition.y ||
                    position.y == topPosition.y && position.x < topPosition.x)
                {
                    topPosition = position;
                }
            }

            return topPosition.x != 0;
        }

        private int GetRowCount()
        {
            if (_bubbleFilter.IsEmpty())
            {
                return 0;
            }
            var result = 0;
            foreach (var i in _bubbleFilter)
            {
                var bubbleRow = _bubbleFilter.Get2(i).Value.y;
                if (bubbleRow > result)
                {
                    result = bubbleRow;
                }
            }
            return result + 1;
        }
    }
}
