using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class CameraInitSystem : IEcsInitSystem
    {
        private readonly ISceneContext _scene = default;
        private readonly IConfig _config = default;
        
        void IEcsInitSystem.Init()
        {
            int colMin, colMax;
            var rowMin = 0;
            var rowMax = _config.BoardSize.y - 1;

            Hex.GetColBounds(rowMin, _config.BoardSize.x, out colMin, out colMax);

            var topLeft = Hex.ToWorldPosition(new Vector2Int(colMin, 0));
            var topRight = Hex.ToWorldPosition(new Vector2Int(colMax, 0));
            
            Hex.GetColBounds(rowMax, _config.BoardSize.x, out colMin, out colMax);
            
            var bottomRight = Hex.ToWorldPosition(new Vector2Int(colMax, rowMax));
            var bottomLeft = Hex.ToWorldPosition(new Vector2Int(0, rowMax));

            Vector3 gridCenter = (topLeft + topRight + bottomRight + bottomLeft) / 4f;

            gridCenter.z = _scene.Camera.transform.position.z;
            _scene.Camera.transform.position = gridCenter;

            var gridWidth = bottomRight.x - bottomLeft.x;

            _scene.Camera.orthographicSize = gridWidth + 2f;
        }
    }
}
