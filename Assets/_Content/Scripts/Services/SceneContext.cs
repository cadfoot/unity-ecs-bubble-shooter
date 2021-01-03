using Game.View;
using UnityEngine;

namespace Game
{
    public class SceneContext : MonoBehaviour, ISceneContext
    {
        [SerializeField] private Camera _camera = default;
        [SerializeField] private SpriteRenderer _background = default;
        [SerializeField] private Transform _bubbleViewRoot = default;
        [SerializeField] private LineRenderer _trajectoryRenderer = default;
        [SerializeField] private BubbleView _predictionView = default;
        [SerializeField] private ParticleSystem _destroyParticles = default;
        
        public Camera Camera => _camera;
        public SpriteRenderer Background => _background;
        public Transform BubbleViewRoot => _bubbleViewRoot;
        public LineRenderer TrajectoryRenderer => _trajectoryRenderer;
        public BubbleView PredictionView => _predictionView;
        public ParticleSystem DestroyParticles => _destroyParticles;
    }
}
