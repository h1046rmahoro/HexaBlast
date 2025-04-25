using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _sprRenderer = null;
        
        [SerializeField]
        private Sprite[] _sprNormal = null;

        public int UniqueKey { get; set; }            


        /// <summary>
        /// 블록 이미지 설정 
        /// </summary>
        /// <param name="type"></param>
        public void SetSprite(BlockModel.Type type)
        {
            switch (type)
            {
                case BlockModel.Type.Blue:
                case BlockModel.Type.Green:
                case BlockModel.Type.Orange:
                case BlockModel.Type.Purple:
                case BlockModel.Type.Red:
                case BlockModel.Type.Yellow:
                    _sprRenderer.sprite = _sprNormal[((int)type)-1];
                    break;
                case BlockModel.Type.Top_2:
                    _sprRenderer.sprite = _sprNormal[6];
                    break;
                case BlockModel.Type.Top_1:
                    _sprRenderer.sprite = _sprNormal[7];
                    break;
                default:
                    break;
            }
        }

        private float speed = 300.0f;

        private Coroutine moveRoutine = null;

        private Queue<EffectData> _blockEffectDatas = new Queue<EffectData>();

        public void EffectAction(EffectData moveData, Action<int, int> finishCallback)
        {
            _blockEffectDatas.Enqueue(moveData);
            
            if (moveRoutine == null)
            {
                moveRoutine = StartCoroutine(EffectRoutine(finishCallback));                
            }
        }

        private IEnumerator EffectRoutine(Action<int, int> finishCallback)
        {
            while(_blockEffectDatas.Count > 0)
            {
                var data = _blockEffectDatas.Dequeue();

                switch (data.Type)
                {
                    case EffectData.EffectType.Move:
                        yield return MoveRoutine(data, finishCallback);
                        break;
                    case EffectData.EffectType.Damage:
                        if(data.BlockType == BlockModel.Type.Top_2)
                        {
                            SetSprite(BlockModel.Type.Top_1);
                        }

                        if (data.BlockHp <= 0)
                        {
                            _sprRenderer.enabled = false;
                        }

                        finishCallback.Invoke(UniqueKey, _blockEffectDatas.Count);
                        break;
                    default:
                        break;
                }
            }

            moveRoutine = null;
        }

        private IEnumerator MoveRoutine(EffectData data, Action<int, int> finishCallback)
        {
            Vector3 origin = transform.position;
            Vector3 target = data.MoveData.TargetPos;
                
            while (transform.position != target)
            {
                var pos = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                transform.position = pos;
                yield return null;
            }
            
            finishCallback.Invoke(UniqueKey, _blockEffectDatas.Count);
        }
    }
}
