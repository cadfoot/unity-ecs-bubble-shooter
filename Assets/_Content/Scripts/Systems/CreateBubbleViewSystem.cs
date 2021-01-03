using DG.Tweening;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class CreateBubbleViewSystem : IEcsRunSystem
    {
        private readonly IConfig _config = null;
        private readonly ISceneContext _scene = null;

        private readonly EcsFilter<Bubble>.Exclude<UnityObject<BubbleView>> _bubbles = default;
        
        void IEcsRunSystem.Run()
        {
            foreach (var i in _bubbles)
            {
                var entity = _bubbles.GetEntity(i);
                var value = _bubbles.Get1(i).Value;
                
                Vector3 worldPosition;
                var isOnGrid = false;
                if (entity.Has<Position>())
                {
                    worldPosition = Hex.ToWorldPosition(entity.Get<Position>().Value);
                    isOnGrid = true;
                }
                else if (entity.Has<WorldPosition>())
                {
                    worldPosition = entity.Get<WorldPosition>().Value;
                    entity.Del<WorldPosition>();
                }
                else
                {
                    continue;
                }
                
                var view = Object.Instantiate(_config.BubbleView, worldPosition, Quaternion.identity,
                    _scene.BubbleViewRoot);
                view.Renderer.color = GetColorForValue(value);
                view.SetNumber(value);

                var duration = isOnGrid ? Random.Range(.4f, .7f) : .25f;

                view.transform.localScale = Vector3.zero;
                view.transform.DOScale(Vector3.one, duration);

                _bubbles.GetEntity(i).Get<UnityObject<BubbleView>>().Value = view;
            }
        }

        private Color GetColorForValue(int value)
        {
            foreach (var data in _config.BubbleData)
            {
                if (data.Number == value)
                {
                    return data.Color;
                }
            }
            return Color.white;
        }
    }
}
