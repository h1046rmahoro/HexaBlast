using UnityEngine;

namespace SB
{
    /// <summary>
    /// 보드의 각 셀 
    /// </summary>
    public class Cell
    {
        private Hexagon _hexagon = null;

        public Cell(float x, float y) : this(new Vector2(x, y))
        {
            
        }
        
        public Cell(Vector2 position)
        {
            _hexagon = new Hexagon(position);
        }

        /// <summary>
        /// 좌상단 셀 
        /// </summary>
        public Cell LeftTop = null;
        /// <summary>
        /// 상단 셀
        /// </summary>
        public Cell Top = null;
        /// <summary>
        /// 우상단 셀
        /// </summary>
        public Cell RightTop = null;

        /// <summary>
        /// 좌하단 셀 
        /// </summary>
        public Cell LeftBottom = null;
        /// <summary>
        /// 하단 셀
        /// </summary>
        public Cell Bottom = null;
        /// <summary>
        /// 우하단 셀
        /// </summary>
        public Cell RightBottom = null;

        /// <summary>
        /// 셀 좌표 
        /// </summary>
        public Vector2 Position => _hexagon.Center;
    }
}
