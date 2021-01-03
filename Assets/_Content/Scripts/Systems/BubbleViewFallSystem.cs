using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    sealed class BubbleViewFallSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Falling, UnityObject<BubbleView>> _bubbleFilter = default;
        
        void IEcsRunSystem.Run()
        {
            foreach (var i in _bubbleFilter)
            {
                var bubbleView = _bubbleFilter.Get2(i).Value;

                bubbleView.Collider.enabled = false;

                if (bubbleView.Rigidbody.bodyType == RigidbodyType2D.Static)
                {
                    bubbleView.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    bubbleView.Rigidbody.AddForce(Vector2.right * Random.Range(-2f, 2f), ForceMode2D.Impulse);    
                }
            }
        }
    }
}
