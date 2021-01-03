using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class InputClearSystem : IEcsRunSystem
    {
        private readonly EcsFilter<InputHeld> _heldFilter = null;
        private readonly EcsFilter<InputReleased> _releasedFilter = null;
        
        void IEcsRunSystem.Run()
        {
            foreach (var i in _heldFilter)
            {
                _heldFilter.GetEntity(i).Destroy();
            }
            
            foreach (var i in _releasedFilter)
            {
                _releasedFilter.GetEntity(i).Destroy();
            }
        }
    }
}