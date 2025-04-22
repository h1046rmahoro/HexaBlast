using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    /// <summary>
    /// 블록 이동 데이터 
    /// </summary>
    public class BlockMoveData
    {
        /// <summary>
        /// 이동 블록 유니크 키 
        /// </summary>
        public int TargetBlockUniqueKey = 0;
        
        /// <summary>
        /// 목표 좌표 
        /// </summary>
        public Vector2 TargetPos = Vector2.zero;

        /// <summary>
        /// 이동후 하단 셀 
        /// </summary>
        public CellModel BottomCell = null;

        /// <summary>
        /// 가속 여부 
        /// </summary>
        public bool IsGravity = false;
    }
}
