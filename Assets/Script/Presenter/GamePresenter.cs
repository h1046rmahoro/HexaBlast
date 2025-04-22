using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField]
        private GameView gameView = null;

        private GameModel _gameModel = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _gameModel = new GameModel();
            _gameModel.OnBlockMove += UpdateBlockMove;
            
            gameView.InitBoard(_gameModel.CellDataList);
        }

        public void Touch(TouchPhase touchPhase, Vector2 position)
        {
            _gameModel.SwapBlock(touchPhase, position);
        }

        private void UpdateBlockMove(List<BlockMoveData> moveDatas)
        {
            gameView.BlockMove(moveDatas);
        }
        
        
    }
}
