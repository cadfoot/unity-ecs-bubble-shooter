using System;
using Client;
using Game.Ecs.Components;
using Game.Ecs.Systems;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs
{
    [RequireComponent(typeof(SceneContext))]
    sealed class Startup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;

        [SerializeField] private Config _config = default;
        [SerializeField] private bool _useSeed = default;
        [SerializeField] private int _randomSeed = default;
        
        private void Start()
        {
            Application.targetFrameRate = 60;
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
#if UNITY_EDITOR
            //Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            //Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
            _systems
                .Add(new BoardInitSystem())
                .Add(new CameraInitSystem())
                .Add(new BackgroundInitSystem())
                .Add(new BoardPhysicsBoundsInitSystem())

                .Add(new InputSystem())
                
                .Add(new TrajectorySystem())
                .Add(new NextBubbleSystem())
                
                .Add(new BubbleConnectionSystem())
                .Add(new BubbleFallSystem())
                
                .Add(new BubbleMergeSystem())
                .Add(new BubbleExplodeSystem())
                
                .Add(new ShootSystem())
                
                .Add(new BubbleFlowSystem())

                .Add(new NextBubbleViewSystem())
                .Add(new CreateBubbleViewSystem())
                .Add(new PredictionViewUpdateSystem())
                .Add(new TrajectoryViewUpdateSystem())
                .Add(new BubbleFallDeathSystem())
                
                .Add(new BubbleViewMergeSystem())
                .Add(new BubbleViewMoveSystem())
                .Add(new BubbleViewFlySystem())
                .Add(new BubbleViewFallSystem())
                .Add(new BubbleViewShakeSystem())
                
                .Add(new MergeTextSpawnSystem())
                .Add(new PerfectNotificationSystem())
                .Add(new ComboMergeNotificationSystem())
                
                .Add(new BubbleViewTweeningMarkSystem())
                .Add(new BubbleCompleteMergeSystem())

                .Add(new BubbleViewHangingDestroySystem())

                .Add(new InputClearSystem())
                .Add(new TrajectoryClearSystem())
                
                .OneFrame<Prediction>()
                .OneFrame<Connected>()
                .OneFrame<Created>()
                .OneFrame<WorldPosition>()
                .OneFrame<Destroyed>()
                .OneFrame<New>()

                .Inject(GetComponent<ISceneContext>())
                .Inject((IConfig)_config)
                .Inject((IRandomService)new RandomService(_useSeed ? _randomSeed : (int?) null))
                
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }
    }
}
