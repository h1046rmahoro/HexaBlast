using UnityEngine;

namespace SB
{
    /// <summary>
    /// 보드의 각 셀 
    /// </summary>
    public class Cell
    {
        private Hexagon _hexagon = null;

        public Cell(Vector2 position)
        {
            _hexagon = new Hexagon(position);
        }
    }
}
