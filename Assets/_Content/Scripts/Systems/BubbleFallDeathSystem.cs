using Client;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public class BubbleFallDeathSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IConfig _config = default;
        
        private readonly EcsFilter<Bubble, Falling, UnityObject<BubbleView>> _filter = default;

        private float _borderY;
        
        void IEcsInitSystem.Init()
        {
            _borderY = Hex.ToWorldPosition(new Vector2Int(0, _config.BoardSize.y)).y;
            _borderY += _config.BubbleView.Renderer.sprite.bounds.size.y;
        }
        
        void IEcsRunSystem.Run()
        {
            foreach (var i in _filter)
            {
                var view = _filter.Get3(i).Value;
                if (view.transform.position.y < _borderY)
                {
                    var entity = _filter.GetEntity(i);
                    entity.Get<Destroyed>();
                    entity.Del<Falling>();
                    entity.Del<Bubble>();
                }
            }    
        }
    }
}
