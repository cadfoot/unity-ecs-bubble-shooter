using Client;
using DG.Tweening;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BubbleViewHangingDestroySystem : IEcsRunSystem
    {
        private readonly ISceneContext _scene = default;

        private readonly EcsFilter<UnityObject<BubbleView>>.Exclude<Bubble, Moving> _filter = default;
        
        void IEcsRunSystem.Run()
        {
            foreach (var i in _filter)
            {
                var view = _filter.Get1(i).Value;

                DOTween.Complete(view.transform);
                
                var destroyed = _filter.GetEntity(i).Has<Destroyed>();
                if (destroyed)
                {
                    var particleParams = new ParticleSystem.EmitParams
                    {
                        position = view.transform.localPosition,
                        startColor = view.Renderer.color,
                        applyShapeToPosition = true
                    };

                    _scene.DestroyParticles.Emit(particleParams, 10);
                }

                _filter.GetEntity(i).Del<UnityObject<BubbleView>>();
                Object.Destroy(view.gameObject);
            }
        }
    }
}
