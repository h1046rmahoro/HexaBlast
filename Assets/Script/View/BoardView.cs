using System;
using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _goCell = null;
        
        
        
        /// <summary>
        /// 최초 보드 생성 
        /// </summary>
        /// <param name="cells"> 보드 정보 </param>
        public void InitBoard(List<List<Cell>> cells)
        {
            for (int x = 0; x < cells.Count; ++x)
            {
                for (int y = 0; y < cells[x].Count; ++y)
                {
                    GameObject go = Instantiate(_goCell, transform);
                    go.transform.position = cells[x][y].Position;
                }
            }
        }
    }
}
