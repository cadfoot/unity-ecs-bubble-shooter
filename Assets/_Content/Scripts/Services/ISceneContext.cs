using Game.View;
using UnityEngine;

namespace Game
{
    public interface ISceneContext
    {
        Camera Camera { get; }
        SpriteRenderer Background { get; }
        Transform BubbleViewRoot { get; }
        LineRenderer TrajectoryRenderer { get; }
        BubbleView PredictionView { get; }
        ParticleSystem DestroyParticles { get; }
    }
}
