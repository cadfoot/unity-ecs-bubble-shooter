using DG.Tweening;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleViewMoveSystem : IEcsRunSystem
    {
        private readonly IConfig _config = default;
            
        private readonly EcsFilter<Bubble, UnityObject<BubbleView>>.Exclude<Moving, Created> _bubbleFilter = default;
        
        void IEcsRunSystem.Run()
        {
            foreach (var i in _bubbleFilter)
            {
                var entity = _bubbleFilter.GetEntity(i);
                var view = _bubbleFilter.Get2(i).Value;
                Vector2 position;

                if (entity.Has<WorldPosition>())
                {
                    position = entity.Get<WorldPosition>().Value;
                }
                else if (entity.Has<Position>())
                {
                    position = Hex.ToWorldPosition(entity.Get<Position>().Value);
                }
                else
                {
                    continue;
                }

                if ((Vector2)view.transform.position == position)
                {
                    continue;
                }

                view.transform.DOComplete();
                view.transform.DOLocalMove(position, _config.BubbleMoveSpeed)
                    .SetEase(Ease.Linear)
                    .SetSpeedBased(true);
            }
        }
    }
}
