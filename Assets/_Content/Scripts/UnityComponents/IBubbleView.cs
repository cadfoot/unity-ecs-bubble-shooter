using UnityEngine;

namespace Game.View
{
    public interface IBubbleView
    {
        void UpdateFrom(BubbleData data);
        void SetPosition(Vector3 position);
    }
}