using System.Collections.Generic;
using System.Linq;
using Game.Ecs.Components;
using Game.View;
using Leopotam.Ecs;
using UnityEngine;

namespace Game.Ecs.Systems
{
    public sealed class TrajectorySystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private readonly IConfig _config = default;

        private readonly EcsFilter<InputHeld, WorldPosition> _inputFilter = default;
        private readonly EcsFilter<Bubble, Position, UnityObject<BubbleView>> _bubbleFilter = default;
        private readonly EcsFilter<Moving> _movingFilter = default;

        private Vector2 _origin;
        private string _bubbleTag;

        private List<Vector2> _trajectory; 

        void IEcsInitSystem.Init()
        {
            _trajectory = new List<Vector2>();
            
            int colMin, colMax, rowMax;
            rowMax = _config.BoardSize.y - 1;
            
            Hex.GetColBounds(0, _config.BoardSize.x, out colMin, out colMax);
            
            var topLeft = Hex.ToWorldPosition(new Vector2Int(colMin, 0));
            var topRight = Hex.ToWorldPosition(new Vector2Int(colMax, 0));

            Hex.GetColBounds(rowMax, _config.BoardSize.x, out colMin, out colMax);

            var bottomRight = Hex.ToWorldPosition(new Vector2Int(colMax, rowMax));
            
            _origin = new Vector2((topLeft.x + topRight.x) * .5f, bottomRight.y);

            _bubbleTag = _config.BubbleView.tag;
        }
        
        void IEcsRunSystem.Run()
        {
            if (_inputFilter.IsEmpty() || !_movingFilter.IsEmpty())
            {
                return;
            }
            
            _trajectory.Clear();
            
            var position = _origin;
            var direction = _inputFilter.Get2(0).Value - position;

            var hitBubbleView = HitTest(position, direction, _trajectory);

            if (!hitBubbleView)
            {
                return;
            }
            
            Vector2Int hitBubblePosition = Vector2Int.zero;
            foreach (var i in _bubbleFilter)
            {
                if (_bubbleFilter.Get3(i).Value == hitBubbleView)
                {
                    hitBubblePosition = _bubbleFilter.Get2(i).Value;
                    break;
                }
            }

            var newBubblePosition =
                NewBubblePosition(hitBubblePosition, hitBubbleView.transform.position, _trajectory.Last());

            if (!newBubblePosition.HasValue)
            {
                return;
            }
            
            foreach (var point in _trajectory)
            {
                var trajectory = _world.NewEntity();
                trajectory.Get<Trajectory>();
                trajectory.Get<WorldPosition>().Value = point;
            }
            
            var prediction = _world.NewEntity();
            prediction.Get<Prediction>();
            prediction.Get<Position>().Value = newBubblePosition.Value;
        }

        private Vector2Int? NewBubblePosition(Vector2Int hitBubbleCoord, Vector2 hitViewPosition,
            Vector2 hitPoint)
        {
            if (hitPoint.x <= hitViewPosition.x && hitPoint.y > hitViewPosition.y)
            {
                var left = Hex.GetNeighbourCoord(hitBubbleCoord, Hex.Neighbour.Left);
                if (PositionFree(left))
                {
                    return left;
                }
            }
            if (hitPoint.x > hitViewPosition.x && hitPoint.y > hitViewPosition.y)
            {
                var right = Hex.GetNeighbourCoord(hitBubbleCoord, Hex.Neighbour.Right);
                if (PositionFree(right))
                {
                    return right;
                }
            }
            if (hitPoint.x <= hitViewPosition.x && hitPoint.y <= hitViewPosition.y)
            {
                var bottomLeft = Hex.GetNeighbourCoord(hitBubbleCoord, Hex.Neighbour.BottomLeft);
                if (PositionFree(bottomLeft))
                {
                    return bottomLeft;
                }
            }
            if (hitPoint.x > hitViewPosition.x && hitPoint.y <= hitViewPosition.y)
            {
                var bottomRight = Hex.GetNeighbourCoord(hitBubbleCoord, Hex.Neighbour.BottomRight);
                if (PositionFree(bottomRight))
                {
                    return bottomRight;
                }
            }

            return null;
        }

        private BubbleView HitTest(Vector2 position, Vector2 direction, List<Vector2> trajectory)
        {
            BubbleView hitBubbleView = null;
            var reflectionCount = 0;
            
            trajectory.Add(position);

            while (reflectionCount <= 1)
            {
                var hit = Physics2D.Raycast(position, direction);
                if (!hit.collider)
                {
                    break;
                }

                trajectory.Add(hit.point);

                if (hit.collider.CompareTag(_bubbleTag))
                {
                    hitBubbleView = hit.collider.GetComponentInChildren<BubbleView>();
                    break;
                }

                direction = Vector2.Reflect(direction, hit.normal);
                position = hit.point + hit.normal * 0.01f;

                reflectionCount++;
            }

            return hitBubbleView;
        }

        private bool PositionFree(Vector2Int position)
        {
            foreach (var i in _bubbleFilter)
            {
                if (_bubbleFilter.Get2(i).Value == position)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
