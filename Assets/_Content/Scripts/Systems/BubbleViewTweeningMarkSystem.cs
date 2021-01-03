using DG.Tweening;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class BubbleViewTweeningMarkSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnityObject<BubbleView>> _viewFilter = default;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _viewFilter)
            {
                var view = _viewFilter.Get1(i).Value;
                var entity = _viewFilter.GetEntity(i);
                if (DOTween.IsTweening(view.transform))
                {
                    entity.Get<Moving>();
                }
                else
                {
                    entity.Del<Moving>();
                }
            }
        }
    }
}