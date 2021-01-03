using System.Collections.Generic;
using Game.View;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Config", menuName = "Game/Config")]
    public sealed class Config : ScriptableObject, IConfig
    {
        [SerializeField] private Vector2Int _boardSize = default;
        [SerializeField] private BubbleView _bubbleView = default;
        [SerializeField] private BubbleData[] _bubbleData = default;
        [SerializeField] private int _rowsMax = default;
        [SerializeField] private int _rowsMin = default;
        [SerializeField] private MergePopupText _mergePopupText = default;
        [SerializeField] private float _bubbleMoveSpeed = default;
        [SerializeField] private float _bubbleFlySpeed = default;

        public Vector2Int BoardSize => _boardSize;
        public BubbleView BubbleView => _bubbleView;

        public MergePopupText MergePopupText => _mergePopupText;

        public IReadOnlyList<BubbleData> BubbleData => _bubbleData;

        public int RowsMax => _rowsMax;
        public int RowsMin => _rowsMin;

        public float BubbleMoveSpeed => _bubbleMoveSpeed;
        public float BubbleFlySpeed => _bubbleFlySpeed;
    }
}
