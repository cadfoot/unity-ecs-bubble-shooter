using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class MergeTextSpawnSystem : IEcsRunSystem
    {
        private readonly IConfig _config = default;

        private readonly EcsFilter<Bubble, Merge, Position>.Exclude<Created> _mergeBubbleFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;
        
        void IEcsRunSystem.Run()
        {
            if (!_movingFilter.IsEmpty())
            {
                return;
            }
            
            foreach (var i in _mergeBubbleFilter)
            {
                var value = _mergeBubbleFilter.Get1(i).Value;
                var position = Hex.ToWorldPosition(_mergeBubbleFilter.Get3(i).Value);

                var text = Object.Instantiate(_config.MergePopupText, position, Quaternion.identity);
                
                if (value < 1024)
                {
                    text._text.SetText("{0}", value);
                }
                else
                {
                    text._text.SetText("{0}K", value / 1024f);
                }
            }
        }
    }
}
