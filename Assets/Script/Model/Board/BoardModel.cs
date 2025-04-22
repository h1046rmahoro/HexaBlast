using System;
using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class BoardModel
    {
        public const int XCount = 13;
        public const int YCount = 13;
        
        /// <summary>
        /// 셀 리스트 
        /// </summary>
        private List<List<CellModel>> _cells = new List<List<CellModel>>();

        /// <summary>
        /// 셀 데이터  
        /// </summary>
        public List<List<CellModel>> CellDataList => _cells;
        
        public BoardModel()
        {
            var data = LoadMapData(21);

            // 셀 생성 
            CreateCells(data);
            
            // 근처 셀 연결 
            SetNearCellLink();
        }

        private string[] LoadMapData(int level)
        {
            var loadData = Resources.Load<TextAsset>($"MapData/{level}");
            var data = loadData.text.Split(",", StringSplitOptions.RemoveEmptyEntries);
            return data;
        }

        /// <summary>
        /// 셀 생성 
        /// </summary>
        /// <param name="data"> 셀 데이터 </param>
        private void CreateCells(string[] data)
        {
            // 보드의 0, 0 위치 값 
            Vector2 origin = new Vector2(-315, -360);

            for (int x = 0; x < XCount; ++x)
            {
                // 짝수줄 반칸 설정 
                int shift = (x % 2 == 0) ? 0 : 30;
                
                // 셀 리스트 추가 
                _cells.Add(new List<CellModel>());
                
                for (int y = 0; y < YCount; ++y)
                {
                    int blockType = int.Parse(data[y + x * YCount]);
                    
                    // 셀 생성 
                    CellModel cell = new CellModel(x * YCount + y, origin.x + (x * 52.5f), origin.y + (y * 60) + shift, blockType);

                    // 셀 추가 
                    _cells[x].Add(cell);
                }
            }
        }

        /// <summary>
        /// 근처 셀 설정 
        /// </summary>
        private void SetNearCellLink()
        {
            for (int x = 0; x < XCount; ++x)
            {
                for (int y = 0; y < YCount; ++y)
                {
                    bool isPair = x % 2 == 0;
                    CellModel leftTop = null;
                    CellModel top = null;
                    CellModel rightTop = null;
                    CellModel leftBottom = null;
                    CellModel bottom = null;
                    CellModel rightBottom = null;

                    // 왼쪽 
                    if (x > 1)
                    {
                        if (isPair)
                        {
                            leftTop = _cells[x-1][y];
                            if(y > 0)
                                leftBottom = _cells[x-1][y-1];
                        }
                        else
                        {
                            if(y < YCount - 1)
                                leftTop = _cells[x-1][y+1];
                            leftBottom = _cells[x-1][y];
                        }
                    }

                    // 오른쪽 
                    if (x < XCount - 1)
                    {
                        if (isPair)
                        {
                            rightTop = _cells[x+1][y];
                            if(y > 0)
                                rightBottom = _cells[x+1][y-1];
                        }
                        else
                        {
                            if(y < YCount - 1)
                                rightTop = _cells[x+1][y+1];
                            rightBottom = _cells[x+1][y];
                        }
                    }
                    
                    // 위
                    if(y < YCount - 1)
                        top = _cells[x][y+1];
                    
                    // 아래 
                    if(y > 0)
                        bottom = _cells[x][y-1];

                    _cells[x][y].SetNear(leftTop, top, rightTop, leftBottom, bottom, rightBottom);
                }
            }
        }


        public CellModel SelectCell(Vector2 position)
        {
            foreach (var cellList in _cells)
            {
                foreach (var cell in cellList)
                {
                    // 비활성 셀 검사하지 않음 
                    if(!cell.IsEnable)
                        continue;
                    
                    if (cell.IsContains(position))
                        return cell;
                }
            }

            return null;
        }

        public List<BlockMoveData> SwapBlock(CellModel touchCell, CellModel targetCell)
        {
            return touchCell.SwapBlock(targetCell);
        }
    }
}
