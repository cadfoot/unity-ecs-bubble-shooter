using System.Collections.Generic;
using Game.View;
using UnityEngine;

namespace Game
{
    public interface IConfig
    {
        Vector2Int BoardSize { get; }
        BubbleView BubbleView { get; }
        MergePopupText MergePopupText { get; }
        IReadOnlyList<BubbleData> BubbleData { get; }
        
        int RowsMax { get; }
        int RowsMin { get; }
        
        float BubbleMoveSpeed { get; }
        float BubbleFlySpeed { get; }
    }
}
