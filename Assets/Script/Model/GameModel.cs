using System;
using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class GameModel
    {
        /// <summary>
        /// 블록 스왑 이벤트 
        /// </summary>
        public event Action<List<BlockMoveData>> OnBlockMove = null; 

        /// <summary>
        /// 보드 모델 
        /// </summary>
        private BoardModel _boardModel = new BoardModel();
 
        /// <summary>
        /// 셀 데이터  
        /// </summary>
        public List<List<CellModel>> CellDataList => _boardModel.CellDataList;

        /// <summary>
        /// 터치 시작한 셀 
        /// </summary>
        private CellModel touchCell = null;

        /// <summary>
        /// 블록 스왑 시도 
        /// </summary>
        /// <param name="touchPhase"> 터치 종류 </param>
        /// <param name="position"> 터치 좌표 </param>
        public void SwapBlock(TouchPhase touchPhase, Vector2 position)
        {
            if (touchPhase == TouchPhase.Began)
            {
                // 셀 선택 
                touchCell = _boardModel.SelectCell(position);

                // 선택된 셀이 없음 
                if (touchCell == null)
                    return;

                // 셀 스왑 가능여부 체크 
                if (!touchCell.IsSwapAble)
                    touchCell = null;
            }
            else if (touchPhase == TouchPhase.Moved)
            {
                // 최초 선택 블록 없으면 작동하지 않음 
                if (touchCell == null)
                    return;

                // 같은 블록 내부는 체크하지 않음 
                if (Vector2.SqrMagnitude(position - touchCell.Position) <= 700)
                    return;
                
                // 셀 선택 
                CellModel cellModel = _boardModel.SelectCell(position);

                // 빈 셀로 이동 불가 
                if (cellModel == null)
                    return;
                
                // 같은 블록 체크하지 않음 
                if (cellModel == touchCell)
                    return;
                
                // 셀 스왑 가능여부 체크 
                if (!cellModel.IsSwapAble)
                    return;

                Debug.Log($"swap : {touchCell.Index} - {cellModel.Index}");
                var blockMoveData =  _boardModel.SwapBlock(touchCell, cellModel);
                
                // 스왑 이벤트 호출 
                OnBlockMove?.Invoke(blockMoveData);
                
                touchCell = null;
            }
            
            
        }
    }
}
