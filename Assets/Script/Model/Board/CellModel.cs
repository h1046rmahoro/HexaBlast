using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    /// <summary>
    /// 보드의 각 셀 
    /// </summary>
    public class CellModel
    {
        /// <summary>
        /// 셀 활성 여부 
        /// </summary>
        public bool IsEnable = false;

        /// <summary>
        /// 셀 인덱스 
        /// </summary>
        public int Index = 0;
        
        /// <summary>
        /// 셀 터치 체크를 위한 육각형 클래스 
        /// </summary>
        private Hexagon _hexagon = null;

        /// <summary>
        /// 셀에 지정된 블록 
        /// </summary>
        private BlockModel _block = null;


        /// <summary>
        /// 스왑 가능여부 
        /// </summary>
        public bool IsSwapAble
        {
            get
            {
                if (_block == null)
                    return false;
                
                return _block.IsSwapAble;
            }
        }

        public CellModel(int index, float x, float y, int blockType) : this(index, new Vector2(x, y), blockType)
        {
            
        }
        
        public CellModel(int index, Vector2 position, int blockType)
        {
            Index = index;
            _hexagon = new Hexagon(position);

            if (blockType != 0)
            {
                IsEnable = true;
                
                if (blockType != 99)
                {
                    _block = new BlockModel((BlockModel.Type)blockType);
                }
            }
        }

        /// <summary>
        /// 좌상단 셀 
        /// </summary>
        public CellModel LeftTop = null;
        /// <summary>
        /// 상단 셀
        /// </summary>
        public CellModel Top = null;
        /// <summary>
        /// 우상단 셀
        /// </summary>
        public CellModel RightTop = null;

        /// <summary>
        /// 좌하단 셀 
        /// </summary>
        public CellModel LeftBottom = null;
        /// <summary>
        /// 하단 셀
        /// </summary>
        public CellModel Bottom = null;
        /// <summary>
        /// 우하단 셀
        /// </summary>
        public CellModel RightBottom = null;

        /// <summary>
        /// 셀 좌표 
        /// </summary>
        public Vector2 Position => _hexagon.Center;

        /// <summary>
        /// 셀에 할당된 블록 
        /// </summary>
        public BlockModel Block
        {
            get => _block;
            set => _block = value;
        }

        /// <summary>
        /// 근처 셀 설정 
        /// </summary>
        /// <param name="leftTop"> 좌상단 </param>
        /// <param name="top"> 상단 </param>
        /// <param name="rightTop"> 우상단 </param>
        /// <param name="leftBottom"> 좌하단 </param>
        /// <param name="bottom"> 하단 </param>
        /// <param name="rightBottom"> 우하단 </param>
        public void SetNear(CellModel leftTop, CellModel top, CellModel rightTop, CellModel leftBottom,
            CellModel bottom, CellModel rightBottom)
        {
            LeftTop = leftTop;
            Top = top;
            RightTop = rightTop;
            LeftBottom = leftBottom;
            Bottom = bottom;
            RightBottom = rightBottom;
        }

        /// <summary>
        /// 터치 포함 여부 검사 
        /// </summary>
        /// <param name="position"> 터치 위치 </param>
        /// <returns> 포함 여부 </returns>
        public bool IsContains(Vector2 position)
        {
            return _hexagon.IsContainsPosition(position);
        }

        public List<BlockMoveData> SwapBlock(CellModel cell)
        {
            List<BlockMoveData> moveData = new List<BlockMoveData>();

            moveData.AddRange(Swap(cell, LeftTop));
            moveData.AddRange(Swap(cell, Top));
            moveData.AddRange(Swap(cell, RightTop));
            moveData.AddRange(Swap(cell, LeftBottom));
            moveData.AddRange(Swap(cell, Bottom));
            moveData.AddRange(Swap(cell, RightBottom));

            return moveData;
        }

        private List<BlockMoveData> Swap(CellModel swapTarget, CellModel nearCell)
        {
            List<BlockMoveData> moveData = new List<BlockMoveData>();
            
            if (swapTarget.Index != nearCell.Index)
                return moveData;

            // 블록 스왑 
            (swapTarget.Block, Block) = (Block, swapTarget.Block);


            BlockMoveData data = new BlockMoveData
            {
                TargetBlockUniqueKey = Block.UniqueKey,
                TargetPos = Position,
                BottomCell = Bottom
            };
            moveData.Add(data);

            data = new BlockMoveData()
            {
                TargetBlockUniqueKey = swapTarget.Block.UniqueKey,
                TargetPos = swapTarget.Position,
                BottomCell = swapTarget.Bottom
            };
            moveData.Add(data);

            return moveData;
        }
    }
}
