using System.Collections.Generic;
using DG.Tweening;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleViewFlySystem : IEcsRunSystem
    {
        private readonly IConfig _config = default;
        
        private readonly EcsFilter<Created, UnityObject<BubbleView>> _bubbleFilter = default;
        private readonly EcsFilter<Trajectory, WorldPosition> _trajectoryFilter = default;
        private readonly EcsFilter<Prediction, Position> _predictionFilter = default;

        private List<Vector3> _path = new List<Vector3>();

        void IEcsRunSystem.Run()
        {
            if (_bubbleFilter.IsEmpty() || _trajectoryFilter.IsEmpty() || _predictionFilter.IsEmpty())
            {
                return;
            }
            
            _path.Clear();
            
            GetFlyPath(_path);

            var bubbleEntity = _bubbleFilter.GetEntity(0);
            var bubbleView = _bubbleFilter.Get2(0).Value;
            bubbleView.Trail.enabled = true;

            bubbleView.transform.position = _path[0];

            bubbleView.DOComplete();
            bubbleView.transform.DOPath(_path.ToArray(), _config.BubbleFlySpeed)
                .SetEase(Ease.Linear)
                .SetSpeedBased(true)
                .OnComplete(() =>
                {
                    bubbleEntity.Get<New>();
                    bubbleView.Trail.enabled = false;
                });
        }

        private void GetFlyPath(List<Vector3> path)
        {
            foreach (var i in _trajectoryFilter)
            {
                path.Add(_trajectoryFilter.Get2(i).Value);
            }
            
            path.RemoveAt(_path.Count - 1);
            path.Add(Hex.ToWorldPosition(_predictionFilter.Get2(0).Value));
        }
    }
}
