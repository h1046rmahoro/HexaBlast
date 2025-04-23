using System;
using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class GameModel
    {
        public const int XCount = 13;
        public const int YCount = 13;

        /// <summary>
        /// 블록 이펙트 재생 
        /// </summary>
        public event Action<List<EffectData>> OnBlockEffect = null;
        
        /*
        /// <summary>
        /// 블록 스왑 이벤트 
        /// </summary>
        public event Action<List<BlockMoveData>> OnBlockMove = null;
        
        
        /// <summary>
        /// 블록 데미지 이벤트 
        /// </summary>
        public event Action<List<BlockDamageData>> OnBlockDamage = null;
        */

        /// <summary>
        /// 셀 리스트 
        /// </summary>
        private List<List<CellModel>> _cells = new List<List<CellModel>>();

        /// <summary>
        /// 블록 리스트 
        /// </summary>
        private List<BlockModel> _blocks = new List<BlockModel>();

        /// <summary>
        /// 셀 데이터  
        /// </summary>
        public List<List<CellModel>> CellDataList => _cells;

        /// <summary>
        /// 터치 시작한 셀 
        /// </summary>
        private CellModel _touchCell = null;

        /// <summary>
        /// 스왑 대상 셀 
        /// </summary>
        private CellModel _targetCell = null;

        public GameModel()
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

        public enum State
        {
            Wait,
            Swap,
            Matching,
            Damage,
            SwapBack,
            Move,
            MoveCheck
        }

        /// <summary>
        /// 현재 게임 상태 
        /// </summary>
        private State GameState = State.Wait;


        public void EffectFinish()
        {
            Debug.Log($"state before : {GameState}");
            switch (GameState)
            {
                case State.Wait:
                    break;
                case State.Swap:
                    // 상태 변경 
                    GameState = State.Matching;
                    
                    // 스왑 후 매칭 검사 
                    CheckSwapMatch();
                    break;
                case State.Matching:
                    break;
                case State.Damage:
                    // GameState = State.Move;
                    GameState = State.Wait;
                    break;
                case State.SwapBack:
                    GameState = State.Wait;
                    break;
                case State.Move:
                    break;
                case State.MoveCheck:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Debug.Log($"state : {GameState}");
        }

        private void CheckSwapMatch()
        {
            var matchOrigin = _touchCell.MatchCheck();
            var matchTarget = _targetCell.MatchCheck();

            List<BlockDamageData> matchData = new List<BlockDamageData>();
            foreach (var damageData in matchOrigin)
            {
                bool isContains = false;
                foreach (var blockDamageData in matchOrigin)
                {
                    if (blockDamageData.UniqueKey == damageData.UniqueKey)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                    matchData.Add(damageData);
            }
            
            foreach (var damageData in matchTarget)
            {
                bool isContains = false;
                foreach (var blockDamageData in matchOrigin)
                {
                    if (blockDamageData.UniqueKey == damageData.UniqueKey)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                    matchData.Add(damageData);
            }

            Debug.Log($"match Data : {matchData.Count}");

            if (matchData.Count >= 3)
            {
                //
                BlockDamageEvent(matchData);

            }
            else
            {   // 스왑 되돌아가기 
                GameState = State.SwapBack;
                var blockMoveData = SwapBlock(_touchCell, _targetCell);
                
                // 스왑 이벤트 호출 
                OnBlockEffect?.Invoke(blockMoveData);
            }
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
                    CellModel cell = new CellModel(x * YCount + y, origin.x + (x * 52.5f), origin.y + (y * 60) + shift);
                    
                    // 셀 활성여부 설정 
                    cell.IsEnable = blockType != 0;

                    // 블록 생성 
                    var block = cell.CreateBlock(blockType);
                    if (block != null)
                        _blocks.Add(block);

                    // 블록 데미지 이벤트 추가 
                    cell.OnBlockDamage += BlockDamageEvent;

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

        private bool _isSwapAble = false;
        
        /// <summary>
        /// 블록 스왑 시도 
        /// </summary>
        /// <param name="touchPhase"> 터치 종류 </param>
        /// <param name="position"> 터치 좌표 </param>
        public void SwapBlock(TouchPhase touchPhase, Vector2 position)
        {
            // 대기중이 아닌경우 조작 불가 
            if (GameState != State.Wait)
                return;
            
            if (touchPhase == TouchPhase.Began)
            {
                // 셀 선택 
                _touchCell = SelectCell(position);

                // 선택된 셀이 없음 
                if (_touchCell == null)
                    return;

                // 셀 스왑 가능여부 체크 
                if (!_touchCell.IsSwapAble)
                    _touchCell = null;

                // 스왑 가능 설정 
                _isSwapAble = true;
            }
            else if (touchPhase == TouchPhase.Moved)
            {
                if (!_isSwapAble)
                    return;
                
                // 최초 선택 블록 없으면 작동하지 않음 
                if (_touchCell == null)
                    return;

                // 같은 블록 내부는 체크하지 않음 
                if (Vector2.SqrMagnitude(position - _touchCell.Position) <= 700)
                    return;
                
                // 셀 선택 
                CellModel cellModel = SelectCell(position);

                // 빈 셀로 이동 불가 
                if (cellModel == null)
                    return;
                
                // 같은 블록 체크하지 않음 
                if (cellModel == _touchCell)
                    return;
                
                // 셀 스왑 가능여부 체크 
                if (!cellModel.IsSwapAble)
                    return;

                // 근처 셀이 아니면 스왑 불가 
                if (!_touchCell.IsNearCell(cellModel))
                    return;

                _targetCell = cellModel;

                // 블록 스왑
                _isSwapAble = false;
                GameState = State.Swap;
                var blockMoveData =  SwapBlock(_touchCell, _targetCell);
                
                // 스왑 이벤트 호출 
                OnBlockEffect?.Invoke(blockMoveData);

            }
        }

        public List<EffectData> SwapBlock(CellModel touchCell, CellModel targetCell)
        {
            return touchCell.SwapBlock(targetCell);
        }

        public void BlockDamageEvent(List<BlockDamageData> damageData)
        {
            List<EffectData> effectDatas = new List<EffectData>();

            // 블록 데미지 설정 
            foreach (var data in damageData)
            {
                data.Cell.Block.Hp -= 1;
                if (data.Cell.Block.Hp <= 0)
                    data.Cell.Block = null;
                
                effectDatas.Add(new EffectData()
                {
                    Type = EffectData.EffectType.Damage,
                    UniqueKey = data.UniqueKey,
                    DamageData = data
                });
            }

            GameState = State.Damage;
            OnBlockEffect?.Invoke(effectDatas);
        }

        private void BlockGenerate()
        {
            
        }

        private void MoveBlock()
        {
            
        }

        private void MatchingCheck()
        {
            
        }
        
        
    }
}
