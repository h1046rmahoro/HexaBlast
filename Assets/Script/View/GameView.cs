using System;
using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class GameView : MonoBehaviour
    {
        /// <summary>
        /// 연출 종료 이벤트 
        /// </summary>
        public event Action OnEffectFinish = null;
        
        /// <summary>
        /// 셀 부모 오브젝트 
        /// </summary>
        [SerializeField]
        private Transform _transCells = null;

        /// <summary>
        /// 블록 부모 오브젝트 
        /// </summary>
        [SerializeField]
        private Transform _transBlocks = null;
        
        /// <summary>
        /// 셀 프리팹 
        /// </summary>
        [SerializeField]
        private GameObject _goCell = null;

        /// <summary>
        /// 블록 프리팹 
        /// </summary>
        [SerializeField]
        private GameObject _goBlock = null;

        private Dictionary<int, BlockView> _blockViews = new Dictionary<int, BlockView>();

        /// <summary>
        /// 최초 보드 생성 
        /// </summary>
        /// <param name="cells"> 보드 정보 </param>
        public void InitBoard(List<List<CellModel>> cells)
        {
            for (int x = 0; x < cells.Count; ++x)
            {
                for (int y = 0; y < cells[x].Count; ++y)
                {
                    if(!cells[x][y].IsEnable)
                        continue;
                    
                    GameObject go = Instantiate(_goCell, _transCells);
                    go.name = $"Cell-{y + x * GameModel.YCount}";
                    go.transform.position = cells[x][y].Position;

                    if (cells[x][y].Block != null)
                    {
                        BlockModel blockModel = cells[x][y].Block;
                        
                        // 블록 생성 
                        go = Instantiate(_goBlock, _transBlocks);
                        go.name = $"Block-{blockModel.UniqueKey}";
                        go.transform.position = cells[x][y].Position;
                        
                        // 블록 뷰 설정 
                        BlockView blockView = go.GetComponent<BlockView>();
                        
                        // 블록 이미지 변경 
                        blockView.SetSprite(blockModel.BlockType);
                        
                        // 블록 추가 
                        _blockViews.Add(blockModel.UniqueKey, blockView);
                    }
                }
            }
        }

        private int _effectCount = 0;

        public void BlockEffect(List<EffectData> effectDatas)
        {
            foreach (var data in effectDatas)
            {
                var blockView = _blockViews[data.UniqueKey];

                ++_effectCount;
                blockView.EffectAction(data, EffectFinish);
            }
        }

        private void EffectFinish()
        {
            --_effectCount;
            
            if(_effectCount == 0)
                OnEffectFinish?.Invoke();
        }
    }
}
