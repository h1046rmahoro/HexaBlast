using System;
using System.Collections;
using UnityEngine;

namespace SB
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _sprRenderer = null;
        
        [SerializeField]
        private Sprite[] _sprNormal = null;

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
                case BlockModel.Type.Top:
                    _sprRenderer.sprite = _sprNormal[6];
                    break;
                default:
                    break;
            }
        }

        private float speed = 300.0f;

        public void Move(BlockMoveData moveData)
        {
            StopAllCoroutines();
            StartCoroutine(MoveRoutine(moveData.TargetPos, moveData.BottomCell));
        }

        private IEnumerator MoveRoutine(Vector2 targetPos, CellModel bottom)
        {
            Vector3 origin = transform.position;
            Vector3 target = targetPos;


            while (transform.position != target)
            {
                 var pos = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                 transform.position = pos;
                 yield return null;
            }
        }
    }
}
