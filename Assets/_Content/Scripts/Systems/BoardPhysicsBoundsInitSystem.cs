using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class BoardPhysicsBoundsInitSystem : IEcsInitSystem
    {
        private readonly IConfig _config = null;

        void IEcsInitSystem.Init()
        {
            var parent = new GameObject("Bounds").transform;

            int colMin, colMax;
            var rowMin = 0;
            var rowMax = _config.BoardSize.y - 1;

            Hex.GetColBounds(rowMin, _config.BoardSize.x, out colMin, out colMax);

            var topLeft = Hex.ToWorldPosition(new Vector2Int(colMin, 0));
            var topRight = Hex.ToWorldPosition(new Vector2Int(colMax, 0));
            
            Hex.GetColBounds(rowMax, _config.BoardSize.x, out colMin, out colMax);
            
            var bottomLeft = Hex.ToWorldPosition(new Vector2Int(colMin, rowMax));
            var bottomRight = Hex.ToWorldPosition(new Vector2Int(colMax, rowMax));

            var left = new GameObject("Left").AddComponent<EdgeCollider2D>();
            left.transform.SetParent(parent);
            left.points = new[] {topLeft, bottomLeft};
            
            var right = new GameObject("Right").AddComponent<EdgeCollider2D>();
            right.transform.SetParent(parent);
            right.points = new[] {topRight, bottomRight};
        }
    }
}
