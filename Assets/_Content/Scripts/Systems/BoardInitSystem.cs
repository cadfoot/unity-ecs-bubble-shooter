using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BoardInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = default;
        private readonly IConfig _config = default;
        private readonly IRandomService _random = default;

        public void Init()
        {
            Random.InitState(2);
            for (int r = 0; r < _config.BoardSize.y / 2; r++)
            {
                Hex.GetColBounds(r, _config.BoardSize.x, out var start, out var end);
                for (var q = start; q < end; q += 2)
                {
                    var bubble = _world.NewEntity();
                    var random = _random.Range(0, 6);
                    bubble.Get<Bubble>().Value = _config.BubbleData[random].Number;
                    bubble.Get<Position>().Value = new Vector2Int(q, r);
                }
            }
        }
    }
}