using System;
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
            Application.targetFrameRate = 60;
            
            _gameModel = new GameModel();
            _gameModel.OnBlockEffect += BlockEffect;

            gameView.OnEffectFinish += EffectFinish;
            gameView.OnEffectComplete += BlockEffectComplete;
            gameView.OnTouch += Touch;
            gameView.InitBoard(_gameModel.CellDataList);
        }

        private void Update()
        {
            // _gameModel.Update();
        }

        public void Touch(TouchPhase touchPhase, Vector2 position)
        {
            _gameModel.SwapBlock(touchPhase, position);
        }

        private void BlockEffect(List<EffectData> effectDatas)
        {
            gameView.BlockEffect(effectDatas);
        }

        private void EffectFinish()
        {
            _gameModel.EffectFinish();
        }

        private void BlockEffectComplete(int uniqueKey)
        {
            _gameModel.BlockEffectComplete(uniqueKey);
        }
    }
}
