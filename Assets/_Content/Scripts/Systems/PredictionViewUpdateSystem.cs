using DG.Tweening;
using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class PredictionViewUpdateSystem : IEcsRunSystem
    {
        private readonly ISceneContext _scene = default;
        private readonly IConfig _config = default;

        private readonly EcsFilter<Prediction, Position> _predictionFilter = default;
        private readonly EcsFilter<Bubble, Next> _bubbleFilter = default;

        void IEcsRunSystem.Run()
        {
            if (_predictionFilter.IsEmpty() || _bubbleFilter.IsEmpty())
            {
                _scene.PredictionView.Renderer.enabled = false;
                return;
            }

            _scene.PredictionView.Renderer.enabled = true;

            var color = GetColorForNumber(_bubbleFilter.Get1(0).Value);
            color.a = _scene.PredictionView.Renderer.color.a;
            _scene.PredictionView.Renderer.color = color;

            var newPosition = Hex.ToWorldPosition(_predictionFilter.Get2(0).Value);
            var predictionTransform =_scene.PredictionView.transform;
            if (newPosition != (Vector2)predictionTransform.position)
            {
                predictionTransform.position = newPosition;
                predictionTransform.DOKill();
                predictionTransform.localScale = Vector3.one * .5f;
                predictionTransform.DOScale(Vector3.one, .25f);
            }
        }

        private Color GetColorForNumber(int number)
        {
            foreach (var bubbleData in _config.BubbleData)
            {
                if (bubbleData.Number == number)
                {
                    return bubbleData.Color;
                }
            }
            return default;
        }
    }
}
