using System;
using System.Collections.Generic;
using NUnit.Compatibility;
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
        /// 셀에 할당된 블록 
        /// </summary>
        public BlockModel Block
        {
            get => _block;
            set => _block = value;
        }

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

        /// <summary>
        /// 임시 셀 이동 불가 설정 
        /// </summary>
        public bool IsHold = false;

        public CellModel(int index, float x, float y) : this(index, new Vector2(x, y))
        {

        }

        public CellModel(int index, Vector2 position)
        {
            Index = index;
            _hexagon = new Hexagon(position);
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

        public BlockModel CreateBlock(int blockType)
        {
            if (blockType != 0)
            {
                if (blockType != 99)
                {
                    _block = new BlockModel((BlockModel.Type)blockType);
                }
            }

            return _block;
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
        /// 근처 셀인지 검사 
        /// </summary>
        /// <param name="cell"> 검사 할 셀 </param>
        /// <returns> 근처 셀 여부 </returns>
        public bool IsNearCell(CellModel cell)
        {
            if (cell == null)
                return false;

            if (LeftTop.Index == cell.Index)
                return true;
            if (Top.Index == cell.Index)
                return true;
            if (RightTop.Index == cell.Index)
                return true;
            if (LeftBottom.Index == cell.Index)
                return true;
            if (Bottom.Index == cell.Index)
                return true;
            if (RightBottom.Index == cell.Index)
                return true;

            return false;
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

        public List<EffectData> PullBlock()
        {
            List<EffectData> moveData = new List<EffectData>();

            // 홀드된 셀은 블록 이동 불가 
            if (IsHold)
                return moveData;
            
            if (!IsEnable)
                return moveData;
            
            if (Block != null)
                return moveData;
            
            BlockModel blockModel = null;
            CellModel topCell = Top;
            CellModel targetCell = null;

            // 상단 블록 찾기 
            blockModel = topCell.Block;
            targetCell = topCell;
            
            // 상단에 가져올 셀이 있는경우 홀드 
            if(topCell.IsEnable)
                IsHold = true;

            // 블록이 있는 경우 위치 변경 
            if (blockModel != null)
            {
                return SwapBlock(targetCell);
            }
            
            return moveData;
        }

        public List<EffectData> PullBlockSide()
        {
            List<EffectData> moveData = new List<EffectData>();

            // 홀드된 셀은 블록 이동 불가 
            if (IsHold)
                return moveData;

            if (!IsEnable)
                return moveData;
            
            if (Block != null)
                return moveData;
            
            BlockModel blockModel = null;
            CellModel targetCell = null;
            
            // 좌상단 블록 찾기 
            if (blockModel == null && LeftTop != null)
            {
                // 블록이 연출중인경우 이동 할 수 없음 
                if(LeftTop.Block != null)
                {
                    if (!LeftTop.Block.IsEffect)
                    {
                        blockModel = LeftTop.Block;
                        targetCell = LeftTop;
                    }
                }
            }
            
            // 우상단 블록 찾기 
            if (blockModel == null && RightTop != null)
            {
                // 블록이 연출중인경우 이동 할 수 없음 
                if(RightTop.Block != null)
                {
                    if (!RightTop.Block.IsEffect)
                    {
                        blockModel = RightTop.Block;
                        targetCell = RightTop;
                    }
                }
            }

            // 블록이 있는 경우 위치 변경 
            if (blockModel != null)
            {
                CellModel topCell = Top;
                while (topCell != null)
                {
                    topCell.IsHold = true;
                    topCell = topCell.Top;
                }
                
                return SwapBlock(targetCell);
            }
            
            return moveData;
            
        }

        public List<EffectData> SwapBlock(CellModel swapTarget)
        {
            List<BlockMoveData> moveData = new List<BlockMoveData>();

            // 블록 스왑 
            (swapTarget.Block, Block) = (Block, swapTarget.Block);

            BlockMoveData data;
            
            if(Block != null)
            {
                data = new BlockMoveData()
                {
                    TargetBlockUniqueKey = Block.UniqueKey,
                    TargetPos = Position,
                    BottomCell = Bottom
                };
                moveData.Add(data);
                Block.IsEffect = true;
            }

            if (swapTarget.Block != null)
            {
                data = new BlockMoveData()
                {
                    TargetBlockUniqueKey = swapTarget.Block.UniqueKey,
                    TargetPos = swapTarget.Position,
                    BottomCell = swapTarget.Bottom
                };
                moveData.Add(data);
                swapTarget.Block.IsEffect = true;
            }

            List<EffectData> effectDatas = new List<EffectData>();
            foreach (var blockMoveData in moveData)
            {
                effectDatas.Add(new EffectData()
                {
                    Type = EffectData.EffectType.Move,
                    UniqueKey = blockMoveData.TargetBlockUniqueKey,
                    MoveData = blockMoveData
                });
            }

            return effectDatas;
        }

        /// <summary>
        /// 블록 데미지 콜백 
        /// </summary>
        public event Action<List<BlockDamageData>> OnBlockDamage = null; 

        /// <summary>
        /// 방향 값 
        /// </summary>
        public enum Direction
        {
            LeftTop,
            Top,
            RightTop,
            RightBottom,
            Bottom,
            LeftBottom,
            Around
        }

        public List<BlockDamageData> MatchCheck()
        {
            BlockModel.Type type = Block.BlockType;
            List<BlockDamageData> matchBlock = new List<BlockDamageData>();

            // 매칭 할 수 없는 블록은 검사하지 않음 
            if (!Block.IsMatchAble)
                return matchBlock;

            var lt = MatchLineCheck(new MatchSearchData() { Cell = this, Direction = Direction.LeftTop }, type);
            var t = MatchLineCheck(new MatchSearchData() { Cell = this, Direction = Direction.Top }, type);
            var rt = MatchLineCheck(new MatchSearchData() { Cell = this, Direction = Direction.RightTop }, type);

            foreach (var data in lt)
            {
                bool isContains = false;
                foreach (var blockDamageData in matchBlock)
                {
                    if (blockDamageData.UniqueKey == data.UniqueKey)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                    matchBlock.Add(data);
            }
            foreach (var data in t)
            {
                bool isContains = false;
                foreach (var blockDamageData in matchBlock)
                {
                    if (blockDamageData.UniqueKey == data.UniqueKey)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                    matchBlock.Add(data);
            }
            foreach (var data in rt)
            {
                bool isContains = false;
                foreach (var blockDamageData in matchBlock)
                {
                    if (blockDamageData.UniqueKey == data.UniqueKey)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                    matchBlock.Add(data);
            }

            return matchBlock;
        }

        private List<BlockDamageData> MatchLineCheck(MatchSearchData searchData, BlockModel.Type type)
        {
            List<BlockDamageData> matchData = new List<BlockDamageData>();
            Queue<MatchSearchData> searchCell = new Queue<MatchSearchData>();

            // 현재 셀 추가 
            searchCell.Enqueue(searchData);
            
            // 반대방향 추가 
            var reverse = (Direction)(((int)searchData.Direction + 3) % 6);

            switch (reverse)
            {
                case Direction.RightBottom:
                    searchCell.Enqueue(new MatchSearchData(){Cell = searchData.Cell.RightBottom, Direction = reverse});
                    break;
                case Direction.Bottom:
                    searchCell.Enqueue(new MatchSearchData(){Cell = searchData.Cell.Bottom, Direction = reverse});
                    break;
                case Direction.LeftBottom:
                    searchCell.Enqueue(new MatchSearchData(){Cell = searchData.Cell.LeftBottom, Direction = reverse});
                    break;
                case Direction.Around:
                    break;
                case Direction.LeftTop:
                case Direction.Top:
                case Direction.RightTop:
                default:
                    break;
            }
            
            // 큐 검사 
            while (searchCell.Count > 0)
            {
                // 검사 셀 
                var data = searchCell.Dequeue();
                
                // 빈 셀 검사 하지 않음 
                if(data.Cell == null)
                    continue;

                // 빈 블록 검사 하지 않음 
                if (data.Cell.Block == null)
                    continue;
                
                // 이미 검사 셀 여부 체크
                bool isContains = false;
                foreach (var blockDamageData in matchData)
                {
                    if (blockDamageData.UniqueKey == data.Cell.Block.UniqueKey)
                    {
                        isContains = true;
                        break;
                    }
                }
                
                if (!isContains)
                {
                    // 매칭 블록 추가
                    if (data.Cell.Block.BlockType == type)
                    {
                        matchData.Add(new BlockDamageData(){Cell = data.Cell, UniqueKey = data.Cell.Block.UniqueKey});

                        // 방향에 맞는 셀 큐에 추가 
                        if (data.Direction == Direction.LeftTop)
                        {
                            searchCell.Enqueue(new MatchSearchData()
                            {
                                Cell = data.Cell.LeftTop,
                                Direction = data.Direction
                            });
                        }

                        if (data.Direction == Direction.Top)
                        {
                            searchCell.Enqueue(new MatchSearchData()
                            {
                                Cell = data.Cell.Top,
                                Direction = data.Direction
                            });
                        }

                        if (data.Direction == Direction.RightTop)
                        {
                            searchCell.Enqueue(new MatchSearchData()
                            {
                                Cell = data.Cell.RightTop,
                                Direction = data.Direction
                            });
                        }

                        if (data.Direction == Direction.LeftBottom)
                        {
                            searchCell.Enqueue(new MatchSearchData()
                            {
                                Cell = data.Cell.LeftBottom,
                                Direction = data.Direction
                            });
                        }

                        if (data.Direction == Direction.Bottom)
                        {
                            searchCell.Enqueue(new MatchSearchData()
                            {
                                Cell = data.Cell.Bottom,
                                Direction = data.Direction
                            });
                        }

                        if (data.Direction == Direction.RightBottom)
                        {
                            searchCell.Enqueue(new MatchSearchData()
                            {
                                Cell = data.Cell.RightBottom,
                                Direction = data.Direction
                            });
                        }
                    }
                }
            }
            
            if(matchData.Count < 3)
                matchData.Clear();

            return matchData;
        }
    }
}
