using Game.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class InputSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private readonly ISceneContext _scene = default;
        
        void IEcsRunSystem.Run()
        {
            var entity = EcsEntity.Null;

            if (Input.GetMouseButton(0))
            {
                entity = _world.NewEntity();
                entity.Get<InputHeld>();
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                entity = _world.NewEntity();
                entity.Get<InputReleased>();
                entity.Get<InputHeld>();
            }

            if (!entity.IsNull())
            {
                entity.Get<WorldPosition>().Value = _scene.Camera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
}
