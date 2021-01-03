using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class PerfectNotificationSystem : IEcsRunSystem
    {
        private readonly IConfig _config = default;
        private readonly ISceneContext _scene = default;

        private readonly EcsFilter<Bubble, Position>.Exclude<Falling, Merging> _bubbleFilter = default;
        private readonly EcsFilter<Moving> _moving = default;

        void IEcsRunSystem.Run()
        {
            if (!_moving.IsEmpty())
            {
                return;
            }
            
            if (_bubbleFilter.IsEmpty())
            {
                var position = _scene.Camera.ViewportToWorldPoint(Vector3.one * .5f);

                var text = Object.Instantiate(_config.MergePopupText, position, Quaternion.identity);
                text._text.fontSize = 10;
                text._text.text = "Perfect!";
            }
        }
    }
}
