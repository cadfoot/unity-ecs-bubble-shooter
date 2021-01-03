using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class ComboMergeNotificationSystem : IEcsRunSystem
    {
        private readonly IConfig _config = default;
        private readonly ISceneContext _scene = default;

        private readonly EcsFilter<Bubble, Merge> _mergeFilter = default;
        private readonly EcsFilter<Moving> _moving = default;

        void IEcsRunSystem.Run()
        {
            if (!_moving.IsEmpty() || _mergeFilter.IsEmpty())
            {
                return;
            }

            var index = _mergeFilter.Get2(0).Index + 1;
            if (index < 2)
            {
                return;
            }

            var position = _scene.Camera.ViewportToWorldPoint(Vector3.one * .5f);
            var text = Object.Instantiate(_config.MergePopupText, position, Quaternion.identity);
            text._text.SetText("{0}X", index);
            text._text.fontSize = 10;
        }
    }
}
