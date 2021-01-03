using Game.Ecs.Components;
using Leopotam.Ecs;

namespace Game.Ecs.Systems
{
    public sealed class TrajectoryViewUpdateSystem : IEcsRunSystem
    {
        private readonly ISceneContext _scene = null;

        private readonly EcsFilter<Prediction> _prediction = null;
        private readonly EcsFilter<Trajectory, WorldPosition> _trajectory = null;

        void IEcsRunSystem.Run()
        {
            _scene.TrajectoryRenderer.enabled = !_prediction.IsEmpty();

            var trajectoryCount = _trajectory.GetEntitiesCount();
            
            _scene.TrajectoryRenderer.positionCount = trajectoryCount;

            for (int i = 0; i < trajectoryCount; i++)
            {
                _scene.TrajectoryRenderer.SetPosition(i,  _trajectory.Get2(i).Value);
            }
        }
    }
}
