using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class NextBubbleViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IConfig _config = default;

        private readonly EcsFilter<Bubble, Next>.Exclude<Moving> _bubbleFilter = default;
        
        private Vector2 _origin;

        void IEcsInitSystem.Init()
        {
            int colMin, colMax, rowMax;
            rowMax = _config.BoardSize.y - 1;
            
            Hex.GetColBounds(0, _config.BoardSize.x, out colMin, out colMax);
            
            var topLeft = Hex.ToWorldPosition(new Vector2Int(colMin, 0));
            var topRight = Hex.ToWorldPosition(new Vector2Int(colMax, 0));

            Hex.GetColBounds(rowMax, _config.BoardSize.x, out colMin, out colMax);

            var bottomRight = Hex.ToWorldPosition(new Vector2Int(colMax, rowMax));
            
            _origin = new Vector2((topLeft.x + topRight.x) * .5f, bottomRight.y);
        }

        void IEcsRunSystem.Run()
        {
            foreach (var i in _bubbleFilter)
            {
                var index = _bubbleFilter.Get2(i).Index;
                _bubbleFilter.GetEntity(i).Get<WorldPosition>().Value = _origin + Vector2.left * index;
            }
        }
    }
}
