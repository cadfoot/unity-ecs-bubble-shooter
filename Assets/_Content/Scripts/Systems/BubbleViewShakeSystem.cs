using DG.Tweening;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleViewShakeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnityObject<BubbleView>, Position> _bubbleViewFilter = default;
        private readonly EcsFilter<Bubble, New, Position> _newBubbleFilter = default;
        
        void IEcsRunSystem.Run()
        {
            if (_newBubbleFilter.IsEmpty())
            {
                return;
            }

            var newBubbleCoord = _newBubbleFilter.Get3(0).Value;
            Vector3 newBubbleViewPosition = Hex.ToWorldPosition(newBubbleCoord);

            foreach (var offset in Hex.NeighboursOffsets)
            {
                var neighbourBubbleView = GetBubbleViewAt(newBubbleCoord + offset);
                if (neighbourBubbleView == null)
                {
                    continue;
                }

                var punchDir = (neighbourBubbleView.transform.position - newBubbleViewPosition).normalized * .1f;

                DOTween.Complete(neighbourBubbleView.Renderer.transform);
                neighbourBubbleView.Renderer.transform.DOPunchPosition(punchDir, .1f, 0);
            }
        }
        
        private BubbleView GetBubbleViewAt(Vector2Int coord)
        {
            foreach (var i in _bubbleViewFilter)
            {
                if (_bubbleViewFilter.Get2(i).Value == coord)
                {
                    return _bubbleViewFilter.Get1(i).Value;
                }
            }
            return null;
        }
    }
}
