using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class TrajectoryClearSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Trajectory> _trajectoryFilter = null;
        private readonly EcsFilter<Prediction> _predictionFilter = null;
        
        void IEcsRunSystem.Run()
        {
            foreach (var i in _trajectoryFilter)
            {
                _trajectoryFilter.GetEntity(i).Destroy();
            }
            
            foreach (var i in _predictionFilter)
            {
                _predictionFilter.GetEntity(i).Destroy();
            }
        }
    }
}